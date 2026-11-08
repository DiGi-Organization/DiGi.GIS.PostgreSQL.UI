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
    /// Represents a task for posting orthodata from files to a PostgreSQL database, specifically designed for use within the UI layer.
    /// </summary>
    public class UIOrtoDatasFromFilePostTask : OrtoDatasPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIOrtoDatasFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to communicate with the GIS PostgreSQL Web API.</param>
        public UIOrtoDatasFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The progress reporter used to track the operation's completion percentage.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while writing the task to stop executing.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the operation succeeded; otherwise, false.</returns>
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

                string? directory_GISModel = Path.GetDirectoryName(path_GISModel);
                if (!Directory.Exists(directory_GISModel))
                {
                    return false;
                }

                string? directory_OrtoDatas = GIS.Query.OrtoDatasDirectory_Building2D(directory_GISModel);
                if (!Directory.Exists(directory_OrtoDatas))
                {
                    continue;
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

                    IEnumerable<OrtoDatas>? ortoDatas_Building2D = GIS.Query.OrtoDatasDictionary(directory_OrtoDatas, building2Ds_Split)?.Values;
                    if (ortoDatas_Building2D is null)
                    {
                        continue;
                    }

                    bool succeeded = false;
                    try
                    {
                        succeeded = await ExecuteAsync(ortoDatas_Building2D, code_GISModel, longProgressWrapper, cancellationToken);
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
