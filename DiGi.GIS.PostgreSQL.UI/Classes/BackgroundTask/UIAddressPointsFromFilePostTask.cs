using DiGi.GIS.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using DiGi.PRG.Geometry.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Imports PRG address points from GML files selected through the user interface and posts them to the server.
    /// <para>The PRG is published as one GML file per voivodeship (0.7 - 1.6 GB each), not per county, so the county of every point is taken from its own TERYT commune code (see <see cref="Query.CountyCode(string?)"/>). Each code is resolved to every polygon part of the county through <see cref="AdministrativeAreal2DController.GetIdsByCodeAsync"/> and the points are posted with those identifiers; the server files each point under the part it lies in.</para>
    /// <para>Files are loaded one at a time and released before the next, because a loaded file holds a clone of its street and locality on every point (about 3 GB for a 714 MB file). A county that fails is named, recorded in the report and skipped, and the run continues.</para>
    /// <para>A report is written to <see cref="ReportDirectory"/> and names the server the run posted to, so it cannot be mistaken for a description of another database.</para>
    /// </summary>
    public class UIAddressPointsFromFilePostTask : AddressPointsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIAddressPointsFromFilePostTask"/> class.
        /// </summary>
        /// <param name="GISWebAPIManager">The manager used to communicate with the GIS Web API.</param>
        public UIAddressPointsFromFilePostTask(GISWebAPIManager GISWebAPIManager)
            : base(GISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the directory the report is written into. When null the directory the application was launched from is used.
        /// <para>Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment.</para>
        /// </summary>
        public string? ReportDirectory { get; set; } = null;

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for reporting progress of the operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if every county was posted; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want select single PRG address points GML file?", "Selection", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return false;
            }

            List<string> paths_GML = [];
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "Select PRG address points GML file",
                    Filter = "GML file (*.gml)|*.gml|All files (*.*)|*.*"
                };

                bool? dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                paths_GML.Add(openFileDialog.FileName);
            }
            else
            {
                OpenFolderDialog openFolderDialog = new();
                bool? dialogResult = openFolderDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                string directory_Input = openFolderDialog.FolderName;
                if (string.IsNullOrWhiteSpace(directory_Input) || !Directory.Exists(directory_Input))
                {
                    return false;
                }

                paths_GML = [.. Directory.GetFiles(directory_Input, "*.gml", SearchOption.AllDirectories)];
            }

            if (paths_GML.Count == 0)
            {
                return false;
            }

            string? directory_Report = ReportDirectory;
            if (string.IsNullOrWhiteSpace(directory_Report))
            {
                directory_Report = AppContext.BaseDirectory;
            }

            if (string.IsNullOrWhiteSpace(directory_Report) || !Directory.Exists(directory_Report))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Report directory does not exist");
                return false;
            }

            HttpClient? httpClient = GISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetIdsByCodeAsync), out string? path_Ids);
            if (httpClient is null || string.IsNullOrWhiteSpace(path_Ids))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County identifiers endpoint is not available");
                return false;
            }

            // Names the server this run describes - the report is read long after the run, possibly beside another server's.
            string server = httpClient.BaseAddress?.ToString() ?? "unknown";

            List<string> lines_Report =
            [
                $"Server: {server}",
                $"Started: {DateTimeOffset.Now:O}"
            ];

            Core.Classes.LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            PostOptions postOptions = new() { RequestResult = true };

            Dictionary<string, HashSet<int>?> countyIds_Codes = [];
            List<string> codes_Failed = [];

            foreach (string path_GML in paths_GML)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(path_GML) || !File.Exists(path_GML))
                {
                    lines_Report.Add($"File missing: {path_GML}");
                    codes_Failed.Add(path_GML);
                    continue;
                }

                // Grouped and converted inside a scope of their own, so the loaded points are unreachable before the next file loads.
                Dictionary<string, List<AddressPoint>> dictionary = [];
                int count_Loaded = 0;
                int count_Skipped = 0;
                {
                    List<PunktAdresowy>? punktAdresowes = DiGi.PRG.Geometry.Create.PunktyAdresowe(path_GML);
                    if (punktAdresowes is null)
                    {
                        lines_Report.Add($"File could not be loaded: {path_GML}");
                        codes_Failed.Add(path_GML);
                        continue;
                    }

                    count_Loaded = punktAdresowes.Count;
                    foreach (PunktAdresowy punktAdresowy in punktAdresowes)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        string? code = Query.CountyCode(punktAdresowy.TERYTGminy);
                        AddressPoint? addressPoint = punktAdresowy.ToDiGi();
                        if (code is null || addressPoint is null)
                        {
                            count_Skipped++;
                            continue;
                        }

                        if (!dictionary.TryGetValue(code, out List<AddressPoint>? addressPoints))
                        {
                            addressPoints = [];
                            dictionary[code] = addressPoints;
                        }

                        addressPoints.Add(addressPoint);
                    }
                }

                lines_Report.Add($"File: {path_GML} - loaded {count_Loaded}, skipped {count_Skipped} (no commune code or no position), {dictionary.Count} counties");
                Serilog.Modify.Log("{Path}: loaded {Loaded}, skipped {Skipped}, {Count} counties", path_GML, count_Loaded, count_Skipped, dictionary.Count);

                foreach (KeyValuePair<string, List<AddressPoint>> keyValuePair in dictionary.OrderBy(x => x.Key))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string code = keyValuePair.Key;

                    if (!countyIds_Codes.TryGetValue(code, out HashSet<int>? countyIds))
                    {
                        // The administrativearealtype parameter is not nullable-safe on omission - always sent.
                        string requestUri = new UrlBuilder(path_Ids).AddParameter("code", code).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).ToString();

                        PostResponse<HashSet<int>?> postResponse = await DiGi.WebAPI.Query.GetAsync<HashSet<int>>(httpClient, requestUri, postOptions);
                        countyIds = postResponse is not null && postResponse.Succeeded ? postResponse.Result : null;
                        countyIds_Codes[code] = countyIds;
                    }

                    if (countyIds is null || countyIds.Count == 0)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County {Code} has no county rows - {Count} address points skipped", code, keyValuePair.Value.Count);
                        lines_Report.Add($"County {code}: no county rows, {keyValuePair.Value.Count} points NOT posted");
                        codes_Failed.Add(code);
                        continue;
                    }

                    bool succeeded;
                    try
                    {
                        succeeded = await ExecuteAsync(keyValuePair.Value, countyIds, longProgressWrapper, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County {Code} failed: {Message}", code, exception.Message);
                        succeeded = false;
                    }

                    if (!succeeded)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County {Code} failed - the run continues with the next county", code);
                        lines_Report.Add($"County {code}: FAILED, {keyValuePair.Value.Count} points (county ids {string.Join(", ", countyIds)})");
                        codes_Failed.Add(code);
                        continue;
                    }

                    lines_Report.Add($"County {code}: posted {keyValuePair.Value.Count} points (county ids {string.Join(", ", countyIds)})");
                }
            }

            lines_Report.Add($"Ended: {DateTimeOffset.Now:O}");
            lines_Report.Add(codes_Failed.Count == 0 ? "Failed: none" : $"Failed: {string.Join(", ", codes_Failed)}");

            string path_Report = Path.Combine(directory_Report, "AddressPoints_Import_Report.txt");
            await File.WriteAllLinesAsync(path_Report, lines_Report, cancellationToken);

            Serilog.Modify.Log("Address points import ended. Report written to {Path}", path_Report);

            return codes_Failed.Count == 0;
        }
    }
}
