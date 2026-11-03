using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.GIS.WebAPI.Classes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// An orthophoto sheet index import that is scoped from the user interface: the index years and the pacing are asked for through <see cref="PostgreSQLOrtoSheetUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLOrtoSheetUpdateTask"/>.
    /// <para>A single year is a trial of a few seconds; every year is the 23-minute national import that the acquisition backfill reads (DiGi.GIS.PostgreSQL.UI#37).</para>
    /// </summary>
    public class UIPostgreSQLOrtoSheetUpdateTask : PostgreSQLOrtoSheetUpdateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLOrtoSheetUpdateTask"/> class.
        /// </summary>
        /// <param name="GISWebAPIManager">The manager the GUGiK service client is built from.</param>
        /// <param name="GISPostgreSQLConverterManager">The GIS PostgreSQL converter manager the sheet table is written through.</param>
        public UIPostgreSQLOrtoSheetUpdateTask(GISWebAPIManager? GISWebAPIManager, GISPostgreSQLConverterManager? GISPostgreSQLConverterManager)
            : base(WebAPI.Create.HttpClient_GUGiK(GISWebAPIManager), GISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (httpClient is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Index service client could not be created - the sheet import cannot start");
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the sheet import options cannot be asked for");
                return false;
            }

            PostgreSQLOrtoSheetUpdateOptions? postgreSQLOrtoSheetUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLOrtoSheetUpdateOptionsWindow postgreSQLOrtoSheetUpdateOptionsWindow = new(PostgreSQLOrtoSheetUpdateOptions);

                if (postgreSQLOrtoSheetUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLOrtoSheetUpdateOptions = postgreSQLOrtoSheetUpdateOptionsWindow.PostgreSQLOrtoSheetUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLOrtoSheetUpdateOptions is null)
            {
                Serilog.Modify.Log("Sheet import options were cancelled - nothing was imported");
                return false;
            }

            PostgreSQLOrtoSheetUpdateOptions = postgreSQLOrtoSheetUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
