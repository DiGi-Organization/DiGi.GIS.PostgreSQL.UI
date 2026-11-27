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
    /// A building data run that is scoped from the user interface: the counties, the kinds of column to write and the statement timeout are asked for through <see cref="PostgreSQLBuildingDataUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLBuildingDataUpdateTask"/>.
    /// <para>That is what the counties are worth asking for. Unscoped the run walks every subdivision in the country - around a hundred thousand of them - reading each subdivision's buildings and writing a row per building; over one named county it is minutes. Neither is a default the other would tolerate, and the base task takes an unset county collection as the whole country.</para>
    /// </summary>
    public class UIPostgreSQLBuildingDataUpdateTask : PostgreSQLBuildingDataUpdateTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingDataUpdateTask"/> class.
        /// </summary>
        /// <param name="GISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the areas and buildings and write the building data.</param>
        public UIPostgreSQLBuildingDataUpdateTask(GISPostgreSQLConverterManager GISPostgreSQLConverterManager)
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
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the building data options cannot be asked for");
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
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: PostgreSQLBuildingDataUpdateOptions.CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Counties could not be retrieved - the building data run cannot be scoped");
                return false;
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLBuildingDataUpdateOptions? postgreSQLBuildingDataUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLBuildingDataUpdateOptionsWindow postgreSQLBuildingDataUpdateOptionsWindow = new(PostgreSQLBuildingDataUpdateOptions, administrativeAreal2DReferences);

                if (postgreSQLBuildingDataUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLBuildingDataUpdateOptions = postgreSQLBuildingDataUpdateOptionsWindow.PostgreSQLBuildingDataUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy -
            // and ends the run here rather than starting a national pass nobody asked for.
            if (postgreSQLBuildingDataUpdateOptions is null)
            {
                Serilog.Modify.Log("Building data options were cancelled - nothing was written");
                CancelRun();
                return false;
            }

            PostgreSQLBuildingDataUpdateOptions = postgreSQLBuildingDataUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
