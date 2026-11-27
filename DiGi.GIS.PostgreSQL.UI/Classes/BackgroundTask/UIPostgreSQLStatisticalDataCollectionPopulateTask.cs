using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Represents a task for populating statistical data collections into PostgreSQL from .sdcf files in a directory selected through the user interface.
    /// </summary>
    public class UIPostgreSQLStatisticalDataCollectionPopulateTask : PostgreSQLStatisticalDataCollectionPopulateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLStatisticalDataCollectionPopulateTask"/> class with a converter.
        /// </summary>
        /// <param name="statisticalDataCollectionPostgreSQLConverter">The statistical data collection PostgreSQL converter used to populate the table.</param>
        public UIPostgreSQLStatisticalDataCollectionPopulateTask(StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter)
            : base(statisticalDataCollectionPostgreSQLConverter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLStatisticalDataCollectionPopulateTask"/> class from a manager.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager containing the statistical data collection converter.</param>
        public UIPostgreSQLStatisticalDataCollectionPopulateTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(PostgreSQLStatisticalDataCollectionPopulateOptions?.Path) || (!Directory.Exists(PostgreSQLStatisticalDataCollectionPopulateOptions.Path) && !File.Exists(PostgreSQLStatisticalDataCollectionPopulateOptions.Path)))
            {
                string? selectedPath = null;

                if (System.Windows.Application.Current is System.Windows.Application application)
                {
                    application.Dispatcher.Invoke(() =>
                    {
                        OpenFolderDialog openFolderDialog = new()
                        {
                            Title = "Select directory containing *.sdcf files"
                        };

                        if (openFolderDialog.ShowDialog() is true)
                        {
                            selectedPath = openFolderDialog.FolderName;
                        }
                    });
                }
                else
                {
                    OpenFolderDialog openFolderDialog = new()
                    {
                        Title = "Select directory containing *.sdcf files"
                    };

                    if (openFolderDialog.ShowDialog() is true)
                    {
                        selectedPath = openFolderDialog.FolderName;
                    }
                }

                // No path means the file dialog was cancelled: the run ends as canceled, not as a failure
                // (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#61).
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    CancelRun();
                    return false;
                }

                PostgreSQLStatisticalDataCollectionPopulateOptions ??= new();
                PostgreSQLStatisticalDataCollectionPopulateOptions.Path = selectedPath;
            }

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
