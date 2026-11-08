using DiGi.Core.Classes;
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
    /// Represents a task for posting Building 2D objects to a PostgreSQL database from GIS model files selected through the user interface.
    /// </summary>
    public class UIBuilding2DsFromFilePostTask : Building2DsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIBuilding2DsFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to communicate with the GIS PostgreSQL Web API.</param>
        public UIBuilding2DsFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for reporting progress of the operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the operation succeeded; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await ExecuteAsync(progress, cancellationToken);
            }

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

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            foreach (string path_GISModel in paths_GISModel)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(path_GISModel) || !File.Exists(path_GISModel))
                {
                    return false;
                }

                using GISModelFile gISModelFile = new(path_GISModel);

                gISModelFile.Open();

                GISModel? gISModel = gISModelFile.Value;

                cancellationToken.ThrowIfCancellationRequested();

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

                bool succeeded = false;
                try
                {
                    succeeded = await ExecuteAsync(building2Ds, code_GISModel, longProgressWrapper, cancellationToken);
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

            return true;
        }
    }
}
