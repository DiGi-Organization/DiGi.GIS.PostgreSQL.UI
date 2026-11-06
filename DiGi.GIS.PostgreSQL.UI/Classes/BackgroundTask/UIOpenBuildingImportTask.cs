using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.Geometry.Planar.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// The tray-side wrapper of <see cref="PostgreSQLOpenBuildingImportTask"/> (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38): it asks the operator for the run before it starts one, computes the release's SHA-256 from the source files instead of having it typed, and writes the run report.
    /// <para>The window is shown on the dispatcher, because this body runs on a thread pool thread where a window cannot be created at all. A cancelled dialog ends the run before anything is written, and <see cref="SkipOptionsDialog"/> is the seam a test or a hosted run uses to drive the task through its properties instead.</para>
    /// <para>What the dialog asked is checked once more through <see cref="Query.OpenBuildingImportOptionsMessage(PostgreSQLOpenBuildingImportTaskOptions)"/> before the core task is started, so a hand-built task that skipped the dialog still refuses with a reason in the log rather than failing deep inside the import.</para>
    /// </summary>
    public class UIOpenBuildingImportTask : PostgreSQLOpenBuildingImportTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// The report file name prefix; the dataset key follows it, so the six datasets never overwrite each other's report.
        /// </summary>
        private const string ReportPrefix = "OpenBuilding_Import_Report_";

        /// <summary>
        /// Initializes a new instance of the <see cref="UIOpenBuildingImportTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The manager holding the PostgreSQL converters.</param>
        public UIOpenBuildingImportTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <summary>
        /// Gets or sets the dataset key this row imports. It forwards to <see cref="PostgreSQLOpenBuildingImportTaskOptions.DatasetKey"/> of the options, so each of the six rows fixes its own dataset and a row named for one release cannot import another.
        /// </summary>
        public string? DatasetKey
        {
            get
            {
                return PostgreSQLOpenBuildingImportTaskOptions?.DatasetKey;
            }

            set
            {
                PostgreSQLOpenBuildingImportTaskOptions = PostgreSQLOpenBuildingImportTaskOptions ?? new PostgreSQLOpenBuildingImportTaskOptions();
                PostgreSQLOpenBuildingImportTaskOptions.DatasetKey = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the options dialog is skipped and the run takes its options from the task's properties as they are.
        /// <para>The dialog needs a running WPF application, which a test host has not got; a fact sets this and drives the run through the properties instead (the seam <see cref="UIPostgreSQLBuildingModelBuildingInformationUpdateTask"/> carries for the same reason). Off for every row the tray registers.</para>
        /// </summary>
        public bool SkipOptionsDialog { get; set; } = false;

        /// <summary>
        /// Asks for the run's options, validates them, computes the release's SHA-256 from the source files, runs the core import and writes the run report.
        /// </summary>
        /// <param name="progress">A progress reporter carrying the running total of rows written.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. Returns true unless the run was cancelled or refused.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            DateTime started = DateTime.Now;

            if (!SkipOptionsDialog)
            {
                if (System.Windows.Application.Current is not System.Windows.Application application)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the open building import options cannot be asked for");
                    return false;
                }

                AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
                if (administrativeAreal2DPostgreSQLConverter is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the county parts cannot be listed to scope the run", nameof(UIOpenBuildingImportTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                    return false;
                }

                // Read on this thread, shown on the user interface thread.
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(AdministrativeArealType.County, cancellationToken: cancellationToken);
                if (administrativeAreal2DReferences is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the county parts could not be read - the scope cannot be asked for", nameof(UIOpenBuildingImportTask));
                    return false;
                }

                PostgreSQLOpenBuildingImportTaskOptions? postgreSQLOpenBuildingImportTaskOptions = null;

                application.Dispatcher.Invoke(() =>
                {
                    OpenBuildingImportOptionsWindow openBuildingImportOptionsWindow = new(PostgreSQLOpenBuildingImportTaskOptions, administrativeAreal2DReferences);

                    if (openBuildingImportOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                    {
                        return;
                    }

                    postgreSQLOpenBuildingImportTaskOptions = openBuildingImportOptionsWindow.PostgreSQLOpenBuildingImportTaskOptions;
                });

                // A cancelled dialog ends the run here rather than starting an import nobody scoped.
                if (postgreSQLOpenBuildingImportTaskOptions is null)
                {
                    Serilog.Modify.Log("Open building import options were cancelled - nothing was written");
                    return false;
                }

                PostgreSQLOpenBuildingImportTaskOptions = postgreSQLOpenBuildingImportTaskOptions;
            }

            if (Query.OpenBuildingImportOptionsMessage(PostgreSQLOpenBuildingImportTaskOptions) is string message)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {Message} - nothing was imported", nameof(UIOpenBuildingImportTask), message);
                return false;
            }

            if (!await Sha256Async(cancellationToken))
            {
                return false;
            }

            bool succeeded;
            try
            {
                succeeded = await base.ExecuteAsync(progress, cancellationToken);
            }
            catch (Exception exception)
            {
                // The report goes out for a run that faulted or was cancelled too: the tallies it reached and
                // the reason belong on disk while they are known, before the base class stores the exception.
                WriteReport(started, false, exception, cancellationToken);
                throw;
            }

            WriteReport(started, succeeded, null, cancellationToken);

            return succeeded;
        }

        /// <summary>
        /// Computes the release's SHA-256 from the source files and stores it on the options - never typed, always derived from what is actually imported.
        /// <para>One source file answers its bare lowercase hex digest, the form <c>BuildingDatasetRelease</c> documents. Several files - GHS-OBAT comes as CSV with GeoPackage centroids - answer one <c>digest␣␣file name</c> line per file in ordinal path order, the sha256sum format, so every line of the stored value can be verified against its file with a stock tool.</para>
        /// <para>A file that cannot be hashed refuses the run: a release stored without provenance is worse than no run, and the failure is logged with the path that caused it.</para>
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
        /// <returns>A task whose result is true when every source file was hashed and the value stored on the options.</returns>
        private async Task<bool> Sha256Async(CancellationToken cancellationToken)
        {
            PostgreSQLOpenBuildingImportTaskOptions options = PostgreSQLOpenBuildingImportTaskOptions;
            if (options.SourcePaths is not HashSet<string> sourcePaths || sourcePaths.Count == 0)
            {
                // Unreachable through the validation that runs first; refused rather than hashed over nothing.
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no source file to compute the SHA-256 from", nameof(UIOpenBuildingImportTask));
                return false;
            }

            List<string> paths = [.. sourcePaths];
            paths.Sort(StringComparer.Ordinal);

            if (paths.Count == 1)
            {
                if (await Query.FileSHA256Async(paths[0], cancellationToken) is not string sha256)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the SHA-256 of {Path} could not be computed - the run was not started", nameof(UIOpenBuildingImportTask), paths[0]);
                    return false;
                }

                options.Sha256 = sha256;

                Serilog.Modify.Log("SHA-256 of {Path}: {Sha256}", paths[0], sha256);

                return true;
            }

            List<string> lines = [];
            foreach (string path in paths)
            {
                if (await Query.FileSHA256Async(path, cancellationToken) is not string sha256)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the SHA-256 of {Path} could not be computed - the run was not started", nameof(UIOpenBuildingImportTask), path);
                    return false;
                }

                lines.Add($"{sha256}  {Path.GetFileName(path)}");
            }

            options.Sha256 = string.Join('\n', lines);

            Serilog.Modify.Log("SHA-256 computed for {Count} source files", paths.Count);

            return true;
        }

        /// <summary>
        /// Writes the run report next to this application: the release, the scope, the source files, the tallies and the register row id (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#38).
        /// <para>The report is written for a refused or cancelled run as well - a run that stored nothing is exactly when the reason belongs on disk - and a failure to write it is logged rather than thrown, so a read-only output directory cannot turn a finished import into a failed one.</para>
        /// </summary>
        /// <param name="started">When the run started, for the report's duration.</param>
        /// <param name="succeeded">What the core task answered for the run.</param>
        /// <param name="exception">The exception that ended the run, or null when none escaped the core task.</param>
        /// <param name="cancellationToken">The token the run observed, so the report can say whether it was cancelled.</param>
        private void WriteReport(DateTime started, bool succeeded, Exception? exception, CancellationToken cancellationToken)
        {
            try
            {
                PostgreSQLOpenBuildingImportTaskOptions? options = PostgreSQLOpenBuildingImportTaskOptions;
                if (options is null || options.DatasetKey is not string datasetKey || string.IsNullOrWhiteSpace(datasetKey))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the run report was not written - no dataset key to name it by", nameof(UIOpenBuildingImportTask));
                    return;
                }

                datasetKey = datasetKey.Trim();

                string scope;
                if (options.CountyIds is HashSet<int> countyIds && countyIds.Count != 0)
                {
                    List<int> countyIds_Sorted = [.. countyIds];
                    countyIds_Sorted.Sort();
                    scope = string.Create(CultureInfo.InvariantCulture, $"county parts ({countyIds_Sorted.Count}): {string.Join(' ', countyIds_Sorted)}");
                }
                else if (options.BoundingBox2D is BoundingBox2D boundingBox2D)
                {
                    scope = string.Create(CultureInfo.InvariantCulture, $"bounding box ({boundingBox2D.Min.X}, {boundingBox2D.Min.Y}) - ({boundingBox2D.Max.X}, {boundingBox2D.Max.Y})");
                }
                else
                {
                    scope = "(none)";
                }

                List<string> lines =
                [
                    "Open building dataset import",
                    $"Dataset: {datasetKey}",
                    $"Version: {options.Version ?? "(none)"}",
                    $"Download date: {options.DownloadDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "(none)"}",
                    $"Licence: {options.Licence ?? "(none)"}",
                    $"Source URL: {options.SourceUrl ?? "(none)"}",
                    $"Source observation date: {options.SourceObservationDate ?? "(none)"}",
                    $"File name: {options.FileName ?? "(none)"}"
                ];

                string? sha256 = options.Sha256;
                if (sha256 is not null && sha256.Contains('\n'))
                {
                    // A multi-file release stores one sha256sum line per file, so the value is shown the same
                    // way rather than collapsed into one unreadable report line.
                    lines.Add("SHA-256 (per source file, sha256sum format):");
                    foreach (string line in sha256.Split('\n'))
                    {
                        lines.Add($"  {line}");
                    }
                }
                else
                {
                    lines.Add($"SHA-256: {sha256 ?? "(not computed)"}");
                }

                lines.Add($"Scope: {scope}");
                lines.Add("Source files:");

                if (options.SourcePaths is HashSet<string> sourcePaths)
                {
                    List<string> paths = [.. sourcePaths];
                    paths.Sort(StringComparer.Ordinal);
                    foreach (string path in paths)
                    {
                        lines.Add($"  {path}");
                    }
                }

                lines.Add($"Started: {started.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}");
                lines.Add($"Finished: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}");
                lines.Add(string.Create(CultureInfo.InvariantCulture, $"Rows written: {RowCount}"));
                lines.Add(string.Create(CultureInfo.InvariantCulture, $"Out of scope (filtered): {FilteredCount}"));
                lines.Add(string.Create(CultureInfo.InvariantCulture, $"Rejected before the database: {RejectionCount}"));
                lines.Add(string.Create(CultureInfo.InvariantCulture, $"Refused by the readers: {ReaderRejectionCount}"));
                lines.Add($"Register row id: {(DatasetId is int datasetId ? System.Convert.ToString(datasetId, CultureInfo.InvariantCulture) : "(none)")}");
                lines.Add($"Cancelled: {cancellationToken.IsCancellationRequested}");
                lines.Add($"Succeeded: {succeeded}");
                lines.Add($"Exception: {exception?.ToString() ?? "(none)"}");

                string path_Report = Path.Combine(AppContext.BaseDirectory, $"{ReportPrefix}{datasetKey.ToLowerInvariant()}.txt");
                File.WriteAllLines(path_Report, lines, Encoding.UTF8);

                Serilog.Modify.Log("Open building import report written to {Path}", path_Report);
            }
            catch (Exception exception_Temp)
            {
                Serilog.Modify.Log(exception_Temp, "{Type}: the run report could not be written", nameof(UIOpenBuildingImportTask));
            }
        }
    }
}
