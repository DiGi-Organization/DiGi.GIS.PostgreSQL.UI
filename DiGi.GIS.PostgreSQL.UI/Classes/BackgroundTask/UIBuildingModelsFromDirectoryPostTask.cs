using DiGi.CityGML.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A UI-driven post task that prompts the user to select a directory containing CityGML archives, enumerates the counties held on the server, and for every county whose code matches a CityGML archive in that directory downloads the county's <see cref="GIS.Classes.Building2D"/> data page by page, generates <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> instances and uploads them.
    /// <para>Both the CityGML archives and the Building2D data are processed one county - and within a county one page - at a time, so neither the whole country's CityGML nor a whole county's buildings are ever held in memory at once.</para>
    /// </summary>
    public class UIBuildingModelsFromDirectoryPostTask : BuildingModelsPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIBuildingModelsFromDirectoryPostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the server.</param>
        public UIBuildingModelsFromDirectoryPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the number of <see cref="Building2DReference"/> items requested per page while downloading a county's buildings.
        /// </summary>
        public int PageSize { get; set; } = 250;

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            OpenFolderDialog openFolderDialog = new();
            bool? dialogResult = openFolderDialog.ShowDialog();
            if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
            {
                return false;
            }

            string directory = openFolderDialog.FolderName;
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return false;
            }

            HttpClient? httpClient_AdministrativeAreal2D = gISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path_AdministrativeAreal2D);
            if (httpClient_AdministrativeAreal2D is null || string.IsNullOrWhiteSpace(path_AdministrativeAreal2D))
            {
                return false;
            }

            // The endpoint is a HttpGet action and its administrativearealtype parameter is not nullable - omitting it binds to Country, not County.
            string requestUri_AdministrativeAreal2D = new UrlBuilder(path_AdministrativeAreal2D).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).ToString();

            PostOptions postOptions = new() { RequestResult = true };

            PostResponse<List<AdministrativeAreal2DReference>?> postResponse_AdministrativeAreal2DReferences = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient_AdministrativeAreal2D, requestUri_AdministrativeAreal2D, postOptions);
            if (postResponse_AdministrativeAreal2DReferences is null || !postResponse_AdministrativeAreal2DReferences.Succeeded || postResponse_AdministrativeAreal2DReferences.Result is not List<AdministrativeAreal2DReference> administrativeAreal2DReferences)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County references could not be retrieved");
                return false;
            }

            HttpClient? httpClient_Building2DReferences = gISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetBuilding2DReferencesByPagingParameterAsync), out string? path_Building2DReferences);
            if (httpClient_Building2DReferences is null || string.IsNullOrWhiteSpace(path_Building2DReferences))
            {
                return false;
            }

            HttpClient? httpClient_Building2D = gISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetItemsByBuilding2DReferencesAsync), out string? path_Building2D);
            if (httpClient_Building2D is null || string.IsNullOrWhiteSpace(path_Building2D))
            {
                return false;
            }

            string requestUri_Building2DReferences = new UrlBuilder(path_Building2DReferences).ToString();
            string requestUri_Building2D = new UrlBuilder(path_Building2D).ToString();

            Core.Classes.LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            int pageSize = PageSize < 1 ? 1 : PageSize;

            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Id identifies the county itself - CountryId/VoivodeshipId/CountyId are the parent chain and are not the value building_2d.county_id holds.
                int countyId = administrativeAreal2DReference.Id;

                string? code = administrativeAreal2DReference.Code;
                if (countyId < 0 || string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }

                // Same matching rule as Analytical.Create.BuildingModels(GISModelFile, directory) - a county's CityGML lives in <code>.zip.
                string[] paths_CityGML = Directory.GetFiles(directory, string.Format("{0}.zip", code), SearchOption.AllDirectories);
                if (paths_CityGML.Length == 0)
                {
                    continue;
                }

                List<CityModel> cityModels = [];
                foreach (string path_CityGML in paths_CityGML)
                {
                    List<CityModel>? cityModels_Temp = CityGML.Create.CityModels(path_CityGML);
                    if (cityModels_Temp is null || cityModels_Temp.Count == 0)
                    {
                        continue;
                    }

                    cityModels.AddRange(cityModels_Temp);
                }

                if (cityModels.Count == 0)
                {
                    continue;
                }

                Serilog.Modify.Log("County {Code} (id {CountyId}): {Count} CityModels loaded", code, countyId, cityModels.Count);

                string? cursor = null;

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    List<Building2DReference>? building2DReferences = null;

                    Building2DReferencesByPagingParameter building2DReferencesByPagingParameter = new() { CountyId = countyId, PageSize = pageSize, Cursor = cursor };

                    try
                    {
                        // A factory rather than a single-use HttpContent - the body is rebuilt per attempt, so a dropped pooled
                        // connection is retried instead of failing the county (8 counties were lost to one reset on 2026-09-19).
                        // The content cannot be created only when the paging parameter fails to serialize; the exception is the
                        // caller's failure signal, logged by the catch below.
                        PostResponse<List<Building2DReference>?> postResponse_Building2DReferences = await DiGi.WebAPI.Modify.PostAsync<List<Building2DReference>>(httpClient_Building2DReferences, requestUri_Building2DReferences, async () => await WebAPI.Create.HttpContent(Core.Convert.ToSystem_String(building2DReferencesByPagingParameter) ?? string.Empty, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("Paging parameter content could not be created"), postOptions);
                        if (postResponse_Building2DReferences is null || !postResponse_Building2DReferences.Succeeded)
                        {
                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DReferences page could not be retrieved for county {CountyId}", countyId);
                            return false;
                        }

                        building2DReferences = postResponse_Building2DReferences.Result;
                    }
                    // A cancellation raised by the caller's token is left to propagate; anything else is a genuine request failure.
                    catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                    {
                        Serilog.Modify.Log(exception, "Building2DReferences page request failed for county {CountyId}", countyId);
                        return false;
                    }

                    if (building2DReferences is null || building2DReferences.Count == 0)
                    {
                        break;
                    }

                    List<GIS.Classes.Building2D>? building2Ds = null;

                    try
                    {
                        // Factory for the same reason as the paging request above.
                        PostResponse<List<GIS.Classes.Building2D>?> postResponse_Building2D = await DiGi.WebAPI.Modify.PostAsync<List<GIS.Classes.Building2D>>(httpClient_Building2D, requestUri_Building2D, async () => await WebAPI.Create.HttpContent(building2DReferences, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("Building2DReferences content could not be created"), postOptions);
                        if (postResponse_Building2D is null || !postResponse_Building2D.Succeeded)
                        {
                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2Ds could not be retrieved for county {CountyId}", countyId);
                            return false;
                        }

                        building2Ds = postResponse_Building2D.Result;
                    }
                    catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                    {
                        Serilog.Modify.Log(exception, "Building2Ds request failed for county {CountyId}", countyId);
                        return false;
                    }

                    if (building2Ds is not null && building2Ds.Count != 0)
                    {
                        List<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels = Analytical.Create.BuildingModels(building2Ds, cityModels);
                        if (buildingModels is not null && buildingModels.Count != 0)
                        {
                            if (!await ExecuteAsync(buildingModels, code, longProgressWrapper, cancellationToken))
                            {
                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingModels could not be uploaded for county {CountyId}", countyId);
                                return false;
                            }
                        }
                    }

                    if (building2DReferences.Count < pageSize)
                    {
                        break;
                    }

                    cursor = building2DReferences[^1].Reference;
                    if (string.IsNullOrWhiteSpace(cursor))
                    {
                        break;
                    }
                }
            }

            return true;
        }
    }
}
