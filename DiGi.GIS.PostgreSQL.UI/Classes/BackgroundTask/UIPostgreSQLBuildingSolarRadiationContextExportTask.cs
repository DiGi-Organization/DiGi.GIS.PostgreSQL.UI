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
    /// An urban-context export scoped from the user interface: the county parts, the radii and their column order, the residential filter, the batch size, the timeout and the output folder are asked for through <see cref="PostgreSQLBuildingSolarRadiationContextExportOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingSolarRadiationContextExportTask"/> (DiGi.GIS.PostgreSQL#136).
    /// <para>The export refuses a run without an output folder, and a server has no folder it could sensibly pick, so the dialog proposes <see cref="Query.SolarContextDirectory(string?)"/> and creates it when it is accepted.</para>
    /// </summary>
    public class UIPostgreSQLBuildingSolarRadiationContextExportTask : PostgreSQLBuildingSolarRadiationContextExportTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingSolarRadiationContextExportTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the counties, the stored solar radiation and the building data.</param>
        public UIPostgreSQLBuildingSolarRadiationContextExportTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the counties cannot be read and the run cannot be scoped", nameof(UIPostgreSQLBuildingSolarRadiationContextExportTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the export options cannot be asked for", nameof(UIPostgreSQLBuildingSolarRadiationContextExportTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationContextExportOptions ??= new();

            // Every county part rather than one per code (uniqueCode: false): a multi-part county would otherwise
            // lose the rest of its territory. Read on this thread, shown on the user interface thread.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: counties could not be retrieved - the export cannot be scoped", nameof(UIPostgreSQLBuildingSolarRadiationContextExportTask));
                return false;
            }

            PostgreSQLBuildingSolarRadiationContextExportOptions? postgreSQLBuildingSolarRadiationContextExportOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingSolarRadiationContextExportOptionsWindow postgreSQLBuildingSolarRadiationContextExportOptionsWindow = new(PostgreSQLBuildingSolarRadiationContextExportOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingSolarRadiationContextExportOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingSolarRadiationContextExportOptions = postgreSQLBuildingSolarRadiationContextExportOptionsWindow.PostgreSQLBuildingSolarRadiationContextExportOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLBuildingSolarRadiationContextExportOptions is null)
            {
                Serilog.Modify.Log("{Type}: export options were cancelled - nothing was written", nameof(UIPostgreSQLBuildingSolarRadiationContextExportTask));
                CancelRun();
                return false;
            }

            PostgreSQLBuildingSolarRadiationContextExportOptions = postgreSQLBuildingSolarRadiationContextExportOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
