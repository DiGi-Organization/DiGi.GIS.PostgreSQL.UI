using DiGi.Analytical.Building.Enums;
using DiGi.Core.Classes;
using DiGi.GIS.Analytical;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.PostgreSQL.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Stamps the WGS 84 coordinates and the Polish standard-time UTC onto the <c>BuildingInformation</c> of the <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> rows stored in the building model tables.
    /// <para>The rows the national regeneration wrote before the creation paths started stamping carry no coordinates at all, so every stored model computes its sun path at (0, 0). The task walks each county part of each level table by identifier, classifies every row, and writes back the one JSON key it changes - <c>BuildingInformation</c> - leaving the rest of the stored object exactly as it is.</para>
    /// <para><b>Reports by default and writes nothing.</b> <see cref="DryRun"/> has to be turned off deliberately, and the summary it reports first - how many rows would be stamped and which are rejected - is what the real run should be reviewed against.</para>
    /// <para><b>The run is idempotent and checkpointed.</b> A row whose stored values already equal the computed ones is reported <c>AlreadyCorrect</c> and not written, so a second run reports 0 updated. Finished parts are appended to <c>BuildingModels_BuildingInformation_Checkpoint.txt</c> and skipped on a resumed run; a dry run neither reads nor writes it, so a dry run followed by a real run with <see cref="Resume"/> still touches every part.</para>
    /// <para>The scope - the county parts, the detail level, the page size, the statement timeout, the checkpoint behaviour and the report directory - is asked for each time the task starts, through <see cref="PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow"/>; a cancelled dialog ends the run with nothing written.</para>
    /// </summary>
    public class UIPostgreSQLBuildingModelBuildingInformationUpdateTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private const string StatusFailed = "Failed";
        private const string StatusRejected = "Rejected";
        private const string StatusAlreadyCorrect = "AlreadyCorrect";
        private const string StatusToUpdate = "ToUpdate";

        private const string FileName_Checkpoint = "BuildingModels_BuildingInformation_Checkpoint.txt";
        private const string FileName_CSV = "BuildingModels_BuildingInformation.csv";
        private const string FileName_Summary = "BuildingModels_BuildingInformation_Summary.txt";
        private const string PropertyName_BuildingInformation = "BuildingInformation";

        private readonly GISPostgreSQLConverterManager gISPostgreSQLConverterManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLBuildingModelBuildingInformationUpdateTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The manager holding the PostgreSQL converters.</param>
        public UIPostgreSQLBuildingModelBuildingInformationUpdateTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
        {
            this.gISPostgreSQLConverterManager = gISPostgreSQLConverterManager;
        }

        /// <summary>
        /// Gets or sets the identifiers of the county rows to stamp. When null every county row is in scope.
        /// <para>These are polygon parts, not counties - a multi-part county holds one row per part and each is walked on its own.</para>
        /// </summary>
        public IEnumerable<int>? CountyIds { get; set; }

        /// <summary>
        /// Gets or sets the two-digit voivodeship codes to be stamped. A county row is in scope when its code starts with one of them. When null every voivodeship is in scope. Combined with <see cref="CountyIds"/> both filters have to admit the row.
        /// </summary>
        public IEnumerable<string>? VoivodeshipCodes { get; set; }

        /// <summary>
        /// Gets or sets the detail level of the building model table to walk. When null every level whose table exists is walked.
        /// </summary>
        public BuildingModelDetailLevel? BuildingModelDetailLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the task only reports what it would stamp. Defaults to <see langword="true"/>; nothing is written until it is turned off.
        /// </summary>
        public bool DryRun { get; set; } = true;

        /// <summary>
        /// Gets or sets the number of rows read and classified per page.
        /// </summary>
        public int BatchSize { get; set; } = 500;

        /// <summary>
        /// Gets or sets the timeout in seconds applied to the statements. A value of 0 disables the timeout.
        /// </summary>
        public int CommandTimeout { get; set; } = 120;

        /// <summary>
        /// Gets or sets a value indicating whether the parts named in the checkpoint of an earlier run are skipped. Defaults to <see langword="true"/>.
        /// <para>The national pass has to survive being interrupted mid-country, so it resumes rather than restarting from the first part. Turning this off starts from the first part in scope and truncates the checkpoint, which is what a deliberate re-run of an already-completed scope needs.</para>
        /// </summary>
        public bool Resume { get; set; } = true;

        /// <summary>
        /// Gets or sets the directory the checkpoint and the reports are written into. When null the directory the application was launched from is used.
        /// <para>Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment and throws instead of opening.</para>
        /// </summary>
        public string? ReportDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the options window is skipped and the properties of this task are used as they are. Defaults to <see langword="false"/>.
        /// <para>A run started from the tray always asks for its scope through <see cref="PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow"/>. A caller that sets the options itself - a test, or a host that runs the task without the user interface - turns this on so <see cref="ExecuteAsync(IProgress{long}, CancellationToken)"/> never asks for them.</para>
        /// </summary>
        public bool SkipOptionsDialog { get; set; }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager?.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            BuildingModelPostgreSQLConverter? buildingModelPostgreSQLConverter = gISPostgreSQLConverterManager?.GetPostgreSQLConverter<BuildingModelPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null || buildingModelPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "PostgreSQL converters could not be resolved");
                return false;
            }

            ConnectionData? connectionData = buildingModelPostgreSQLConverter.ConnectionData;
            if (connectionData is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No connection data for the building model tables");
                return false;
            }

            // One row per polygon part, 406 of them for the 380 codes: uniqueCode stays false, or a
            // multi-part county would collapse to one part and lose the rest of its territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(AdministrativeArealType.County, parentId: null, uniqueCode: false, commandTimeout: CommandTimeout, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County references could not be retrieved");
                return false;
            }

            if (!SkipOptionsDialog)
            {
                // The dialog is a window, and this runs on a thread pool thread, where a window cannot be
                // created at all. Without an application there is no user interface thread to move it to.
                if (System.Windows.Application.Current is not System.Windows.Application application)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the BuildingInformation options cannot be asked for");
                    return false;
                }

                // Read on this thread, shown on the user interface thread. The options carry the task's
                // current scope, so the dialog opens pre-filled with what the last run used.
                PostgreSQLBuildingModelBuildingInformationUpdateOptions options = new()
                {
                    CountyIds = CountyIds is null ? null : [.. CountyIds],
                    BuildingModelDetailLevel = BuildingModelDetailLevel,
                    DryRun = DryRun,
                    BatchSize = BatchSize,
                    CommandTimeout = CommandTimeout,
                    Resume = Resume,
                    ReportDirectory = ReportDirectory
                };

                PostgreSQLBuildingModelBuildingInformationUpdateOptions? options_Dialog = null;

                application.Dispatcher.Invoke(() =>
                {
                    PostgreSQLBuildingModelBuildingInformationUpdateOptionsWindow optionsWindow = new(options, administrativeAreal2DReferences);

                    if (optionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                    {
                        return;
                    }

                    options_Dialog = optionsWindow.PostgreSQLBuildingModelBuildingInformationUpdateOptions;
                });

                // A cancelled dialog leaves the settings of an earlier run as they were - the window works
                // on a copy - and ends the run here rather than starting a national pass nobody asked for.
                if (options_Dialog is null)
                {
                    Serilog.Modify.Log("BuildingInformation options were cancelled - nothing was written");
                    return false;
                }

                CountyIds = options_Dialog.CountyIds;
                BuildingModelDetailLevel = options_Dialog.BuildingModelDetailLevel;
                DryRun = options_Dialog.DryRun;
                BatchSize = options_Dialog.BatchSize;
                CommandTimeout = options_Dialog.CommandTimeout;
                Resume = options_Dialog.Resume;
                ReportDirectory = options_Dialog.ReportDirectory;
            }

            // The directory may have been chosen in the dialog, so it is resolved here rather than before it.
            string directory = string.IsNullOrWhiteSpace(ReportDirectory) ? AppContext.BaseDirectory : ReportDirectory!;
            if (!Directory.Exists(directory))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Report directory {Directory} does not exist", directory);
                return false;
            }

            HashSet<int>? countyIds = CountyIds is null ? null : [.. CountyIds];
            HashSet<string>? voivodeshipCodes = VoivodeshipCodes is null ? null : [.. VoivodeshipCodes];

            List<int> countyIds_InScope = [];
            Dictionary<int, string> code_ByCountyId = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (administrativeAreal2DReference is null)
                {
                    continue;
                }

                if (PostgreSQL.Query.IsInScope(administrativeAreal2DReference.Id, administrativeAreal2DReference.Code, countyIds, voivodeshipCodes))
                {
                    countyIds_InScope.Add(administrativeAreal2DReference.Id);
                    code_ByCountyId[administrativeAreal2DReference.Id] = administrativeAreal2DReference.Code ?? string.Empty;
                }
            }

            if (countyIds_InScope.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No county row is in scope - nothing to stamp");
                return false;
            }

            countyIds_InScope.Sort();

            int batchSize = BatchSize < 1 ? 1 : BatchSize;

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            string path_Checkpoint = System.IO.Path.Combine(directory, FileName_Checkpoint);

            // A dry run neither reads nor writes the checkpoint: a dry run followed by a real run with
            // Resume on would otherwise skip everything the dry run touched.
            HashSet<string> checkpoint = [];
            if (Resume && !DryRun && File.Exists(path_Checkpoint))
            {
                foreach (string line in await File.ReadAllLinesAsync(path_Checkpoint, cancellationToken))
                {
                    string line_Trimmed = line.Trim();
                    if (!string.IsNullOrWhiteSpace(line_Trimmed))
                    {
                        checkpoint.Add(line_Trimmed);
                    }
                }

                Serilog.Modify.Log("Resuming from {Path}: {Count} parts already completed are skipped", path_Checkpoint, checkpoint.Count);
            }

            StreamWriter? streamWriter_Checkpoint = null;
            if (!DryRun)
            {
                streamWriter_Checkpoint = new(path_Checkpoint, Resume, Encoding.UTF8);
            }

            List<BuildingModelDetailLevel> levels = BuildingModelDetailLevel is null ? [.. Enum.GetValues<BuildingModelDetailLevel>()] : [BuildingModelDetailLevel.Value];

            Serilog.Modify.Log("{Type} started. DryRun: {DryRun}. VoivodeshipCodes: {VoivodeshipCodes}. County parts in scope: {Count}. Levels: {Levels}", nameof(UIPostgreSQLBuildingModelBuildingInformationUpdateTask), DryRun, voivodeshipCodes is null ? "all" : string.Join(' ', voivodeshipCodes), countyIds_InScope.Count, BuildingModelDetailLevel is null ? "all existing tables" : BuildingModelDetailLevel.Value.ToString());

            List<string> summaryLines =
            [
                "BuildingModel BuildingInformation update",
                $"Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"DryRun: {DryRun}{(DryRun ? " (Updated means would update)" : string.Empty)}",
                $"VoivodeshipCodes: {(voivodeshipCodes is null ? "all" : string.Join(' ', voivodeshipCodes))}",
                $"County parts in scope: {countyIds_InScope.Count}",
                $"Levels: {(BuildingModelDetailLevel is null ? "all existing tables" : BuildingModelDetailLevel.Value.ToString())}",
                string.Empty,
                "Table;Code;CountyId;Scanned;Updated;AlreadyCorrect;Rejected;Failed"
            ];

            List<string> parts_Failed = [];

            long count_Scanned = 0;
            long count_Updated = 0;
            long count_AlreadyCorrect = 0;
            long count_Rejected = 0;
            long count_Failed = 0;

            using StreamWriter streamWriter_CSV = new(System.IO.Path.Combine(directory, FileName_CSV), false, Encoding.UTF8);
            await streamWriter_CSV.WriteLineAsync("Code;CountyId;Table;Id;Reference;Status;Reason");

            foreach (BuildingModelDetailLevel level in levels)
            {
                cancellationToken.ThrowIfCancellationRequested();

                BuildingModelPostgreSQLConverter buildingModelConverter = new(connectionData, level);

                // The table is checked, never created: the DDL that creates it builds the indexes, which
                // a stamping run has no business triggering.
                bool exists = await DiGi.PostgreSQL.Query.TableExistsAsync(connectionData, buildingModelConverter.TableName, commandTimeout: CommandTimeout, cancellationToken: cancellationToken);
                if (!exists)
                {
                    Serilog.Modify.Log("Table {Table} does not exist - the level is skipped", buildingModelConverter.TableName);
                    continue;
                }

                long count_Scanned_Level = 0;
                long count_Updated_Level = 0;
                long count_AlreadyCorrect_Level = 0;
                long count_Rejected_Level = 0;
                long count_Failed_Level = 0;

                foreach (int countyId in countyIds_InScope)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string checkpointKey = $"{buildingModelConverter.TableName};{countyId}";
                    if (!DryRun && checkpoint.Contains(checkpointKey))
                    {
                        continue;
                    }

                    string code = code_ByCountyId[countyId];

                    long count_Scanned_Part = 0;
                    long count_Updated_Part = 0;
                    long count_AlreadyCorrect_Part = 0;
                    long count_Rejected_Part = 0;
                    long count_Failed_Part = 0;

                    bool partSucceeded = true;

                    try
                    {
                        await using NpgsqlConnection? npgsqlConnection = DiGi.PostgreSQL.Create.NpgsqlConnection(connectionData);
                        if (npgsqlConnection is null)
                        {
                            partSucceeded = false;
                        }
                        else
                        {
                            await npgsqlConnection.OpenAsync(cancellationToken);

                            // The upper bound is read once when the part starts: rows a concurrent
                            // regeneration inserts afterwards are not chased by the walk - they are
                            // stamped by the creation path that writes them.
                            long? idMax = await buildingModelConverter.GetMaxIdAsync(npgsqlConnection, countyId, commandTimeout: CommandTimeout, cancellationToken: cancellationToken);
                            if (idMax is null)
                            {
                                // An empty part is recorded and checkpointed rather than paged: it stays
                                // empty for this run, and a concurrent regeneration that fills it
                                // afterwards stamps the rows it writes.
                            }
                            else
                            {
                                long idAfter = 0;
                                while (true)
                                {
                                    cancellationToken.ThrowIfCancellationRequested();

                                    List<BuildingModel>? items = await buildingModelConverter.GetItemsByIdRangeAsync(npgsqlConnection, countyId, idAfter, idMax.Value, batchSize, commandTimeout: CommandTimeout, cancellationToken: cancellationToken);
                                    if (items is null)
                                    {
                                        // A read that answers null did not fail with an exception, so it is
                                        // handled here rather than in the catch: the part is failed and the
                                        // walk stopped, and the part is not checkpointed.
                                        partSucceeded = false;
                                        break;
                                    }

                                    if (items.Count == 0)
                                    {
                                        break;
                                    }

                                    // The deserialization is CPU-bound, so the page is classified in
                                    // parallel, with the results written into per-index slots - no shared
                                    // state to lock.
                                    RowResult[] results = new RowResult[items.Count];
                                    Parallel.For(0, items.Count, Core.Create.ParallelOptions(), i =>
                                    {
                                        results[i] = Classify(items[i]);
                                    });

                                    // The page stands or falls together: one transaction for every update
                                    // of the page, and an id the write does not answer was deleted between
                                    // the read and the write by a concurrent regeneration - it is reported
                                    // rather than retried, and the page still commits.
                                    if (!DryRun)
                                    {
                                        bool hasToUpdate = false;
                                        for (int i = 0; i < items.Count; i++)
                                        {
                                            if (results[i].Status == StatusToUpdate)
                                            {
                                                hasToUpdate = true;
                                                break;
                                            }
                                        }

                                        if (hasToUpdate)
                                        {
                                            await using NpgsqlTransaction npgsqlTransaction = await npgsqlConnection.BeginTransactionAsync(cancellationToken);
                                            List<KeyValuePair<long, JsonNode?>> values = [];
                                            for (int i = 0; i < items.Count; i++)
                                            {
                                                if (results[i].Status == StatusToUpdate)
                                                {
                                                    values.Add(new KeyValuePair<long, JsonNode?>(items[i].Id, results[i].Value));
                                                }
                                            }

                                            HashSet<long>? ids_Updated = await buildingModelConverter.UpdateObjectPropertiesAsync(npgsqlConnection, npgsqlTransaction, countyId, PropertyName_BuildingInformation, values, commandTimeout: CommandTimeout, cancellationToken: cancellationToken);
                                            await npgsqlTransaction.CommitAsync(cancellationToken);

                                            for (int i = 0; i < items.Count; i++)
                                            {
                                                if (results[i].Status == StatusToUpdate && (ids_Updated is null || !ids_Updated.Contains(items[i].Id)))
                                                {
                                                    results[i].Status = StatusFailed;
                                                    results[i].Reason = "RowNotFound";
                                                    results[i].Value = null;
                                                }
                                            }
                                        }
                                    }

                                    for (int i = 0; i < items.Count; i++)
                                    {
                                        RowResult rowResult = results[i];
                                        switch (rowResult.Status)
                                        {
                                            case StatusAlreadyCorrect:
                                                count_AlreadyCorrect_Part++;
                                                break;
                                            case StatusToUpdate:
                                                count_Updated_Part++;
                                                break;
                                            case StatusRejected:
                                                count_Rejected_Part++;
                                                await streamWriter_CSV.WriteLineAsync($"{code};{countyId};{buildingModelConverter.TableName};{items[i].Id};{items[i].Reference};{StatusRejected};{rowResult.Reason}");
                                                break;
                                            case StatusFailed:
                                                count_Failed_Part++;
                                                await streamWriter_CSV.WriteLineAsync($"{code};{countyId};{buildingModelConverter.TableName};{items[i].Id};{items[i].Reference};{StatusFailed};{rowResult.Reason}");
                                                break;
                                        }
                                    }

                                    // Progress is the scanned rows, which is what a run of this shape is
                                    // measured in: the write is a fraction of the cost.
                                    longProgressWrapper?.Increment(items.Count);

                                    count_Scanned_Part += items.Count;
                                    idAfter = items[items.Count - 1].Id;

                                    if (items.Count < batchSize)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                    {
                        // The page transaction is rolled back when its using statement is left, and the
                        // part is not checkpointed, so a re-run redoes it. That is safe because the run
                        // is idempotent.
                        Serilog.Modify.Log(exception, "Part {CountyId} ({Table}) failed - it is not checkpointed and a re-run redoes it", countyId, buildingModelConverter.TableName);
                        partSucceeded = false;
                    }

                    summaryLines.Add($"{buildingModelConverter.TableName};{code};{countyId};{count_Scanned_Part};{count_Updated_Part};{count_AlreadyCorrect_Part};{count_Rejected_Part};{count_Failed_Part}");

                    // Flushed per part so a run interrupted late still leaves everything already decided
                    // on disk.
                    await streamWriter_CSV.FlushAsync(cancellationToken);

                    if (partSucceeded)
                    {
                        if (count_Scanned_Part != 0)
                        {
                            Serilog.Modify.Log("Part {CountyId} ({Table}): scanned {Scanned}, updated {Updated}, already correct {AlreadyCorrect}, rejected {Rejected}, failed {Failed}", countyId, buildingModelConverter.TableName, count_Scanned_Part, count_Updated_Part, count_AlreadyCorrect_Part, count_Rejected_Part, count_Failed_Part);
                        }

                        count_Scanned_Level += count_Scanned_Part;
                        count_Updated_Level += count_Updated_Part;
                        count_AlreadyCorrect_Level += count_AlreadyCorrect_Part;
                        count_Rejected_Level += count_Rejected_Part;
                        count_Failed_Level += count_Failed_Part;

                        if (streamWriter_Checkpoint is not null)
                        {
                            await streamWriter_Checkpoint.WriteLineAsync(checkpointKey);
                            await streamWriter_Checkpoint.FlushAsync(cancellationToken);
                        }
                    }
                    else
                    {
                        parts_Failed.Add(checkpointKey);
                    }
                }

                if (count_Scanned_Level != 0)
                {
                    summaryLines.Add(string.Empty);
                    summaryLines.Add($"=== {buildingModelConverter.TableName.ToUpperInvariant()} ===");
                    summaryLines.Add("Table;Scanned;Updated;AlreadyCorrect;Rejected;Failed");
                    summaryLines.Add($"{buildingModelConverter.TableName};{count_Scanned_Level};{count_Updated_Level};{count_AlreadyCorrect_Level};{count_Rejected_Level};{count_Failed_Level}");
                }

                count_Scanned += count_Scanned_Level;
                count_Updated += count_Updated_Level;
                count_AlreadyCorrect += count_AlreadyCorrect_Level;
                count_Rejected += count_Rejected_Level;
                count_Failed += count_Failed_Level;
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== TOTALS ===");
            summaryLines.Add("Table;Scanned;Updated;AlreadyCorrect;Rejected;Failed");
            summaryLines.Add($"(all tables);{count_Scanned};{count_Updated};{count_AlreadyCorrect};{count_Rejected};{count_Failed}");

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== FAILED PARTS ===");
            if (parts_Failed.Count == 0)
            {
                summaryLines.Add("none");
            }
            else
            {
                foreach (string part_Failed in parts_Failed)
                {
                    summaryLines.Add(part_Failed);
                }
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add($"Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            await File.WriteAllLinesAsync(System.IO.Path.Combine(directory, FileName_Summary), summaryLines, cancellationToken);

            if (streamWriter_Checkpoint is not null)
            {
                // The run is over and every line is flushed, so the file can be closed now: left open, it
                // holds the checkpoint locked on the operating system until the stream is garbage collected,
                // which on Windows is when an operator opening the file after the run finds it in use.
                await streamWriter_Checkpoint.DisposeAsync();
            }

            Serilog.Modify.Log("{Type} ended. DryRun: {DryRun}. Scanned {Scanned}, updated {Updated}, already correct {AlreadyCorrect}, rejected {Rejected}, failed {Failed}, failed parts {FailedParts}. Report written to {Directory}", nameof(UIPostgreSQLBuildingModelBuildingInformationUpdateTask), DryRun, count_Scanned, count_Updated, count_AlreadyCorrect, count_Rejected, count_Failed, parts_Failed.Count, directory);

            return parts_Failed.Count == 0;
        }

        /// <summary>
        /// Classifies one stored row: what the run would do with it and, for the outcomes that are reported, why.
        /// <para>The stored values are taken before <c>UpdateBuildingInformation</c> is called, because the method always overwrites: the comparison against them is what makes a row already at its target values <c>AlreadyCorrect</c> rather than <c>ToUpdate</c>, and a second run report 0 updated.</para>
        /// </summary>
        /// <param name="buildingModel">The stored row to classify.</param>
        /// <returns>The classification: <c>Failed</c> with a deserialization or exception reason, <c>Rejected</c> when the model cannot be located, <c>AlreadyCorrect</c> when the stored values already equal the computed ones, or <c>ToUpdate</c> carrying the value to write.</returns>
        private static RowResult Classify(BuildingModel buildingModel)
        {
            RowResult result = new();

            try
            {
                DiGi.Analytical.Building.Classes.BuildingModel? buildingModel_Analytical = buildingModel.ToDiGi();
                if (buildingModel_Analytical is null)
                {
                    result.Status = StatusFailed;
                    result.Reason = "Deserialization";
                    return result;
                }

                DiGi.Analytical.Building.Classes.BuildingInformation buildingInformation_Stored = buildingModel_Analytical.BuildingInformation;
                Coordinates? coordinates_Stored = buildingInformation_Stored.Coordinates;
                double latitude_Stored = coordinates_Stored is null ? 0 : coordinates_Stored.Latitude;
                double longitude_Stored = coordinates_Stored is null ? 0 : coordinates_Stored.Longitude;
                Core.Enums.UTC utc_Stored = buildingInformation_Stored.UTC;

                bool located = buildingModel_Analytical.UpdateBuildingInformation();
                if (!located)
                {
                    result.Status = StatusRejected;
                    // The two ways the method refuses: no bounding box to take a centre from, and a
                    // centre that converts outside the Polish range, which means the geometry is not in
                    // EPSG:2180.
                    result.Reason = buildingModel_Analytical.GetBoundingBox() is null ? "NoBoundingBox" : "OutsidePolishRange";
                    return result;
                }

                // Compared exactly: the EPSG:2180 to WGS 84 conversion is deterministic and the
                // serializer round-trips a double exactly, so a row at its target values compares equal.
                Coordinates coordinates = buildingModel_Analytical.BuildingInformation.Coordinates!;
                if (coordinates.Latitude == latitude_Stored && coordinates.Longitude == longitude_Stored && buildingModel_Analytical.BuildingInformation.UTC == utc_Stored)
                {
                    result.Status = StatusAlreadyCorrect;
                    return result;
                }

                result.Status = StatusToUpdate;
                result.Value = buildingModel_Analytical.BuildingInformation.ToJsonObject();
                return result;
            }
            catch (Exception exception)
            {
                result.Status = StatusFailed;
                result.Reason = $"Exception: {exception.GetType().Name}: {exception.Message}";
                return result;
            }
        }

        /// <summary>
        /// The classification of one stored row: its status, the reason for the outcomes that are reported, and, for <c>ToUpdate</c> rows, the value to write.
        /// </summary>
        private class RowResult
        {
            /// <summary>
            /// One of <c>Failed</c>, <c>Rejected</c>, <c>AlreadyCorrect</c> or <c>ToUpdate</c>.
            /// </summary>
            public string Status = string.Empty;

            /// <summary>
            /// The reason for a <c>Failed</c> or <c>Rejected</c> row; null otherwise.
            /// </summary>
            public string? Reason;

            /// <summary>
            /// The value to write for a <c>ToUpdate</c> row; null otherwise.
            /// </summary>
            public JsonNode? Value;
        }
    }
}
