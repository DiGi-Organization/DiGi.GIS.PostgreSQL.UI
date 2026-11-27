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
    /// Represents a task for creating the statistical data collection table in PostgreSQL and populating it from .sdcf files in a directory selected through the user interface.
    /// <para>A cancelled folder dialog ends the run as canceled (<see cref="Core.Classes.CancelableBackgroundTask"/>), with nothing created; a directory that does not exist is a refusal, logged with its path, and ends the run as failed (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#77). <see cref="SkipDirectoryDialog"/> is the seam a test uses instead of the dialog.</para>
    /// </summary>
    public class UIPostgreSQLStatisticalDataCollectionCreateTableTask : PostgreSQLStatisticalDataCollectionCreateTableTask, IGISPostgreSQLUIObject
    {
        private readonly StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLStatisticalDataCollectionCreateTableTask"/> class with a statistical data collection PostgreSQL converter.
        /// </summary>
        /// <param name="statisticalDataCollectionPostgreSQLConverter">The statistical data collection PostgreSQL converter used to create and populate the table.</param>
        public UIPostgreSQLStatisticalDataCollectionCreateTableTask(StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter)
            : base(statisticalDataCollectionPostgreSQLConverter)
        {
            this.statisticalDataCollectionPostgreSQLConverter = statisticalDataCollectionPostgreSQLConverter ?? throw new ArgumentNullException(nameof(statisticalDataCollectionPostgreSQLConverter));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLStatisticalDataCollectionCreateTableTask"/> class from a manager.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager containing the statistical data collection converter.</param>
        public UIPostgreSQLStatisticalDataCollectionCreateTableTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
            ArgumentNullException.ThrowIfNull(gISPostgreSQLConverterManager);

            statisticalDataCollectionPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<StatisticalDataCollectionPostgreSQLConverter>() ?? throw new InvalidOperationException($"{nameof(StatisticalDataCollectionPostgreSQLConverter)} not registered in converter manager.");
        }

        /// <summary>
        /// Gets or sets whether the folder dialog is skipped and the run takes its directory from <see cref="SourceDirectory"/> instead.
        /// <para>The dialog needs a running WPF application, which a test host has not got; a fact sets this and drives the run through the property (the seam <see cref="UIOpenBuildingImportTask.SkipOptionsDialog"/> carries for the same reason). A null or empty <see cref="SourceDirectory"/> then stands for a cancelled dialog. Off for every row the tray registers.</para>
        /// </summary>
        public bool SkipDirectoryDialog { get; set; }

        /// <summary>
        /// Gets or sets the directory of .sdcf files the run uses when <see cref="SkipDirectoryDialog"/> is set; ignored otherwise.
        /// </summary>
        public string? SourceDirectory { get; set; }

        /// <summary>
        /// Asks for the directory of .sdcf files, creates the statistical data collection table and populates it from that directory.
        /// </summary>
        /// <param name="token">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation. Returns true if the table was created and populated; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(CancellationToken token)
        {
            string? directory = SourceDirectory;

            if (!SkipDirectoryDialog)
            {
                if (System.Windows.Application.Current is not System.Windows.Application application)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the statistical data collection options cannot be asked for");
                    return false;
                }

                directory = null;

                application.Dispatcher.Invoke(() =>
                {
                    OpenFolderDialog openFolderDialog = new()
                    {
                        Title = "Select directory containing *.sdcf files"
                    };

                    if (openFolderDialog.ShowDialog() is true)
                    {
                        directory = openFolderDialog.FolderName;
                    }
                });
            }

            // No directory means the dialog was cancelled: the run ends as canceled, not as a failure the tray would
            // log as an error (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#77).
            if (string.IsNullOrWhiteSpace(directory))
            {
                Serilog.Modify.Log("{Type}: no directory was selected - nothing was created", nameof(UIPostgreSQLStatisticalDataCollectionCreateTableTask));
                CancelRun();
                return false;
            }

            if (!Directory.Exists(directory))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the directory {Directory} does not exist - nothing was created", nameof(UIPostgreSQLStatisticalDataCollectionCreateTableTask), directory);
                return false;
            }

            bool tableCreated = await base.ExecuteAsync(token);
            if (!tableCreated)
            {
                return false;
            }

            return await statisticalDataCollectionPostgreSQLConverter.PopulateAsync(directory, clear: false, commandTimeout: 600, cancellationToken: token);
        }
    }
}
