using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// An acquisition-date backfill of <c>orto_datas</c> that is scoped from the user interface: the county parts, the last fetched year and the paging are asked for through <see cref="PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLOrtoDatasAcquisitionUpdateTask"/>.
    /// <para>The county parts are always named: the backfill rewrites every stored series it reaches, so a national pass is a deliberate selection of every part, not a default. Run the sheet import (<see cref="UIPostgreSQLOrtoSheetUpdateTask"/>) first.</para>
    /// </summary>
    public class UIPostgreSQLOrtoDatasAcquisitionUpdateTask : PostgreSQLOrtoDatasAcquisitionUpdateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLOrtoDatasAcquisitionUpdateTask"/> class.
        /// </summary>
        /// <param name="GISPostgreSQLConverterManager">The GIS PostgreSQL converter manager the tables and the county parts are read through.</param>
        public UIPostgreSQLOrtoDatasAcquisitionUpdateTask(GISPostgreSQLConverterManager? GISPostgreSQLConverterManager)
            : base(GISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the acquisition backfill options cannot be asked for");
                return false;
            }

            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return false;
            }

            // Every county row - all 406 polygon parts of the 380 codes, which is what orto_datas is partitioned by.
            // uniqueCode stays false, or a multi-part county would collapse to one part and lose the rest of its territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Counties could not be retrieved - the acquisition backfill cannot be scoped");
                return false;
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLOrtoDatasAcquisitionUpdateOptions? postgreSQLOrtoDatasAcquisitionUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLOrtoDatasAcquisitionUpdateOptionsWindow postgreSQLOrtoDatasAcquisitionUpdateOptionsWindow = new(PostgreSQLOrtoDatasAcquisitionUpdateOptions, administrativeAreal2DReferences);

                if (postgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLOrtoDatasAcquisitionUpdateOptions = postgreSQLOrtoDatasAcquisitionUpdateOptionsWindow.PostgreSQLOrtoDatasAcquisitionUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLOrtoDatasAcquisitionUpdateOptions is null)
            {
                Serilog.Modify.Log("Acquisition backfill options were cancelled - nothing was written");
                return false;
            }

            PostgreSQLOrtoDatasAcquisitionUpdateOptions = postgreSQLOrtoDatasAcquisitionUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
