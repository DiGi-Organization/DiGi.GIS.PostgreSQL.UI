using DiGi.Core.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.Constants;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Provides functionality to post administrative areal 2D objects to a PostgreSQL database by selecting GIS model files through the user interface.
    /// </summary>
    public class UIAdministrativeAreal2DFromFilePostTask : AdministrativeAreal2DsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIAdministrativeAreal2DFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to communicate with the GIS PostgreSQL Web API.</param>
        public UIAdministrativeAreal2DFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for a value and elapsed time to report progress.</param>
        /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the operation succeeded; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
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
                    Filter = string.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", FileTypeName.GISModelFile, FileExtension.GISModelFile)
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
                if (string.IsNullOrWhiteSpace(path_GISModel) || !File.Exists(path_GISModel))
                {
                    return false;
                }

                using GISModelFile gISModelFile = new(path_GISModel);

                gISModelFile.Open();

                GISModel? gISModel = gISModelFile.Value;

                List<AdministrativeAreal2D>? administrativeAreal2Ds = gISModel?.GetObjects<AdministrativeAreal2D>();
                if (administrativeAreal2Ds is null || administrativeAreal2Ds.Count == 0)
                {
                    continue;
                }

                bool succeeded = false;
                try
                {
                    succeeded = await ExecuteAsync(administrativeAreal2Ds, longProgressWrapper, cancellationToken);
                }
                catch
                {
                    Values = null;
                    throw;
                }

                if (!succeeded)
                {
                    return false;
                }
            }

            Values = null;
            return true;
        }
    }
}
