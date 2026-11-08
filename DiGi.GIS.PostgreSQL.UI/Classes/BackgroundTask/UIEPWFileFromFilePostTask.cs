using DiGi.Core.Classes;
using DiGi.EPW.Classes;
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
    /// Represents a task for posting <see cref="EPWFile"/> objects to a PostgreSQL database from EPW files selected through the user interface.
    /// </summary>
    public class UIEPWFileFromFilePostTask : EPWFilesPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIEPWFileFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to communicate with the GIS PostgreSQL Web API.</param>
        public UIEPWFileFromFilePostTask(GISWebAPIManager gISWebAPIManager)
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
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want select single EPW file?", "Selection", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return false;
            }

            List<string> paths_EPW = [];
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "Select EPW file",
                    Filter = string.Format(CultureInfo.CurrentCulture, "EPW File (*.{0})|*.{0}|All files (*.*)|*.*", EPW.Constants.FileExtension.EPWFile)
                };

                bool? dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                paths_EPW.Add(openFileDialog.FileName);
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

                string[] paths_Input = Directory.GetFiles(directory, "*." + EPW.Constants.FileExtension.EPWFile, SearchOption.AllDirectories);
                if (paths_Input == null || paths_Input.Length == 0)
                {
                    return false;
                }

                paths_EPW = [.. paths_Input];
            }

            if (paths_EPW is null || paths_EPW.Count == 0)
            {
                return true;
            }

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            List<EPWFile> ePWFiles = [];
            foreach (string path_EPW in paths_EPW)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(path_EPW) || !File.Exists(path_EPW))
                {
                    return false;
                }

                EPWFile? ePWFile = EPW.Modify.Read(path_EPW);
                if (ePWFile is not null)
                {
                    ePWFiles.Add(ePWFile);
                }
            }

            if (ePWFiles.Count == 0)
            {
                return true;
            }

            bool succeeded;
            try
            {
                succeeded = await ExecuteAsync(ePWFiles, longProgressWrapper, cancellationToken);
            }
            catch
            {
                throw;
            }

            return succeeded;
        }
    }
}
