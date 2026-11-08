using DiGi.GIS.Constants;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI;
using DiGi.GIS.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Represents a task that handles the process of updating GIS data from a file via user interface interactions.
    /// </summary>
    public class UIUpdateFromFilePostTask : Building2DsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIUpdateFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to handle PostgreSQL Web API communications.</param>
        public UIUpdateFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for reporting progress of the operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            bool? result;

            OpenFileDialog openFileDialog = new()
            {
                Filter = "zip files (*.zip)|*.zip|All files (*.*)|*.*"
            };
            result = openFileDialog.ShowDialog();
            if (result == null || !result.HasValue || !result.Value)
            {
                return false;
            }

            string path = openFileDialog.FileName;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return false;
            }

            List<string>? sufixes = null;

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                DiGi.UI.WPF.Windows.ListBoxWindow listBoxWindow = new("File types")
                {
                    SelectionMode = System.Windows.Controls.SelectionMode.Multiple
                };

                listBoxWindow.SetItems([FileNameSufix.OT_ADJA_A, FileNameSufix.OT_ADMS_A, FileNameSufix.OT_BUBD_A]);

                bool? dialogResult = listBoxWindow.ShowDialog();
                if (dialogResult is null || !dialogResult.Value)
                {
                    return;
                }

                sufixes = listBoxWindow.GetItems<string>();
            });

            if (sufixes is null || sufixes.Count == 0)
            {
                return false;
            }

            SerializableObjectsPostOptions serializableObjectsPostOptions = new()
            {
                Delay = TimeSpan.FromSeconds(40),
                RequestResult = false
            };

            return await gISWebAPIManager.UpdateItemsAsync(path, sufixes.Contains(FileNameSufix.OT_ADJA_A), sufixes.Contains(FileNameSufix.OT_ADMS_A), sufixes.Contains(FileNameSufix.OT_BUBD_A), serializableObjectsPostOptions, progress, cancellationToken);
        }
    }
}
