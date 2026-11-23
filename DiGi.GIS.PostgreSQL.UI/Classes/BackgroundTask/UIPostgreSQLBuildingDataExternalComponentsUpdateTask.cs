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
    /// An external components area run that is scoped from the user interface: the counties, the statement timeout, the batch size, the skipping of finished buildings, the party walls and the number of parallel batches are asked for through <see cref="PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingDataExternalComponentsUpdateTask"/>.
    /// <para>That is what the counties are worth asking for. The run reads every stored building model of the counties in scope and classifies their components into the wall, roof and floor area columns, so unscoped it walks all 406 county parts and their models, while over one county it is minutes. A county whose buildings carry no stored model is processed, not failed, so the scoping decides what is touched and how long it takes, not whether the run succeeds.</para>
    /// </summary>
    public class UIPostgreSQLBuildingDataExternalComponentsUpdateTask : PostgreSQLBuildingDataExternalComponentsUpdateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingDataExternalComponentsUpdateTask"/> class.
        /// </summary>
        /// <param name="GISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the stored building models and write the external components area columns.</param>
        public UIPostgreSQLBuildingDataExternalComponentsUpdateTask(GISPostgreSQLConverterManager GISPostgreSQLConverterManager)
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
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the external components area options cannot be asked for");
                return false;
            }

            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No {Converter} - the counties cannot be read and the run cannot be scoped", nameof(AdministrativeAreal2DPostgreSQLConverter));
                return false;
            }

            // References rather than the identifiers GetIdsAsync returns: that method does list every county row
            // - all 406 polygon parts of the 380 codes, which is what the run is keyed by - but it carries
            // neither code nor name, and a list of 406 bare integers is not one a county can be picked from.
            // uniqueCode stays false, or a multi-part county would collapse to one part and lose the rest of its
            // territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingDataExternalComponentsUpdateOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Counties could not be retrieved - the external components area run cannot be scoped");
                return false;
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLBuildingDataExternalComponentsUpdateOptions? postgreSQLBuildingDataExternalComponentsUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow postgreSQLBuildingDataExternalComponentsUpdateOptionsWindow = new(PostgreSQLBuildingDataExternalComponentsUpdateOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingDataExternalComponentsUpdateOptions = postgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.PostgreSQLBuildingDataExternalComponentsUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy -
            // and ends the run here rather than starting a national pass nobody asked for.
            if (postgreSQLBuildingDataExternalComponentsUpdateOptions is null)
            {
                Serilog.Modify.Log("External components area options were cancelled - nothing was written");
                return false;
            }

            PostgreSQLBuildingDataExternalComponentsUpdateOptions = postgreSQLBuildingDataExternalComponentsUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
