using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Energy.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A batch heat demand run scoped from the user interface: the county parts, the construction flavour, the solar radius, whether stored results are stepped over and the batch, ceiling, parallelism and timeout settings are asked for through <see cref="PostgreSQLBuildingHeatDemandSolveOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingHeatDemandSolveTask"/> (DiGi.GIS.PostgreSQL.UI#49, for DiGi-Organization/DiGi.Analytical#10).
    /// <para>A building costs a placement of its openings and a monthly calculation, so unscoped the run is every building in the country - not a default anyone starts on purpose from a tray menu.</para>
    /// </summary>
    public class UIPostgreSQLBuildingHeatDemandSolveTask : PostgreSQLBuildingHeatDemandSolveTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingHeatDemandSolveTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the counties, buildings, models, solar results and weather and to write the results.</param>
        public UIPostgreSQLBuildingHeatDemandSolveTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the counties cannot be read and the run cannot be scoped", nameof(UIPostgreSQLBuildingHeatDemandSolveTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the heat demand options cannot be asked for", nameof(UIPostgreSQLBuildingHeatDemandSolveTask));
                return false;
            }

            PostgreSQLBuildingHeatDemandSolveOptions ??= new();

            // Every county part rather than one per code (uniqueCode: false): a multi-part county would otherwise
            // lose the rest of its territory. Read on this thread, shown on the user interface thread.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingHeatDemandSolveOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: counties could not be retrieved - the run cannot be scoped", nameof(UIPostgreSQLBuildingHeatDemandSolveTask));
                return false;
            }

            PostgreSQLBuildingHeatDemandSolveOptions? postgreSQLBuildingHeatDemandSolveOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingHeatDemandSolveOptionsWindow postgreSQLBuildingHeatDemandSolveOptionsWindow = new(PostgreSQLBuildingHeatDemandSolveOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingHeatDemandSolveOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingHeatDemandSolveOptions = postgreSQLBuildingHeatDemandSolveOptionsWindow.PostgreSQLBuildingHeatDemandSolveOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLBuildingHeatDemandSolveOptions is null)
            {
                Serilog.Modify.Log("{Type}: heat demand options were cancelled - nothing was calculated", nameof(UIPostgreSQLBuildingHeatDemandSolveTask));
                return false;
            }

            PostgreSQLBuildingHeatDemandSolveOptions = postgreSQLBuildingHeatDemandSolveOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
