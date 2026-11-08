using DiGi.CityGML.Classes;
using DiGi.Core.Parameter.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A UI-driven post task that prompts the user to select a directory, reads CityGML city models from it, extracts <see cref="Building"/> instances, determines the county code from the file path, and uploads them to the server in batches.
    /// </summary>
    public class UIBuildingsFromDirectoryPostTask : BuildingsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIBuildingsFromDirectoryPostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the server.</param>
        public UIBuildingsFromDirectoryPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
            // A bulk import posts batches that take the server far longer than an interactive request:
            // it deserializes thousands of buildings with full 3D geometry and upserts them in one go.
            // The 20s default bounds a single attempt and would cancel a large county file client-side.
            SerializableObjectsPostOptions.Delay = TimeSpan.FromMinutes(3);
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            OpenFolderDialog openFolderDialog = new();
            bool? dialogResult = openFolderDialog.ShowDialog();
            if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
            {
                return false;
            }

            string directory = openFolderDialog.FolderName;
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return false;
            }

            string? source_Resume = null;

            // The last written building records the file it came from, so an interrupted import can pick
            // up there instead of walking the whole directory again.
            string? source_Latest = await LatestSourceAsync(gISWebAPIManager);
            if (!string.IsNullOrWhiteSpace(source_Latest))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(string.Format("The last building on the server came from:\n\n{0}\n\nContinue from that file?\n\nYes - resume from it.\nNo - import the whole directory from the start.", source_Latest), "Continue", MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.Cancel)
                {
                    return false;
                }

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    source_Resume = source_Latest;
                }
            }

            Core.Classes.LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            // Parsing a city model and uploading a batch are independent: one is CPU bound, the other
            // waits on the server. A bounded channel lets the next file be parsed while the previous
            // batch is in flight, without ever putting more than one POST on the wire.
            Channel<BuildingsBatch> channel = Channel.CreateBounded<BuildingsBatch>(new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = true,
            });

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            ResumeFilter? resumeFilter = source_Resume is null ? null : new(source_Resume);

            Task<bool> task_Producer = ProduceAsync(directory, channel.Writer, resumeFilter, cancellationTokenSource.Token);

            bool result = true;

            try
            {
                await foreach (BuildingsBatch buildingsBatch in channel.Reader.ReadAllAsync(cancellationToken))
                {
                    if (!await ExecuteAsync(buildingsBatch.Buildings, buildingsBatch.Code, longProgressWrapper, cancellationToken))
                    {
                        result = false;

                        // Stop the producer before it parses another file for a run that is already lost.
                        cancellationTokenSource.Cancel();
                        break;
                    }
                }
            }
            finally
            {
                cancellationTokenSource.Cancel();

                try
                {
                    result &= await task_Producer;
                }
                catch (OperationCanceledException)
                {
                    // Expected once the consumer has stopped the walk.
                }
            }

            // A resume that never matched means the recorded file is no longer in this directory - report
            // it rather than letting an import that silently did nothing look like a success.
            if (result && resumeFilter is not null && !resumeFilter.Matched)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Resume source {Source} was not found under {Directory}; nothing was imported", source_Resume ?? string.Empty, directory);

                MessageBox.Show(string.Format("Could not resume: no file matching\n\n{0}\n\nwas found under the selected directory. Nothing has been imported.", source_Resume), "Continue", MessageBoxButton.OK);

                return false;
            }

            return result;
        }

        /// <summary>
        /// Asynchronously reads the source path recorded on the most recently written building.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to reach the server.</param>
        /// <returns>A task whose result is the recorded source path, or null when the server holds no buildings or the path was not recorded.</returns>
        private static async Task<string?> LatestSourceAsync(GISWebAPIManager? gISWebAPIManager)
        {
            if (gISWebAPIManager is null)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<BuildingController>(nameof(BuildingController.GetItemByLatestCreatedAtAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            // No countyid: the walk spans counties, so the globally latest building is the one wanted.
            string requestUri = new UrlBuilder(path).ToString();

            PostResponse<Building?> postResponse;

            try
            {
                postResponse = await DiGi.WebAPI.Query.GetAsync<Building>(httpClient, requestUri, new PostOptions() { RequestResult = true });
            }
            catch (Exception exception)
            {
                // An unreachable or older server must not block a plain import.
                Serilog.Modify.Log(exception, "Latest building could not be retrieved; resume will not be offered");
                return null;
            }

            if (postResponse is null || !postResponse.Succeeded || postResponse.Result is not Building building)
            {
                return null;
            }

            if (!building.TryGetValue(Analytical.Enums.BuildingParameter.Source, out string? source, new GetValueSettings(true, false)))
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(source) ? null : source;
        }

        /// <summary>
        /// Walks the directory, parses each city model and publishes its tagged buildings to the channel.
        /// </summary>
        /// <param name="directory">The directory to walk.</param>
        /// <param name="channelWriter">The writer the parsed batches are published to.</param>
        /// <param name="resumeFilter">An optional filter that skips files preceding a recorded resume point, or null to walk everything.</param>
        /// <param name="cancellationToken">A token observed before each file is parsed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the walk completed successfully; otherwise, false.</returns>
        private static async Task<bool> ProduceAsync(string directory, ChannelWriter<BuildingsBatch> channelWriter, ResumeFilter? resumeFilter, CancellationToken cancellationToken)
        {
            Exception? exception = null;

            bool result;

            try
            {
                result = await CityGML.Query.RunAsync(directory, async (path, cityModel) =>
                {
                    IEnumerable<Building>? buildings = cityModel?.Buildings;
                    if (buildings is null || !buildings.Any())
                    {
                        return true;
                    }

                    GetValueSettings getValueSettings = new(true, false);

                    if (cityModel is null || !cityModel.TryGetValue(CityGML.Enums.CityModelParameter.Year, out short? year, getValueSettings))
                    {
                        year = null;
                    }

                    if (cityModel is null || !cityModel.TryGetValue(CityGML.Enums.CityModelParameter.LOD, out CityGML.Enums.LOD? lOD, getValueSettings))
                    {
                        lOD = null;
                    }

                    string? code = null;
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        if (Path.GetFileNameWithoutExtension(path) is string name)
                        {
                            if (name.Contains('_'))
                            {
                                string value = name.Split("_")[0];
                                if (string.IsNullOrWhiteSpace(code) && int.TryParse(value, out _))
                                {
                                    code = value;
                                }
                            }

                            if (string.IsNullOrWhiteSpace(code) && int.TryParse(name, out _))
                            {
                                code = name;
                            }
                        }
                    }

                    SetValueSettings setValueSettings = new(true, false);

                    // Recorded relative to the selected directory, not absolute: a resume then survives the
                    // data being moved, a different drive letter, or a different machine.
                    string source = Core.IO.Query.RelativePath(directory, path) ?? path;

                    foreach (Building building in buildings)
                    {
                        building.SetValue(Analytical.Enums.BuildingParameter.Year, year, setValueSettings);
                        building.SetValue(Analytical.Enums.BuildingParameter.LOD, lOD, setValueSettings);
                        building.SetValue(Analytical.Enums.BuildingParameter.Source, source, setValueSettings);

                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            building.SetValue(Analytical.Enums.BuildingParameter.Code, code, setValueSettings);
                        }
                    }

                    // Blocks once the channel is full, so at most one parsed city model is held ahead
                    // of the upload - the parse runs during the previous POST, not unboundedly ahead.
                    await channelWriter.WriteAsync(new BuildingsBatch(buildings, code), cancellationToken);

                    return true;
                }, resumeFilter is null ? null : path => resumeFilter.Admit(Core.IO.Query.RelativePath(directory, path) ?? path), cancellationToken);
            }
            catch (Exception exception_Temp)
            {
                exception = exception_Temp;
                result = false;
            }

            channelWriter.Complete(exception);

            return result;
        }

        /// <summary>
        /// Skips walked files until a recorded source path is reached, then admits everything from that file onward.
        /// <para>The recorded file is admitted rather than skipped: the run that wrote it was interrupted, so it was probably only partly uploaded. Re-importing it is safe because the server upserts on (county, reference, lod, year).</para>
        /// </summary>
        private sealed class ResumeFilter
        {
            private readonly string source;

            private bool matched = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResumeFilter"/> class.
            /// </summary>
            /// <param name="source">The recorded source path, relative to the walked directory, to resume from.</param>
            public ResumeFilter(string source)
            {
                this.source = source;
            }

            /// <summary>
            /// Gets a value indicating whether the recorded source path was ever reached during the walk.
            /// </summary>
            public bool Matched => matched;

            /// <summary>
            /// Decides whether a walked file should be parsed.
            /// </summary>
            /// <param name="path">The walked file's path, relative to the walked directory.</param>
            /// <returns>True once the recorded source path has been reached; otherwise, false.</returns>
            public bool Admit(string path)
            {
                if (!matched && string.Equals(path, source, StringComparison.OrdinalIgnoreCase))
                {
                    matched = true;
                }

                return matched;
            }
        }

        /// <summary>
        /// A parsed city model's buildings together with the county code derived from its file path.
        /// </summary>
        private sealed class BuildingsBatch
        {
            private readonly IEnumerable<Building> buildings;
            private readonly string? code;

            /// <summary>
            /// Initializes a new instance of the <see cref="BuildingsBatch"/> class.
            /// </summary>
            /// <param name="buildings">The buildings parsed from a single city model.</param>
            /// <param name="code">The county code derived from the source file path, or null when it could not be determined.</param>
            public BuildingsBatch(IEnumerable<Building> buildings, string? code)
            {
                this.buildings = buildings;
                this.code = code;
            }

            /// <summary>
            /// Gets the buildings parsed from a single city model.
            /// </summary>
            public IEnumerable<Building> Buildings => buildings;

            /// <summary>
            /// Gets the county code derived from the source file path.
            /// </summary>
            public string? Code => code;
        }
    }
}
