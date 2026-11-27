using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Represents a task for populating territorial units into a PostgreSQL database from a JSON file selected through the user interface.
    /// </summary>
    public class UIPostgreSQLUnitInsertFromFileTask : PostgreSQLUnitInsertFromFileTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLUnitInsertFromFileTask"/> class with a unit PostgreSQL converter.
        /// </summary>
        /// <param name="unitPostgreSQLConverter">The unit PostgreSQL converter used to populate the table.</param>
        public UIPostgreSQLUnitInsertFromFileTask(UnitPostgreSQLConverter unitPostgreSQLConverter)
            : base(unitPostgreSQLConverter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLUnitInsertFromFileTask"/> class from a manager.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The GIS PostgreSQL converter manager containing the unit converter.</param>
        public UIPostgreSQLUnitInsertFromFileTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(PostgreSQLUnitInsertFromFileOptions?.Path) || !File.Exists(PostgreSQLUnitInsertFromFileOptions.Path))
            {
                string? selectedPath = null;

                if (System.Windows.Application.Current is System.Windows.Application application)
                {
                    application.Dispatcher.Invoke(() =>
                    {
                        OpenFileDialog openFileDialog = new()
                        {
                            Title = "Select Unit JSON file",
                            Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
                        };

                        if (openFileDialog.ShowDialog() is true)
                        {
                            selectedPath = openFileDialog.FileName;
                        }
                    });
                }
                else
                {
                    OpenFileDialog openFileDialog = new()
                    {
                        Title = "Select Unit JSON file",
                        Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
                    };

                    if (openFileDialog.ShowDialog() is true)
                    {
                        selectedPath = openFileDialog.FileName;
                    }
                }

                // No path means the file dialog was cancelled: the run ends as canceled, not as a failure
                // (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#61).
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    CancelRun();
                    return false;
                }

                PostgreSQLUnitInsertFromFileOptions ??= new();
                PostgreSQLUnitInsertFromFileOptions.Path = selectedPath;
            }

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
