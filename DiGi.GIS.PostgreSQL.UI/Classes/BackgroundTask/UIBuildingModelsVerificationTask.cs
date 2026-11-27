using DiGi.Core.Classes;
using DiGi.GIS.Analytical;
using DiGi.GIS.Analytical.Enums;
using DiGi.GIS.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A UI-driven task that reads the <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> records already stored on the server and reports how complete and how sound they are.
    /// <para>Read-only. Nothing is uploaded, nothing is repaired - the task exists to say what the stored data looks like, which is the state the upload path itself never reported: a model whose spaces are not enclosed is accepted by the server today and stored without a word.</para>
    /// <para>For every county in scope a sample of <see cref="SampleSize"/> 2D building references is drawn with <see cref="RandomSeed"/>, so a run is reproducible and two runs can be compared. The models behind those references are pulled in batches and each one is passed through <c>Analytical.Create.BuildingModelValidationResult</c>. A reference the server holds no model for is recorded as missing, which is the completeness half of the answer.</para>
        /// <para>The seed is combined with the county row identifier rather than shared across counties, so a county draws the same sample whether it is verified on its own, with its voivodeship, or nationally. A single generator advanced across counties made every county's draw depend on how many references each preceding county held, which the 2026-08-14 county part repair changed - and with it the sample of every county after the repaired three.</para>
    /// <para>Two files are written into <see cref="ReportDirectory"/>: one row per reference in <c>BuildingModels_Verification.csv</c>, and per county plus national totals in <c>BuildingModels_Verification_Summary.txt</c>. The row file is flushed county by county, so a run interrupted late still leaves everything it had already measured.</para>
    /// <para>The CSV carries an <c>UnlocatedBuildingInformation</c> column, true for a model whose <c>BuildingInformation</c> carries no located coordinates and defined UTC offset - the state the <see cref="UIPostgreSQLBuildingModelBuildingInformationUpdateTask"/> exists to remove, so the column is 0 after the backfill and on freshly regenerated models. It is a separate column rather than a <see cref="BuildingModelValidationCode"/>, deliberately: a code would make every unstamped model invalid and break comparison with the 2026-08-11 enclosure baseline.</para>
    /// <para>A 404 from a read is a valid empty outcome - a county part with no buildings, a batch with no stored model - but any other failed read is never reported as empty or missing (DiGi.GIS.PostgreSQL.UI#51): a county whose references could not be read, and the references of a batch whose models could not, are left out of the counts, listed under <c>FAILED</c> in the summary, and fail the run once the reports are written.</para>
    /// </summary>
    public class UIBuildingModelsVerificationTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private readonly GISWebAPIManager gISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIBuildingModelsVerificationTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the server.</param>
        public UIBuildingModelsVerificationTask(GISWebAPIManager gISWebAPIManager)
        {
            this.gISWebAPIManager = gISWebAPIManager;
        }

        /// <summary>
        /// Gets or sets the number of references asked for in a single request. The references travel in the query string, so a batch far above this risks the URL length limit of the server.
        /// </summary>
        public int BatchSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets the identifiers of the counties to be processed. When null every county held on the server is processed.
        /// </summary>
        public IEnumerable<int>? CountyIds { get; set; }

        /// <summary>
        /// Gets or sets the seed of the sampling. Two runs sharing a seed draw the same references, which is what lets a run before a change be compared with one after it. The seed is combined per county by <see cref="PostgreSQL.Query.RandomSeed(int, int)"/>, so a county's draw does not depend on the scope of the run or on what any other county holds.
        /// </summary>
        public int RandomSeed { get; set; }

        /// <summary>
        /// Gets or sets the directory the two report files are written into. When null the user is asked for one.
        /// </summary>
        public string? ReportDirectory { get; set; }

        /// <summary>
        /// Gets or sets the number of references drawn per county. A value of zero or less takes every reference of the county.
        /// </summary>
        public int SampleSize { get; set; } = 200;

        /// <summary>
        /// Gets or sets the distance tolerance the enclosure of a space is required to hold at.
        /// </summary>
        public double Tolerance { get; set; } = Analytical.Constants.Tolerance.Enclosure;

        /// <summary>
        /// Gets or sets the two-digit voivodeship codes to be processed. A county is in scope when its code starts with one of them. When null every voivodeship is processed. Combined with <see cref="CountyIds"/> both filters have to admit the county.
        /// </summary>
        public IEnumerable<string>? VoivodeshipCodes { get; set; }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            string? directory = ReportDirectory;
            if (string.IsNullOrWhiteSpace(directory))
            {
                OpenFolderDialog openFolderDialog = new();
                bool? dialogResult = openFolderDialog.ShowDialog();
                if (dialogResult is null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    CancelRun();
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

            HttpClient? httpClient_BuildingModel = gISWebAPIManager.CreateHttpClient<BuildingModelController>(nameof(BuildingModelController.GetItemsByReferencesAsync), out string? path_BuildingModel);
            if (httpClient_BuildingModel is null || string.IsNullOrWhiteSpace(path_BuildingModel))
            {
                return false;
            }

            PostOptions postOptions = new() { RequestResult = true };

            // The per-county reads keep the bound GetAsync gave them, but go through the status-aware read so a 404 stays empty and a failure stays a failure (DiGi.GIS.PostgreSQL.UI#51).
            int requestTimeout = (int)postOptions.Delay.TotalSeconds;

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
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CountyIds is empty - nothing to verify");
                    return false;
                }
            }

            HashSet<string>? voivodeshipCodes = null;
            if (VoivodeshipCodes is not null)
            {
                voivodeshipCodes = [.. VoivodeshipCodes];
                if (voivodeshipCodes.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "VoivodeshipCodes is empty - nothing to verify");
                    return false;
                }
            }

            int batchSize = BatchSize < 1 ? 1 : BatchSize;

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            List<string> summaryLines =
            [
                "=== BUILDING MODEL VERIFICATION ===",
                $"Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"Enclosure tolerance: {Tolerance.ToString(System.Globalization.CultureInfo.InvariantCulture)}",
                $"Sample size per county: {(SampleSize < 1 ? "all" : SampleSize.ToString(CultureInfo.CurrentCulture))}",
                $"Random seed: {RandomSeed} (combined with the county row id, so a county's draw is the same at any scope)",
                string.Empty,
                "Code;CountyId;Requested;Missing;Valid;Invalid;NotEnclosed;SeaLevel;SpacePointOutsideShell;UnlocatedBuildingInformation",
            ];

            Dictionary<BuildingModelValidationCode, int> counts_ByValidationCode = [];
            Dictionary<double, int> counts_ByMinEnclosingTolerance = [];

            long count_Shell = 0;
            long count_Shell_Enclosed = 0;

            long count_Requested = 0;
            long count_Missing = 0;
            long count_Valid = 0;
            long count_Invalid = 0;
            long count_Unlocated = 0;
            long count_Unread = 0;

            // One row per failed read - a county whose references could not be read, or a batch whose models could not - so a failure is listed, never written into the report as empty or missing.
            List<string> lines_Failed = [];

            using StreamWriter streamWriter = new(System.IO.Path.Combine(directory, "BuildingModels_Verification.csv"), false, Encoding.UTF8);
            await streamWriter.WriteLineAsync("Code;CountyId;Reference;Status;SpaceCount;ComponentCount;ShellCount;EnclosedShellCount;MinEnclosingTolerance;MinZ;MaxZ;UnlocatedBuildingInformation;ValidationCodes");

            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Id identifies the county itself - CountryId/VoivodeshipId/CountyId are the parent chain.
                int countyId = administrativeAreal2DReference.Id;

                string code = administrativeAreal2DReference.Code ?? string.Empty;

                if (!PostgreSQL.Query.IsInScope(countyId, code, countyIds, voivodeshipCodes))
                {
                    continue;
                }

                string requestUri_References = new UrlBuilder(path_References).AddParameter("countyid", countyId).ToString();

                (bool succeeded_References, string? json_References) = await Query.JsonResponseAsync(httpClient_References, requestUri_References, requestTimeout, cancellationToken);
                List<string>? references = succeeded_References ? Query.References(json_References) : null;
                if (references is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: Building2D references of county {Code} (id {CountyId}) could not be read - the county is not verified", nameof(UIBuildingModelsVerificationTask), code, countyId);
                    lines_Failed.Add($"{code};{countyId};References;");
                    continue;
                }

                // A 404 is a county part with no buildings (DiGi.GIS.WebAPI#97) - a valid, empty outcome.
                if (references.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "No Building2D references for county {Code} (id {CountyId})", code, countyId);
                    summaryLines.Add($"{code};{countyId};0;0;0;0;0;0;0;0");
                    continue;
                }

                // Seeded per county rather than once for the run, so this county's draw does not depend on
                // how many references the counties before it held - which is what made a scoped run and a
                // national run disagree about which references they sampled.
                Random random = new(PostgreSQL.Query.RandomSeed(RandomSeed, countyId));

                List<string> references_Sample = references.Sample(SampleSize, random) ?? [];

                Serilog.Modify.Log("County {Code} (id {CountyId}) verification started. References: {Sampled}/{Total}", code, countyId, references_Sample.Count, references.Count);

                int count_Requested_County = 0;
                int count_Missing_County = 0;
                int count_Valid_County = 0;
                int count_Invalid_County = 0;
                int count_NotEnclosed_County = 0;
                int count_SeaLevel_County = 0;
                int count_SpacePointOutsideShell_County = 0;
                int count_Unlocated_County = 0;

                for (int i = 0; i < references_Sample.Count; i += batchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    List<string> references_Batch = references_Sample.GetRange(i, Math.Min(batchSize, references_Sample.Count - i));

                    // References repeat in the query string and UrlBuilder holds one value per name, so this
                    // part of the query is written directly rather than through it.
                    StringBuilder stringBuilder = new(path_BuildingModel);
                    stringBuilder.Append("?countyid=").Append(countyId);
                    foreach (string reference in references_Batch)
                    {
                        stringBuilder.Append("&references=").Append(WebUtility.UrlEncode(reference));
                    }

                    // A batch matching nothing answers 404, which is a valid outcome here and leaves every
                    // reference of the batch to be recorded as missing. A failed read records none of them:
                    // the batch is counted as unread and listed, never reported as missing models.
                    (bool succeeded_BuildingModels, string? json_BuildingModels) = await Query.JsonResponseAsync(httpClient_BuildingModel, stringBuilder.ToString(), requestTimeout, cancellationToken);
                    List<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels = !succeeded_BuildingModels ? null : json_BuildingModels is null ? [] : Core.Convert.ToDiGi<DiGi.Analytical.Building.Classes.BuildingModel>(json_BuildingModels);
                    if (buildingModels is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: BuildingModels of {Count} references of county {Code} (id {CountyId}) could not be read - the batch is not verified", nameof(UIBuildingModelsVerificationTask), references_Batch.Count, code, countyId);
                        count_Unread += references_Batch.Count;
                        lines_Failed.Add($"{code};{countyId};BuildingModels;{references_Batch.Count}");
                        longProgressWrapper?.Increment(references_Batch.Count);
                        continue;
                    }

                    Dictionary<string, BuildingModelValidationResult> buildingModelValidationResults_ByReference = [];
                    Dictionary<string, bool> unlocated_ByReference = [];

                    foreach (DiGi.Analytical.Building.Classes.BuildingModel buildingModel in buildingModels)
                    {
                        BuildingModelValidationResult? buildingModelValidationResult = Analytical.Create.BuildingModelValidationResult(buildingModel, Tolerance);
                        if (buildingModelValidationResult is null || string.IsNullOrWhiteSpace(buildingModelValidationResult.Reference))
                        {
                            // Rows are keyed by the reference the model carries, so one without it cannot be
                            // matched to the reference it was asked for and would otherwise be counted as a
                            // reference the database holds nothing for. The two are not the same failure.
                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingModel returned by county {CountyId} carries no reference and cannot be matched to the reference it was requested by", countyId);
                            continue;
                        }

                        buildingModelValidationResults_ByReference[buildingModelValidationResult.Reference!] = buildingModelValidationResult;

                        // The location is a property of the stored model, not of its enclosure, so it is
                        // read here beside the validation and kept out of BuildingModelValidationCode.
                        unlocated_ByReference[buildingModelValidationResult.Reference!] = !buildingModel.IsLocated();
                    }

                    foreach (string reference in references_Batch)
                    {
                        count_Requested_County++;

                        if (!buildingModelValidationResults_ByReference.TryGetValue(reference, out BuildingModelValidationResult? buildingModelValidationResult))
                        {
                            count_Missing_County++;
                            await streamWriter.WriteLineAsync($"{code};{countyId};{reference};Missing;;;;;;;;;");
                            continue;
                        }

                        List<BuildingModelValidationCode>? buildingModelValidationCodes = buildingModelValidationResult.ValidationCodes;

                        if (buildingModelValidationResult.IsValid)
                        {
                            count_Valid_County++;
                        }
                        else
                        {
                            count_Invalid_County++;
                        }

                        foreach (BuildingModelValidationCode buildingModelValidationCode in buildingModelValidationCodes ?? [])
                        {
                            counts_ByValidationCode.TryGetValue(buildingModelValidationCode, out int count);
                            counts_ByValidationCode[buildingModelValidationCode] = count + 1;

                            if (buildingModelValidationCode == BuildingModelValidationCode.NotEnclosed)
                            {
                                count_NotEnclosed_County++;
                            }
                            else if (buildingModelValidationCode == BuildingModelValidationCode.SeaLevel)
                            {
                                count_SeaLevel_County++;
                            }
                            else if (buildingModelValidationCode == BuildingModelValidationCode.SpacePointOutsideShell)
                            {
                                count_SpacePointOutsideShell_County++;
                            }
                        }

                        // The acceptance figure is "enclosed at or below the tolerance", which was previously
                        // recomputed from the row file by hand. The ingredients are already here.
                        count_Shell += buildingModelValidationResult.ShellCount;
                        count_Shell_Enclosed += buildingModelValidationResult.EnclosedShellCount;

                        double minEnclosingTolerance = buildingModelValidationResult.MinEnclosingTolerance;
                        counts_ByMinEnclosingTolerance.TryGetValue(minEnclosingTolerance, out int count_Tolerance);
                        counts_ByMinEnclosingTolerance[minEnclosingTolerance] = count_Tolerance + 1;

                        bool unlocated = unlocated_ByReference[reference];
                        if (unlocated)
                        {
                            count_Unlocated_County++;
                        }

                        await streamWriter.WriteLineAsync($"{code};{countyId};{reference};{(buildingModelValidationResult.IsValid ? "Valid" : "Invalid")};{buildingModelValidationResult.SpaceCount};{buildingModelValidationResult.ComponentCount};{buildingModelValidationResult.ShellCount};{buildingModelValidationResult.EnclosedShellCount};{Text(minEnclosingTolerance)};{Text(buildingModelValidationResult.MinZ)};{Text(buildingModelValidationResult.MaxZ)};{(unlocated ? "true" : "false")};{string.Join(' ', buildingModelValidationCodes ?? [])}");
                    }

                    longProgressWrapper?.Increment(references_Batch.Count);
                }

                await streamWriter.FlushAsync(cancellationToken);

                count_Requested += count_Requested_County;
                count_Missing += count_Missing_County;
                count_Valid += count_Valid_County;
                count_Invalid += count_Invalid_County;
                count_Unlocated += count_Unlocated_County;

                Serilog.Modify.Log("County {Code} (id {CountyId}) verified. Valid: {Valid}/{Requested}, missing: {Missing}, not enclosed: {NotEnclosed}", code, countyId, count_Valid_County, count_Requested_County, count_Missing_County, count_NotEnclosed_County);

                summaryLines.Add($"{code};{countyId};{count_Requested_County};{count_Missing_County};{count_Valid_County};{count_Invalid_County};{count_NotEnclosed_County};{count_SeaLevel_County};{count_SpacePointOutsideShell_County};{count_Unlocated_County}");
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== TOTALS ===");
            summaryLines.Add($"Requested references: {count_Requested}");
            summaryLines.Add($"Missing models: {count_Missing}");
            summaryLines.Add($"Valid models: {count_Valid}");
            summaryLines.Add($"Invalid models: {count_Invalid}");
            summaryLines.Add($"Unlocated BuildingInformation: {count_Unlocated}");
            summaryLines.Add($"Unread references (failed model reads): {count_Unread}");
            summaryLines.Add($"Failed reads: {lines_Failed.Count}");
            summaryLines.Add($"Enclosed shells: {count_Shell_Enclosed}/{count_Shell}{(count_Shell == 0 ? string.Empty : $" ({(100.0 * count_Shell_Enclosed / count_Shell).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)} %)")}");

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== VALIDATION CODES ===");
            foreach (BuildingModelValidationCode buildingModelValidationCode in Enum.GetValues<BuildingModelValidationCode>())
            {
                counts_ByValidationCode.TryGetValue(buildingModelValidationCode, out int count);
                summaryLines.Add($"{buildingModelValidationCode};{count}");
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add("=== SMALLEST TOLERANCE CLOSING THE WHOLE MODEL ===");
            List<double> minEnclosingTolerances = [.. counts_ByMinEnclosingTolerance.Keys];
            minEnclosingTolerances.Sort();
            foreach (double minEnclosingTolerance in minEnclosingTolerances)
            {
                summaryLines.Add($"{Text(minEnclosingTolerance)};{counts_ByMinEnclosingTolerance[minEnclosingTolerance]}");
            }

            if (lines_Failed.Count != 0)
            {
                summaryLines.Add(string.Empty);
                summaryLines.Add("=== FAILED ===");
                summaryLines.Add("Code;CountyId;Stage;References");
                summaryLines.AddRange(lines_Failed);
            }

            summaryLines.Add(string.Empty);
            summaryLines.Add($"Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            await File.WriteAllLinesAsync(System.IO.Path.Combine(directory, "BuildingModels_Verification_Summary.txt"), summaryLines, cancellationToken);

            Serilog.Modify.Log("Verification ended. Valid: {Valid}/{Requested}, invalid: {Invalid}, missing: {Missing}", count_Valid, count_Requested, count_Invalid, count_Missing);

            if (lines_Failed.Count != 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {Count} reads failed ({Unread} references unread) - the report lists them under FAILED and is incomplete", nameof(UIBuildingModelsVerificationTask), lines_Failed.Count, count_Unread);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Formats a value for the report, writing an empty cell rather than the word for not a number.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted value, or an empty string when it is not a number.</returns>
        private static string Text(double value)
        {
            return double.IsNaN(value) ? string.Empty : value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
