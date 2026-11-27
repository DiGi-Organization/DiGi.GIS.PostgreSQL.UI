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
    /// A Building2D refresh that is scoped from the user interface: the batch size, whether existing subdivision identifiers are re-derived, whether the run is limited to counties whose subdivision layer nests, and which counties are walked are asked for through <see cref="PostgreSQLBuilding2DRefreshOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuilding2DRefreshTask"/>.
    /// <para>That is what the scope is worth asking for. Overriding and unscoped, the refresh re-derives the subdivision of every building in the country - a polygon lookup and an intersection per building. The counties that need it after DiGi.GIS.PostgreSQL#77 are the ones whose subdivision layer nests, which the task can find on its own - but that is nearly every county, a village and its named parts nesting as a city and its districts do, so the county list is what makes a trial run small.</para>
    /// </summary>
    public class UIPostgreSQLBuilding2DRefreshTask : PostgreSQLBuilding2DRefreshTask, IGISPostgreSQLUIObject
    {
        private readonly AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuilding2DRefreshTask"/> class.
        /// </summary>
        /// <param name="building2DPostgreSQLConverter">The Building2D PostgreSQL converter used to refresh the data.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The administrative areal converter the counties are read from, so the run can be scoped. When null the dialog offers no counties and the run is nationwide.</param>
        public UIPostgreSQLBuilding2DRefreshTask(Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter)
            : base(building2DPostgreSQLConverter)
        {
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the Building2D refresh options cannot be asked for");
                return false;
            }

            // Every county row - all 406 polygon parts of the 380 codes, which is what the refresh is keyed by.
            // uniqueCode stays false, or a multi-part county would collapse to one part and lose the rest of its
            // territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = null;
            if (administrativeAreal2DPostgreSQLConverter is not null)
            {
                administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, cancellationToken: cancellationToken);
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLBuilding2DRefreshOptions? postgreSQLBuilding2DRefreshOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuilding2DRefreshOptionsWindow postgreSQLBuilding2DRefreshOptionsWindow = new(PostgreSQLBuilding2DRefreshOptions, administrativeAreal2DReferences);

                if (postgreSQLBuilding2DRefreshOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuilding2DRefreshOptions = postgreSQLBuilding2DRefreshOptionsWindow.PostgreSQLBuilding2DRefreshOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy -
            // and ends the run here rather than starting a national pass nobody asked for.
            if (postgreSQLBuilding2DRefreshOptions is null)
            {
                Serilog.Modify.Log("Building2D refresh options were cancelled - nothing was written");
                CancelRun();
                return false;
            }

            PostgreSQLBuilding2DRefreshOptions = postgreSQLBuilding2DRefreshOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
