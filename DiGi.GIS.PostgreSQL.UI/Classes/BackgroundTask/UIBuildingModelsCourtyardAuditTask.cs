using DiGi.Analytical.Building;
using DiGi.Core.Classes;
using DiGi.GIS.Analytical.Enums;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using DiGi.Geometry.Planar.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A read-only task that locates the stored building models whose courtyard the storey split filled with a solid face.
    /// <para>For every county in scope it walks every 2D building reference and classifies it against the defect fixed by DiGi.Geometry#7: the source CityGML <see cref="CityGML.Classes.Building"/> carries a <c>GroundSurface</c> with an interior ring (a courtyard) while the stored <see cref="BuildingModel"/> reports no internal edge on its <c>Footprints</c>. That is the model that renders a floor over the courtyard and drives the terrain cut DiGi.GIS.WebAPI.UI#45 reported.</para>
    /// <para>Nothing is uploaded and nothing is repaired - the task exists to name the affected set and tally it per county, which is the scope the regeneration (DiGi.GIS.PostgreSQL.UI#13) must run against. A county's tally is recorded only once every reference of it has been audited, so a county interrupted part way is not checkpointed and is redone in full on the next run rather than left half-counted.</para>
    /// <para>Two files are written into <see cref="ReportDirectory"/>: one row per reference in <c>BuildingModels_CourtyardAudit.csv</c>, and per-county plus national totals in <c>BuildingModels_CourtyardAudit_Summary.txt</c>, the latter carrying an <c>AffectedCountyIds:</c> line that is the ready-to-paste <see cref="CountyIds"/> scope for the regeneration.</para>
    /// </summary>
    public class UIBuildingModelsCourtyardAuditTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private readonly GISWebAPIManager gISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIBuildingModelsCourtyardAuditTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the server.</param>
        public UIBuildingModelsCourtyardAuditTask(GISWebAPIManager gISWebAPIManager)
        {
            this.gISWebAPIManager = gISWebAPIManager;
        }

        /// <summary>
        /// Gets or sets the number of source-bearing references asked for in a single stored-model request. Those references travel in the query string, so a batch far above this risks the URL length limit of the server.
        /// </summary>
        public int BatchSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets the identifiers of the counties to be audited. When null every county held on the server is audited.
        /// </summary>
        public IEnumerable<int>? CountyIds { get; set; } = null;

        /// <summary>
        /// Gets or sets how many source-building requests are allowed to be in flight at once. The source pull is one request per building, so an unbounded national pass would take weeks when they are issued one after another; the requests are independent, so they go out in groups of this size.
        /// </summary>
        public int MaxConcurrentRequests { get; set; } = 8;

        /// <summary>
        /// Gets or sets the maximum number of references audited per county. A value of zero or less audits every reference of the county - the estate-wide behaviour. Capping a run keeps a sanity-check bounded without changing that default.
        /// </summary>
        public int MaxReferencesPerCounty { get; set; } = 0;

        /// <summary>
        /// Gets or sets the directory the two report files are written into. When null the user is asked for one.
        /// </summary>
        public string? ReportDirectory { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether counties named in the checkpoint of an earlier run are skipped. Defaults to <see langword="true"/>. A county is checkpointed only once every reference of it has been audited, so turning this off re-audits every county in scope from the first.
        /// </summary>
        public bool Resume { get; set; } = true;

        /// <summary>
        /// Gets or sets the distance tolerance the <c>Footprints</c> of a stored model are required to hold their internal edges at.
        /// </summary>
        public double Tolerance { get; set; } = DiGi.Core.Constants.Tolerance.Distance;

        /// <summary>
        /// Gets or sets the two-digit voivodeship codes to be audited. A county is in scope when its code starts with one of them. When null every voivodeship is audited. Combined with <see cref="CountyIds"/> both filters have to admit the county.
        /// </summary>
        public IEnumerable<string>? VoivodeshipCodes { get; set; } = null;

        /// <summary>
        /// Decides whether the source CityGML building carries a courtyard - one of its <see cref="CityGML.Classes.GroundSurface"/> surfaces holds an interior ring. This is the source half of the affected test; a wall or roof surface never counts, so only ground surfaces are examined.
        /// </summary>
        /// <param name="source">The stored CityGML building, or null when the server holds none.</param>
        /// <returns><see langword="true"/> when a ground surface of the building has at least one interior ring.</returns>
        public static bool SourceHasRing(CityGML.Classes.Building? source)
        {
            if (source is null || source.Surfaces is not IEnumerable<CityGML.Interfaces.ISurface> surfaces)
            {
                return false;
            }

            foreach (CityGML.Interfaces.ISurface surface in surfaces)
            {
                if (surface is CityGML.Classes.GroundSurface groundSurface && groundSurface.Geometry?.InternalEdges is { Count: > 0 })
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Decides whether the stored model's outline carries a courtyard - one of its <c>Footprints</c> faces holds an internal edge. This is the stored half of the affected test: a model the storey split filled solid has none.
        /// </summary>
        /// <param name="stored">The stored building model, or null when the server holds none.</param>
        /// <param name="tolerance">The distance tolerance the footprints are required to hold their internal edges at.</param>
        /// <returns><see langword="true"/> when at least one footprint face carries an internal edge.</returns>
        public static bool StoredHasHole(DiGi.Analytical.Building.Classes.BuildingModel? stored, double tolerance)
        {
            if (stored is null)
            {
                return false;
            }

            List<PolygonalFace2D>? footprints = stored.Footprints(tolerance);
            return footprints is not null && footprints.Exists(polygonalFace2D => polygonalFace2D.InternalEdges is { Count: > 0 });
        }

        /// <summary>
        /// Combines the two halves: the model is affected when the source carries a courtyard, a stored model exists, and that stored model's outline has none. A source courtyard with no stored model is a missing model, not an affected one - the regional view falls back to the 2D footprint, which keeps the courtyard.
        /// </summary>
        /// <param name="source">The stored CityGML building, or null when the server holds none.</param>
        /// <param name="stored">The stored building model, or null when the server holds none.</param>
        /// <param name="tolerance">The distance tolerance the footprints are required to hold their internal edges at.</param>
        /// <returns><see langword="true"/> when the source has a courtyard the stored model lost.</returns>
        public static bool IsAffected(CityGML.Classes.Building? source, DiGi.Analytical.Building.Classes.BuildingModel? stored, double tolerance)
        {
            return SourceHasRing(source) && stored is not null && !StoredHasHole(stored, tolerance);
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            string? directory = ReportDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                OpenFolderDialog openFolderDialog = new();
                bool? dialogResult = openFolderDialog.ShowDialog();
                if (dialogResult is null || !dialogResult.Value)
                {
                    return false;
                }

                directory = openFolderDialog.FolderName;
            }

            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Report directory does not exist");
                return false;
            }

            HttpClient? httpClient_AdministrativeAreal2D = gISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path_AdministrativeAreal2D);
            if (httpClient_AdministrativeAreal2D is null || string.IsNullOrWhiteSpace(path_AdministrativeAreal2D))
            {
                return false;
            }

            HttpClient? httpClient_References = gISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetReferencesByCountyIdAsync), out string? path_References);
            if (httpClient_References is null || string.IsNullOrWhiteSpace(path_References))
            {
                return false;
            }

            HttpClient? httpClient_Building = gISWebAPIManager.CreateHttpClient<BuildingController>(nameof(BuildingController.GetItemByReferenceAsync), out string? path_Building);
            if (httpClient_Building is null || string.IsNullOrWhiteSpace(path_Building))
            {
                return false;
            }

            HttpClient? httpClient_BuildingModel = gISWebAPIManager.CreateHttpClient<BuildingModelController>(nameof(BuildingModelController.GetItemsByReferencesAsync), out string? path_BuildingModel);
            if (httpClient_BuildingModel is null || string.IsNullOrWhiteSpace(path_BuildingModel))
            {
                return false;
            }

            PostOptions postOptions = new() { RequestResult = true };

            // The endpoint is a HttpGet action and its administrativearealtype parameter is not nullable - omitting it binds to Country, not County.
            string requestUri_AdministrativeAreal2D = new UrlBuilder(path_AdministrativeAreal2D).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).ToString();

            PostResponse<List<AdministrativeAreal2DReference>?> postResponse_AdministrativeAreal2DReferences = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient_AdministrativeAreal2D, requestUri_AdministrativeAreal2D, postOptions);
            if (postResponse_AdministrativeAreal2DReferences is null || !postResponse_AdministrativeAreal2DReferences.Succeeded || postResponse_AdministrativeAreal2DReferences.Result is not List<AdministrativeAreal2DReference> administrativeAreal2DReferences)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County references could not be retrieved");
                return false;
            }

            HashSet<int>? countyIds = null;
            if (CountyIds is not null)
            {
                countyIds = [.. CountyIds];
                if (countyIds.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CountyIds is empty - nothing to audit");
                    return false;
                }
            }

            HashSet<string>? voivodeshipCodes = null;
            if (VoivodeshipCodes is not null)
            {
                voivodeshipCodes = [.. VoivodeshipCodes];
                if (voivodeshipCodes.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "VoivodeshipCodes is empty - nothing to audit");
                    return false;
                }
            }

            int batchSize = BatchSize < 1 ? 1 : BatchSize;
            int maxConcurrentRequests = MaxConcurrentRequests < 1 ? 1 : MaxConcurrentRequests;

            DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper = progress is null ? null : new(progress);

            string path_Checkpoint = System.IO.Path.Combine(directory, "BuildingModels_CourtyardAudit_Checkpoint.txt");

            // Read before the writer is opened - a run with Resume off truncates the file rather than skipping the counties an earlier, differently scoped run recorded.
            HashSet<int> countyIds_Completed = [];
            if (Resume && File.Exists(path_Checkpoint))
            {
                foreach (string line in File.ReadAllLines(path_Checkpoint))
                {
                    if (int.TryParse(line.Trim(), out int countyId_Completed))
                    {
                        countyIds_Completed.Add(countyId_Completed);
                    }
                }
            }

            using StreamWriter streamWriter_Checkpoint = new(path_Checkpoint, Resume, Encoding.UTF8);

            List<string> summaryLines =
            [
                "=== BUILDING MODEL COURTYARD AUDIT ===",
                $"Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"Footprints tolerance: {Tolerance.ToString(System.Globalization.CultureInfo.InvariantCulture)}",
                "Classification: Affected = source courtyard present, stored model exists, stored Footprints carry no internal edge",
                string.Empty,
                "Code;CountyId;Total;SourceHasRing;Affected;Ok;Missing;NoCourtyard",
            ];

            long count_Total = 0;
            long count_SourceHasRing = 0;
            long count_Affected = 0;
            long count_Ok = 0;
            long count_Missing = 0;
            long count_NoCourtyard = 0;
            List<int> countyIds_Affected = [];

            using StreamWriter streamWriter = new(System.IO.Path.Combine(directory, "BuildingModels_CourtyardAudit.csv"), false, Encoding.UTF8);
            await streamWriter.WriteLineAsync("Code;CountyId;Reference;SourceHasRing;StoredModelPresent;StoredHasHole;Classification");

            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Id identifies the county itself - CountryId/VoivodeshipId/CountyId are the parent chain, and a code covers every polygon part of a multi-part county.
                int countyId = administrativeAreal2DReference.Id;
                string code = administrativeAreal2DReference.Code ?? string.Empty;

                if (!PostgreSQL.Query.IsInScope(countyId, code, countyIds, voivodeshipCodes))
                {
                    continue;
                }

                if (countyIds_Completed.Contains(countyId))
                {
                    Serilog.Modify.Log("County {Code} (id {CountyId}) was audited by an earlier run - skipped", code, countyId);
                    continue;
                }

                string requestUri_References = new UrlBuilder(path_References).AddParameter("countyid", countyId).ToString();

                List<string>? references = null;
                try
                {
                    PostResponse<List<string>?> postResponse_References = await DiGi.WebAPI.Query.GetAsync<List<string>>(httpClient_References, requestUri_References, postOptions);
                    references = postResponse_References is not null && postResponse_References.Succeeded ? postResponse_References.Result : null;
                }
                catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                {
                    Serilog.Modify.Log(exception, "Building2D references request failed for county {CountyId}", countyId);
                }

                if (references is null || references.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No Building2D references for county {Code} (id {CountyId}) - nothing to audit", code, countyId);
                    continue;
                }

                if (MaxReferencesPerCounty > 0 && references.Count > MaxReferencesPerCounty)
                {
                    references = references.GetRange(0, MaxReferencesPerCounty);
                    Serilog.Modify.Log("County {Code} (id {CountyId}) capped to {Cap} references for a bounded run", code, countyId, MaxReferencesPerCounty);
                }

                Serilog.Modify.Log("County {Code} (id {CountyId}) audit started. References: {Total}", code, countyId, references.Count);

                int count_Total_County = 0;
                int count_SourceHasRing_County = 0;
                int count_Affected_County = 0;
                int count_Ok_County = 0;
                int count_Missing_County = 0;
                int count_NoCourtyard_County = 0;

                for (int i = 0; i < references.Count; i += batchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    List<string> references_Batch = references.GetRange(i, Math.Min(batchSize, references.Count - i));

                    // The source pull is one request per building and independent, so it goes out in bounded
                    // groups. A 204 (no stored CityGML building) is a success with a null result.
                    CityGML.Classes.Building?[] sources = new CityGML.Classes.Building?[references_Batch.Count];
                    for (int s = 0; s < references_Batch.Count; s += maxConcurrentRequests)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        List<Task> tasks = [];
                        for (int t = s; t < Math.Min(s + maxConcurrentRequests, references_Batch.Count); t++)
                        {
                            int index = t;
                            string requestUri_Building = new UrlBuilder(path_Building).AddParameter("reference", references_Batch[index]).AddParameter("countyid", countyId).ToString();

                            tasks.Add(Task.Run(async () =>
                            {
                                try
                                {
                                    PostResponse<CityGML.Classes.Building?> postResponse_Building = await DiGi.WebAPI.Query.GetAsync<CityGML.Classes.Building>(httpClient_Building, requestUri_Building, postOptions);
                                    sources[index] = postResponse_Building is not null && postResponse_Building.Succeeded ? postResponse_Building.Result : null;
                                }
                                catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                                {
                                    Serilog.Modify.Log(exception, "Building request failed for reference {Reference} in county {CountyId}", references_Batch[index], countyId);
                                }
                            }, cancellationToken));
                        }

                        await Task.WhenAll(tasks);
                    }

                    // Only the source-bearing references can be affected, so only those need the stored model
                    // pulled to test the hole. Everything else is classified from the source alone.
                    List<string> references_SourceHasRing = [];
                    for (int u = 0; u < references_Batch.Count; u++)
                    {
                        if (SourceHasRing(sources[u]))
                        {
                            references_SourceHasRing.Add(references_Batch[u]);
                        }
                    }

                    Dictionary<string, bool> storedHasHole_ByReference = [];
                    if (references_SourceHasRing.Count != 0)
                    {
                        // References repeat in the query string and UrlBuilder holds one value per name, so this part of the query is written directly rather than through it.
                        StringBuilder stringBuilder = new(path_BuildingModel);
                        stringBuilder.Append("?countyid=").Append(countyId);
                        foreach (string reference in references_SourceHasRing)
                        {
                            stringBuilder.Append("&references=").Append(WebUtility.UrlEncode(reference));
                        }

                        List<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels = null;
                        try
                        {
                            PostResponse<List<DiGi.Analytical.Building.Classes.BuildingModel>?> postResponse_BuildingModels = await DiGi.WebAPI.Query.GetAsync<List<DiGi.Analytical.Building.Classes.BuildingModel>>(httpClient_BuildingModel, stringBuilder.ToString(), postOptions);
                            buildingModels = postResponse_BuildingModels is not null && postResponse_BuildingModels.Succeeded ? postResponse_BuildingModels.Result : null;
                        }
                        catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                        {
                            Serilog.Modify.Log(exception, "BuildingModels request failed for county {CountyId}", countyId);
                        }

                        foreach (DiGi.Analytical.Building.Classes.BuildingModel buildingModel in buildingModels ?? [])
                        {
                            if (buildingModel.TryGetValue(BuildingModelParameter.Reference, out string? reference) && !string.IsNullOrWhiteSpace(reference))
                            {
                                storedHasHole_ByReference[reference] = StoredHasHole(buildingModel, Tolerance);
                            }
                        }
                    }

                    for (int v = 0; v < references_Batch.Count; v++)
                    {
                        string reference = references_Batch[v];
                        count_Total_County++;

                        bool sourceHasRing = SourceHasRing(sources[v]);
                        if (!sourceHasRing)
                        {
                            count_NoCourtyard_County++;
                            await streamWriter.WriteLineAsync($"{code};{countyId};{reference};no;;;NoCourtyard");
                            continue;
                        }

                        count_SourceHasRing_County++;

                        if (!storedHasHole_ByReference.TryGetValue(reference, out bool storedHasHole))
                        {
                            count_Missing_County++;
                            await streamWriter.WriteLineAsync($"{code};{countyId};{reference};yes;no;;Missing");
                            continue;
                        }

                        if (storedHasHole)
                        {
                            count_Ok_County++;
                            await streamWriter.WriteLineAsync($"{code};{countyId};{reference};yes;yes;yes;Ok");
                        }
                        else
                        {
                            count_Affected_County++;
                            await streamWriter.WriteLineAsync($"{code};{countyId};{reference};yes;yes;no;Affected");
                        }
                    }

                    longProgressWrapper?.Increment(references_Batch.Count);
                }

                await streamWriter.FlushAsync(cancellationToken);

                count_Total += count_Total_County;
                count_SourceHasRing += count_SourceHasRing_County;
                count_Affected += count_Affected_County;
                count_Ok += count_Ok_County;
                count_Missing += count_Missing_County;
                count_NoCourtyard += count_NoCourtyard_County;

                if (count_Affected_County != 0)
                {
                    countyIds_Affected.Add(countyId);
                }

                Serilog.Modify.Log("County {Code} (id {CountyId}) audited. Affected: {Affected}/{Total}, ok: {Ok}, missing: {Missing}, no courtyard: {NoCourtyard}", code, countyId, count_Affected_County, count_Total_County, count_Ok_County, count_Missing_County, count_NoCourtyard_County);

                summaryLines.Add($"{code};{countyId};{count_Total_County};{count_SourceHasRing_County};{count_Affected_County};{count_Ok_County};{count_Missing_County};{count_NoCourtyard_County}");

                // Recorded only once every reference of the county has been audited. A county interrupted part way is therefore not checkpointed and is redone in full on the next run.
                await streamWriter_Checkpoint.WriteLineAsync(countyId.ToString());
                await streamWriter_Checkpoint.FlushAsync(cancellationToken);

                Serilog.Modify.Log("County {Code} (id {CountyId}) audited and checkpointed", code, countyId);
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== TOTALS ===");
            summaryLines.Add($"Audited references: {count_Total}");
            summaryLines.Add($"Source carries a courtyard: {count_SourceHasRing}");
            summaryLines.Add($"Affected (source courtyard, stored model filled it): {count_Affected}");
            summaryLines.Add($"Ok (source courtyard, stored model keeps it): {count_Ok}");
            summaryLines.Add($"Missing (source courtyard, no stored model): {count_Missing}");
            summaryLines.Add($"No courtyard in source: {count_NoCourtyard}");
            summaryLines.Add(string.Empty);
            summaryLines.Add($"AffectedCountyIds: {(countyIds_Affected.Count == 0 ? "none" : string.Join(", ", countyIds_Affected))}");
            summaryLines.Add(string.Empty);
            summaryLines.Add($"Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            await File.WriteAllLinesAsync(System.IO.Path.Combine(directory, "BuildingModels_CourtyardAudit_Summary.txt"), summaryLines, cancellationToken);

            Serilog.Modify.Log("Audit ended. Affected: {Affected}/{Total} across {Counties} counties", count_Affected, count_Total, countyIds_Affected.Count);

            return true;
        }
    }
}
