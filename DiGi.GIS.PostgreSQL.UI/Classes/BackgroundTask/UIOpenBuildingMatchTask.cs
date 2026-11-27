using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
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
    /// The tray-side wrapper of <see cref="PostgreSQLOpenBuildingMatchTask"/> (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57): it asks the operator which datasets to match to BDOT10k, over which scope, with which rule parameters and whether to store the links, then runs the core task and writes the run report.
    /// <para>The converters the run needs - the open building converter on Storage, BDOT10k and the county parts on Main - are checked before the dialog, so a tray without them refuses with a reason instead of opening a window for a run that cannot start. The county list is read on this thread; the window is shown on the dispatcher, because this body runs on a thread pool thread where a window cannot be created. The window edits a copy: a cancelled dialog leaves the task's options as they were and ends the run before anything is read or written. <see cref="SkipOptionsDialog"/> is the seam a test or a hosted run uses instead.</para>
    /// <para>What the dialog asked is checked once more through <see cref="Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions?)"/> before the core task starts, so a hand-built task that skipped the dialog still refuses with a reason in the log.</para>
    /// </summary>
    public class UIOpenBuildingMatchTask : PostgreSQLOpenBuildingMatchTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// The report file name; one file, rewritten by every run, beside the application.
        /// </summary>
        private const string ReportFileName = "OpenBuilding_Match_Report.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="UIOpenBuildingMatchTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The manager holding the PostgreSQL converters.</param>
        public UIOpenBuildingMatchTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
            : base(gISPostgreSQLConverterManager)
        {
        }

        /// <summary>
        /// Gets or sets whether the options dialog is skipped and the run takes its options from the task's properties as they are.
        /// <para>The dialog needs a running WPF application, which a test host has not got; a fact sets this and drives the run through the properties instead (the seam <see cref="UIOpenBuildingImportTask"/> carries for the same reason). Off for the row the tray registers.</para>
        /// </summary>
        public bool SkipOptionsDialog { get; set; }

        /// <summary>
        /// Asks for the run's options, validates them, runs the core matching and writes the run report.
        /// </summary>
        /// <param name="progress">A progress reporter carrying the running total of link rows produced.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. Returns true when every county group of every dataset was matched; false when the run was refused, cancelled in the dialog or a group failed.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            DateTime started = DateTime.Now;

            if (!SkipOptionsDialog)
            {
                // What the run cannot do without is checked before the dialog (Coding - Background Tasks.md section 2).
                if (gISPostgreSQLConverterManager.GetPostgreSQLConverter<OpenBuildingPostgreSQLConverter>() is null || gISPostgreSQLConverterManager.GetPostgreSQLConverter<Building2DPostgreSQLConverter>() is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {OpenBuildingConverter} (Storage) or no {Building2DConverter} (Main) - the two sides of the matching cannot be read", nameof(UIOpenBuildingMatchTask), nameof(OpenBuildingPostgreSQLConverter), nameof(Building2DPostgreSQLConverter));
                    return false;
                }

                AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
                if (administrativeAreal2DPostgreSQLConverter is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no {Converter} - the county parts cannot be listed to scope the run", nameof(UIOpenBuildingMatchTask), nameof(AdministrativeAreal2DPostgreSQLConverter));
                    return false;
                }

                if (System.Windows.Application.Current is not System.Windows.Application application)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no WPF application is running - the open building match options cannot be asked for", nameof(UIOpenBuildingMatchTask));
                    return false;
                }

                // Read on this thread, shown on the user interface thread.
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(AdministrativeArealType.County, cancellationToken: cancellationToken);
                if (administrativeAreal2DReferences is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the county parts could not be read - the scope cannot be asked for", nameof(UIOpenBuildingMatchTask));
                    return false;
                }

                PostgreSQLOpenBuildingMatchTaskOptions? postgreSQLOpenBuildingMatchTaskOptions = null;

                application.Dispatcher.Invoke(() =>
                {
                    OpenBuildingMatchOptionsWindow openBuildingMatchOptionsWindow = new(PostgreSQLOpenBuildingMatchTaskOptions, administrativeAreal2DReferences);

                    if (openBuildingMatchOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                    {
                        return;
                    }

                    postgreSQLOpenBuildingMatchTaskOptions = openBuildingMatchOptionsWindow.PostgreSQLOpenBuildingMatchTaskOptions;
                });

                // A cancelled dialog ends the run here rather than starting a match nobody scoped.
                if (postgreSQLOpenBuildingMatchTaskOptions is null)
                {
                    Serilog.Modify.Log("{Type}: the open building match options were cancelled - nothing was read or written", nameof(UIOpenBuildingMatchTask));
                    CancelRun();
                    return false;
                }

                PostgreSQLOpenBuildingMatchTaskOptions = postgreSQLOpenBuildingMatchTaskOptions;
            }

            if (Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions) is string message)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {Message} - nothing was matched", nameof(UIOpenBuildingMatchTask), message);
                return false;
            }

            bool succeeded;
            try
            {
                succeeded = await base.ExecuteAsync(progress, cancellationToken);
            }
            catch (Exception exception)
            {
                // The report goes out for a run that faulted or was cancelled too: the groups it finished and the
                // reason belong on disk while they are known.
                WriteReport(started, false, exception, cancellationToken);
                throw;
            }

            WriteReport(started, succeeded, null, cancellationToken);

            return succeeded;
        }

        /// <summary>
        /// Writes the run report next to this application: the datasets, the scope, the rule parameters, whether the links were stored, one line per dataset and county group, and the total measures per dataset (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57).
        /// <para>Written for a refused group, a failed run and a cancelled one as well, and a failure to write it is logged rather than thrown, so a read-only output directory cannot turn a finished run into a failed one. The figures describe the database the tray is connected to.</para>
        /// </summary>
        /// <param name="started">When the run started, for the report's duration.</param>
        /// <param name="succeeded">What the core task answered for the run.</param>
        /// <param name="exception">The exception that ended the run, or null when none escaped the core task.</param>
        /// <param name="cancellationToken">The token the run observed, so the report can say whether it was cancelled.</param>
        private void WriteReport(DateTime started, bool succeeded, Exception? exception, CancellationToken cancellationToken)
        {
            try
            {
                PostgreSQLOpenBuildingMatchTaskOptions options = PostgreSQLOpenBuildingMatchTaskOptions;
                OpenBuildingMatchParameters openBuildingMatchParameters = options.OpenBuildingMatchParameters ?? new OpenBuildingMatchParameters();

                string datasets;
                if (options.DatasetKeys is HashSet<string> datasetKeys)
                {
                    List<string> datasetKeys_Sorted = [.. datasetKeys];
                    datasetKeys_Sorted.Sort(StringComparer.Ordinal);
                    datasets = string.Join(' ', datasetKeys_Sorted);
                }
                else
                {
                    datasets = "all five footprint datasets";
                }

                string scope;
                if (options.CountyIds is HashSet<int> countyIds && countyIds.Count != 0)
                {
                    List<int> countyIds_Sorted = [.. countyIds];
                    countyIds_Sorted.Sort();
                    scope = string.Create(CultureInfo.InvariantCulture, $"county parts ({countyIds_Sorted.Count}, widened to their siblings by the run): {string.Join(' ', countyIds_Sorted)}");
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
                    "Open building matching to BDOT10k",
                    $"Datasets: {datasets}",
                    $"Scope: {scope}",
                    string.Create(CultureInfo.InvariantCulture, $"Parameters: IoU threshold {openBuildingMatchParameters.IoUThreshold}, minimum overlap share {openBuildingMatchParameters.MinOverlapShare}, minimum area {openBuildingMatchParameters.MinArea} m2, max centroid distance {openBuildingMatchParameters.MaxCentroidDistance} m (0 = off), boundary margin {openBuildingMatchParameters.BoundaryMargin} m"),
                    options.Persist ? "Links: stored (the snapshot of every county group in scope was replaced)" : "Links: not stored (sensitivity run - the summaries below are computed in memory)",
                    $"Started: {started.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}",
                    $"Finished: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}",
                    string.Create(CultureInfo.InvariantCulture, $"Link rows: {RowCount}"),
                    string.Create(CultureInfo.InvariantCulture, $"Failed county groups: {FailedGroupCount}"),
                    "Groups (dataset | county parts | register row | BDOT10k | dataset | unusable BDOT10k / dataset | repaired | link rows | components at the margin | stored):"
                ];

                foreach (OpenBuildingMatchResult openBuildingMatchResult in OpenBuildingMatchResults)
                {
                    lines.Add(string.Create(
                        CultureInfo.InvariantCulture,
                        $"  {openBuildingMatchResult.DatasetKey} | {string.Join(',', openBuildingMatchResult.CountyIds ?? [])} | {(openBuildingMatchResult.DatasetId is int datasetId ? datasetId.ToString(CultureInfo.InvariantCulture) : "(not imported)")} | {openBuildingMatchResult.BDOTCount} | {openBuildingMatchResult.DatasetCount} | {openBuildingMatchResult.BDOTRejectedCount} / {openBuildingMatchResult.DatasetRejectedCount} | {openBuildingMatchResult.RepairedCount} | {openBuildingMatchResult.RowCount} | {openBuildingMatchResult.MarginReachCount} | {(openBuildingMatchResult.Persisted ? "yes" : "no")}"));
                }

                lines.Add("Totals per dataset (omission rate | commission rate | one-to-one | one-to-one weak | 1:n | n:1 | n:m | median IoU | mean dx | mean dy):");
                foreach (OpenBuildingMatchSummaryResult openBuildingMatchSummaryResult in OpenBuildingMatchSummaryResults)
                {
                    if (openBuildingMatchSummaryResult.PrimarySource is not null)
                    {
                        continue;
                    }

                    lines.Add(string.Create(
                        CultureInfo.InvariantCulture,
                        $"  {openBuildingMatchSummaryResult.DatasetKey} (register row {openBuildingMatchSummaryResult.DatasetId}) | {Format(openBuildingMatchSummaryResult.OmissionRate)} | {Format(openBuildingMatchSummaryResult.CommissionRate)} | {openBuildingMatchSummaryResult.OneToOneCount} | {openBuildingMatchSummaryResult.OneToOneWeakCount} | {openBuildingMatchSummaryResult.OneToManyCount} | {openBuildingMatchSummaryResult.ManyToOneCount} | {openBuildingMatchSummaryResult.ManyToManyCount} | {Format(openBuildingMatchSummaryResult.IoUMedian)} | {Format(openBuildingMatchSummaryResult.DxMean)} | {Format(openBuildingMatchSummaryResult.DyMean)}"));
                }

                lines.Add($"Cancelled: {cancellationToken.IsCancellationRequested}");
                lines.Add($"Succeeded: {succeeded}");
                lines.Add($"Exception: {exception?.ToString() ?? "(none)"}");

                string path_Report = Path.Combine(AppContext.BaseDirectory, ReportFileName);
                File.WriteAllLines(path_Report, lines, Encoding.UTF8);

                Serilog.Modify.Log("Open building match report written to {Path}", path_Report);
            }
            catch (Exception exception_Temp)
            {
                Serilog.Modify.Log(exception_Temp, "{Type}: the run report could not be written", nameof(UIOpenBuildingMatchTask));
            }
        }

        /// <summary>
        /// Formats an optional measure for the report with four decimals in the invariant culture, or as a dash when there is none.
        /// </summary>
        /// <param name="value">The measure.</param>
        /// <returns>The formatted text.</returns>
        private static string Format(double? value)
        {
            return value is double value_Temp ? value_Temp.ToString("0.0000", CultureInfo.InvariantCulture) : "-";
        }
    }
}
