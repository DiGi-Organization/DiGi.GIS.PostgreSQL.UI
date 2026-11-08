using DiGi.GIS.Classes;
using DiGi.GIS.Constants;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Represents a task that extracts year built data from GIS model files and posts it to the PostgreSQL database.
    /// </summary>
    public class UIYearBuiltDatasFromFilePostTask : YearBuiltDatasPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIYearBuiltDatasFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to handle communication with the PostgreSQL Web API.</param>
        public UIYearBuiltDatasFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for reporting progress of the operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the process succeeded; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            string? code = Code;

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want select single GIS Model file?", "Selection", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return false;
            }

            List<string> paths_GISModel = [];
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "Select GIS Model file",
                    Filter = string.Format(CultureInfo.CurrentCulture, "{0} (*.{1})|*.{1}|All files (*.*)|*.*", FileTypeName.GISModelFile, FileExtension.GISModelFile)
                };

                bool? dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                paths_GISModel.Add(openFileDialog.FileName);
            }
            else
            {
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

                string[] paths_Input = Directory.GetFiles(directory, "*." + FileExtension.GISModelFile, SearchOption.AllDirectories);
                if (paths_Input == null || paths_Input.Length == 0)
                {
                    return false;
                }

                paths_GISModel = [.. paths_Input];
            }

            if (paths_GISModel is null || paths_GISModel.Count == 0)
            {
                return true;
            }

            Core.Classes.LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            foreach (string path_GISModel in paths_GISModel)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(path_GISModel) || !File.Exists(path_GISModel))
                {
                    return false;
                }

                using GISModelFile gISModelFile = new(path_GISModel);

                gISModelFile.Open();

                cancellationToken.ThrowIfCancellationRequested();

                GISModel? gISModel = gISModelFile.Value;

                List<Building2D>? building2Ds = gISModel?.GetObjects<Building2D>();

                cancellationToken.ThrowIfCancellationRequested();

                if (building2Ds is null || building2Ds.Count == 0)
                {
                    continue;
                }

                cancellationToken.ThrowIfCancellationRequested();

                string? code_GISModel = gISModel!.Reference;
                if (!string.IsNullOrWhiteSpace(code_GISModel))
                {
                    code_GISModel = code_GISModel.ToUpper(CultureInfo.CurrentCulture);
                    int index = code_GISModel.IndexOf('_');
                    if (index != -1)
                    {
                        code_GISModel = code_GISModel[..index];
                    }
                }

                List<Building2D>? building2Ds_Split;

                Core.Classes.SizeSplitter<Building2D> sizeSplitter = new(building2Ds);
                while ((building2Ds_Split = sizeSplitter.Next(100)) is not null)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    List<string> references = [];
                    foreach (Building2D building2D in building2Ds_Split)
                    {
                        if (building2D?.Reference is string reference && !string.IsNullOrWhiteSpace(reference))
                        {
                            references.Add(reference);
                        }
                    }

                    Dictionary<string, YearBuiltData>? dictionary = GIS.Query.YearBuiltDataDictionary<YearBuiltData>(gISModelFile, references);
                    if (dictionary is null || dictionary.Count == 0)
                    {
                        continue;
                    }

                    bool succeeded = false;
                    try
                    {
                        succeeded = await ExecuteAsync(dictionary.Values, code_GISModel, longProgressWrapper, cancellationToken);
                    }
                    catch
                    {
                        throw;
                    }

                    if (!succeeded)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
