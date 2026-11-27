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
    /// A solar radiation summary run scoped from the user interface: the county parts, the radius the summary is built from, whether completed rows are stepped over, the batch size and the timeout are asked for through <see cref="PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingDataSolarRadiationUpdateTask"/>.
    /// <para>The radius matters once a building carries several variants (DiGi.GIS.PostgreSQL#107): without one the variant written last wins, building by building, so a summary meant to describe one variant has to name it.</para>
    /// </summary>
    public class UIPostgreSQLBuildingDataSolarRadiationUpdateTask : PostgreSQLBuildingDataSolarRadiationUpdateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingDataSolarRadiationUpdateTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the counties and the stored solar radiation and to write the building data.</param>
        public UIPostgreSQLBuildingDataSolarRadiationUpdateTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the counties cannot be read and the run cannot be scoped", nameof(UIPostgreSQLBuildingDataSolarRadiationUpdateTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the solar summary options cannot be asked for", nameof(UIPostgreSQLBuildingDataSolarRadiationUpdateTask));
                return false;
            }

            PostgreSQLBuildingDataSolarRadiationUpdateOptions ??= new();

            // Every county part rather than one per code (uniqueCode: false): a multi-part county would otherwise
            // lose the rest of its territory. Read on this thread, shown on the user interface thread.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingDataSolarRadiationUpdateOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: counties could not be retrieved - the solar summary cannot be scoped", nameof(UIPostgreSQLBuildingDataSolarRadiationUpdateTask));
                return false;
            }

            PostgreSQLBuildingDataSolarRadiationUpdateOptions? postgreSQLBuildingDataSolarRadiationUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow postgreSQLBuildingDataSolarRadiationUpdateOptionsWindow = new(PostgreSQLBuildingDataSolarRadiationUpdateOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingDataSolarRadiationUpdateOptions = postgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.PostgreSQLBuildingDataSolarRadiationUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy.
            if (postgreSQLBuildingDataSolarRadiationUpdateOptions is null)
            {
                Serilog.Modify.Log("{Type}: solar summary options were cancelled - nothing was written", nameof(UIPostgreSQLBuildingDataSolarRadiationUpdateTask));
                CancelRun();
                return false;
            }

            PostgreSQLBuildingDataSolarRadiationUpdateOptions = postgreSQLBuildingDataSolarRadiationUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
