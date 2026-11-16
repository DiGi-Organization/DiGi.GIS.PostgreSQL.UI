using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Solar.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A batch solar radiation solve scoped from the user interface: the county parts, the neighbour radii and the batch, tile, ceiling and timeout settings are asked for through <see cref="PostgreSQLBuildingSolarRadiationSolveOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingSolarRadiationSolveTask"/> (DiGi.GIS.PostgreSQL#136).
    /// <para>A solve costs seconds per building and radius, so unscoped it is the whole country once per radius - not a default anyone starts on purpose from a tray menu.</para>
    /// </summary>
    public class UIPostgreSQLBuildingSolarRadiationSolveTask : PostgreSQLBuildingSolarRadiationSolveTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingSolarRadiationSolveTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the counties, buildings, models and weather and to write the results.</param>
        public UIPostgreSQLBuildingSolarRadiationSolveTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the counties cannot be read and the run cannot be scoped", nameof(UIPostgreSQLBuildingSolarRadiationSolveTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the solve options cannot be asked for", nameof(UIPostgreSQLBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions ??= new();

            // Every county part rather than one per code (uniqueCode: false): a multi-part county would otherwise
            // lose the rest of its territory. Read on this thread, shown on the user interface thread.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: counties could not be retrieved - the solve cannot be scoped", nameof(UIPostgreSQLBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions? postgreSQLBuildingSolarRadiationSolveOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingSolarRadiationSolveOptionsWindow postgreSQLBuildingSolarRadiationSolveOptionsWindow = new(PostgreSQLBuildingSolarRadiationSolveOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingSolarRadiationSolveOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingSolarRadiationSolveOptions = postgreSQLBuildingSolarRadiationSolveOptionsWindow.PostgreSQLBuildingSolarRadiationSolveOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLBuildingSolarRadiationSolveOptions is null)
            {
                Serilog.Modify.Log("{Type}: solve options were cancelled - nothing was solved", nameof(UIPostgreSQLBuildingSolarRadiationSolveTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationSolveOptions = postgreSQLBuildingSolarRadiationSolveOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
