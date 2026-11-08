using DiGi.Core.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Removes the <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> rows whose building no longer exists under the county part holding them.
    /// <para>An orphan is a model held under a part whose <c>building_2d</c> no longer holds the building it describes, which is what a county part repair run can leave behind when it re-files a building under the part its footprint lies in.</para>
    /// <para><b>Reports by default and writes nothing.</b> <see cref="DryRun"/> has to be turned off deliberately, and the counts it reports first are what the delete should be reviewed against - the rows removed here have no undo.</para>
    /// <para>The report is written as files into <see cref="ReportDirectory"/> as well as to the log: <c>BuildingModels_Cleanup.csv</c> naming every orphaned reference, and <c>BuildingModels_Cleanup_Summary.txt</c> carrying the totals. The files are what the decision to delete should rest on - a log is shared with whatever else the application is doing and rolls by day.</para>
    /// </summary>
    public class PostgreSQLBuildingModelCleanupTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private readonly GISPostgreSQLConverterManager gISPostgreSQLConverterManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingModelCleanupTask"/> class.
        /// </summary>
        /// <param name="gISPostgreSQLConverterManager">The manager holding the PostgreSQL converters.</param>
        public PostgreSQLBuildingModelCleanupTask(GISPostgreSQLConverterManager gISPostgreSQLConverterManager)
        {
            this.gISPostgreSQLConverterManager = gISPostgreSQLConverterManager;
        }

        /// <summary>
        /// Gets or sets the identifiers of the county rows to clean. When null every county row is examined.
        /// <para>These are polygon parts, not counties - a multi-part county holds one row per part and each is cleaned on its own.</para>
        /// </summary>
        public IEnumerable<int>? CountyIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the task only reports what it would do. Defaults to <see langword="true"/>; nothing is written until it is turned off.
        /// </summary>
        public bool DryRun { get; set; } = true;

        /// <summary>
        /// Gets or sets the directory the two report files are written into. When null the directory the application was launched from is used.
        /// <para>Deliberately not a folder dialog: this runs on a thread pool thread, where a WPF common dialog needs an STA apartment and throws instead of opening.</para>
        /// </summary>
        public string? ReportDirectory { get; set; }

        /// <summary>
        /// Gets or sets the two-digit voivodeship codes to be cleaned. A county row is in scope when its code starts with one of them. When null every voivodeship is cleaned. Combined with <see cref="CountyIds"/> both filters have to admit the row.
        /// <para>This is what makes the national regeneration affordable: a voivodeship is regenerated and then cleaned before the next one starts, so the storage tablespace only ever carries a second copy of one voivodeship rather than of the whole country.</para>
        /// </summary>
        public IEnumerable<string>? VoivodeshipCodes { get; set; }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            string directory = string.IsNullOrWhiteSpace(ReportDirectory) ? AppContext.BaseDirectory : ReportDirectory!;

            if (!Directory.Exists(directory))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Report directory {Directory} does not exist", directory);
                return false;
            }

            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager?.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            BuildingModelPostgreSQLConverter? buildingModelPostgreSQLConverter = gISPostgreSQLConverterManager?.GetPostgreSQLConverter<BuildingModelPostgreSQLConverter>();

            if (administrativeAreal2DPostgreSQLConverter is null || buildingModelPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "PostgreSQL converters could not be resolved");
                return false;
            }

            // building_2d and building_model live in different databases, so the two reference sets cannot be
            // compared in one statement - the buildings have to be read through their own converter.
            Building2DPostgreSQLConverter? building2DPostgreSQLConverter = gISPostgreSQLConverterManager?.GetPostgreSQLConverter<Building2DPostgreSQLConverter>();
            if (building2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2D converter could not be resolved");
                return false;
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByAdministrativeArealType(AdministrativeArealType.County, cancellationToken: cancellationToken);
            if (administrativeAreal2Ds is null || administrativeAreal2Ds.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County rows could not be retrieved");
                return false;
            }

            HashSet<int>? countyIds = CountyIds is null ? null : [.. CountyIds];
            HashSet<string>? voivodeshipCodes = VoivodeshipCodes is null ? null : [.. VoivodeshipCodes];

            List<int> countyIds_Cleaned = [];
            foreach (AdministrativeAreal2D administrativeAreal2D in administrativeAreal2Ds)
            {
                int countyId = administrativeAreal2D.Id;
                if (PostgreSQL.Query.IsInScope(countyId, administrativeAreal2D.Code, countyIds, voivodeshipCodes))
                {
                    countyIds_Cleaned.Add(countyId);
                }
            }

            if (countyIds_Cleaned.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No county row is in scope - nothing to clean up");
                return false;
            }

            countyIds_Cleaned.Sort();

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            Serilog.Modify.Log("{Type} started. DryRun: {DryRun}. VoivodeshipCodes: {VoivodeshipCodes}. County rows examined: {Count}", nameof(PostgreSQLBuildingModelCleanupTask), DryRun, voivodeshipCodes is null ? "all" : string.Join(' ', voivodeshipCodes), countyIds_Cleaned.Count);

            long count_Orphaned = 0;
            long count_Deleted = 0;

            List<string> summaryLines =
            [
                "BuildingModel cleanup",
                $"Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"DryRun: {DryRun}",
                $"VoivodeshipCodes: {(voivodeshipCodes is null ? "all" : string.Join(' ', voivodeshipCodes))}",
                $"County rows examined: {countyIds_Cleaned.Count}",
                string.Empty,
                "CountyId;OrphanReferences"
            ];

            // One row per orphaned reference: these are the models that lose their only row.
            using StreamWriter streamWriter = new(System.IO.Path.Combine(directory, "BuildingModels_Cleanup.csv"), false, Encoding.UTF8);
            await streamWriter.WriteLineAsync("CountyId;Reference");

            foreach (int countyId in countyIds_Cleaned)
            {
                cancellationToken.ThrowIfCancellationRequested();

                List<string> references_Orphaned = await ReferencesOrphanedAsync(buildingModelPostgreSQLConverter, building2DPostgreSQLConverter, countyId, cancellationToken);

                if (references_Orphaned.Count == 0)
                {
                    continue;
                }

                Serilog.Modify.Log("Part {CountyId}: references without a building {Orphaned}", countyId, references_Orphaned.Count);

                summaryLines.Add($"{countyId};{references_Orphaned.Count}");

                foreach (string reference_Orphaned in references_Orphaned)
                {
                    await streamWriter.WriteLineAsync($"{countyId};{reference_Orphaned}");
                }

                // Flushed per part so a run interrupted late still leaves everything already decided on disk.
                await streamWriter.FlushAsync(cancellationToken);

                count_Orphaned += references_Orphaned.Count;

                if (DryRun)
                {
                    longProgressWrapper?.Increment(references_Orphaned.Count);
                    continue;
                }

                if (references_Orphaned.Count != 0)
                {
                    HashSet<long>? ids = await buildingModelPostgreSQLConverter.RemoveAsync(references_Orphaned, countyId, cancellationToken: cancellationToken);

                    int count = ids?.Count ?? 0;
                    count_Deleted += count;

                    // More rows than references is the normal case rather than a fault - a reference the upsert could
                    // not match holds one row per run that wrote it. Fewer means the table no longer holds what was read.
                    if (count < references_Orphaned.Count)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Part {CountyId}: {Deleted} rows deleted for {Counted} references without a building - the table did not hold what was read", countyId, count, references_Orphaned.Count);
                    }

                    longProgressWrapper?.Increment(count);
                }
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add($"References without a building: {count_Orphaned}");
            summaryLines.Add($"Rows deleted: {count_Deleted}");
            summaryLines.Add($"Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            await streamWriter.FlushAsync(cancellationToken);

            await File.WriteAllLinesAsync(System.IO.Path.Combine(directory, "BuildingModels_Cleanup_Summary.txt"), summaryLines, cancellationToken);

            Serilog.Modify.Log("{Type} ended. DryRun: {DryRun}. References without a building {Orphaned}, rows deleted {Deleted}. Report written to {Directory}", nameof(PostgreSQLBuildingModelCleanupTask), DryRun, count_Orphaned, count_Deleted, directory);

            return true;
        }

        /// <summary>
        /// Returns the references a county row holds a model for but no longer holds a building for.
        /// </summary>
        /// <param name="buildingModelPostgreSQLConverter">The converter reading the model table.</param>
        /// <param name="building2DPostgreSQLConverter">The converter reading the building table, which lives in a different database.</param>
        /// <param name="countyId">The identifier of the county row to compare.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the orphaned references, empty when either side could not be read.</returns>
        private static async Task<List<string>> ReferencesOrphanedAsync(BuildingModelPostgreSQLConverter buildingModelPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, int countyId, CancellationToken cancellationToken)
        {
            HashSet<string>? references_Model = await buildingModelPostgreSQLConverter.GetReferencesAsync(countyId, cancellationToken: cancellationToken);
            if (references_Model is null || references_Model.Count == 0)
            {
                return [];
            }

            // GetBuilding2DReferencesByAdministrativeAreal2DIdsAsync resolves through Subdivision children and
            // answers empty for a part with none, which is not the same thing as the part holding no buildings.
            // The excluded set is declared rather than passed inline: the paging overload is otherwise an equally
            // good match for a bare null and the call does not compile.
            IEnumerable<string>? references_Excluded = null;

            List<Building2DReference>? building2DReferences = await building2DPostgreSQLConverter.GetBuilding2DReferencesByCountyIdAsync(countyId, null, references_Excluded, cancellationToken: cancellationToken);
            if (building2DReferences is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Part {CountyId}: Building2D references could not be read - orphans not looked for", countyId);
                return [];
            }

            HashSet<string> references_Building2D = [];
            foreach (Building2DReference building2DReference in building2DReferences)
            {
                string? reference = building2DReference?.Reference;
                if (!string.IsNullOrWhiteSpace(reference))
                {
                    references_Building2D.Add(reference!);
                }
            }

            // An empty building side would condemn every model of the part, which is far more likely to mean the
            // read went wrong than that the part really lost all its buildings.
            if (references_Building2D.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Part {CountyId}: holds {Count} models and no buildings at all - left alone rather than emptied", countyId, references_Model.Count);
                return [];
            }

            return [.. references_Model.Where(x => !references_Building2D.Contains(x))];
        }
    }
}
