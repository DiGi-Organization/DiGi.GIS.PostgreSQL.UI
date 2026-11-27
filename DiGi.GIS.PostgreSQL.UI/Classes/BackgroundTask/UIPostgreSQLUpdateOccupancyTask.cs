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
    /// An occupancy update that is scoped from the user interface: which side runs, whether the stored rows are cleared first and which counties the building side is limited to are asked for through <see cref="PostgreSQLUpdateOccupancyOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLUpdateOccupancyTask"/>.
    /// <para>The counties are what make a re-run of one county affordable after its subdivision identifiers were re-derived (DiGi.GIS.PostgreSQL#77): unscoped, the building side reads and rewrites every building in the country.</para>
    /// </summary>
    public class UIPostgreSQLUpdateOccupancyTask : PostgreSQLUpdateOccupancyTask, IGISPostgreSQLUIObject
    {
        private readonly GISPostgreSQLConverterManager gISPostgreSQLConverterManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLUpdateOccupancyTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the areas and buildings and write the occupancy.</param>
        public UIPostgreSQLUpdateOccupancyTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
            this.gISPostgreSQLConverterManager = gISPostgreSQLConverterManager;
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the occupancy options cannot be asked for");
                return false;
            }

            // Every county row - all 406 polygon parts of the 380 codes, which is what the building side is keyed
            // by. uniqueCode stays false, or a multi-part county would collapse to one part and lose the rest of
            // its territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = null;
            if (gISPostgreSQLConverterManager?.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>() is AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
            {
                administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, cancellationToken: cancellationToken);
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLUpdateOccupancyOptions? postgreSQLUpdateOccupancyOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLUpdateOccupancyOptionsWindow postgreSQLUpdateOccupancyOptionsWindow = new(PostgreSQLUpdateOccupancyOptions, administrativeAreal2DReferences);

                if (postgreSQLUpdateOccupancyOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLUpdateOccupancyOptions = postgreSQLUpdateOccupancyOptionsWindow.PostgreSQLUpdateOccupancyOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy -
            // and ends the run here rather than starting a national pass nobody asked for.
            if (postgreSQLUpdateOccupancyOptions is null)
            {
                Serilog.Modify.Log("Occupancy options were cancelled - nothing was written");
                CancelRun();
                return false;
            }

            PostgreSQLUpdateOccupancyOptions = postgreSQLUpdateOccupancyOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
