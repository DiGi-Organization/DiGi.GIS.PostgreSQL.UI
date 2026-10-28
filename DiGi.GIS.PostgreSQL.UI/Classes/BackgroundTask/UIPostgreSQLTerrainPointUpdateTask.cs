using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.PostgreSQL.UI.Windows;
using DiGi.GIS.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// A terrain point run that is scoped from the user interface: the counties, the spacing of the sampling grid and whether points already stored are sampled again are asked for through <see cref="PostgreSQLTerrainPointUpdateOptionsWindow"/> each time the task is started, and only then is the run handed to <see cref="PostgreSQLTerrainPointUpdateTask"/>.
    /// <para>That is what the settings are worth asking for: a national pass at 50 m is about 125 million points and one request to the elevation service each, while the same task over a named county at 10 m is an afternoon. Neither is a default the other would tolerate.</para>
    /// </summary>
    public class UIPostgreSQLTerrainPointUpdateTask : PostgreSQLTerrainPointUpdateTask, IGISPostgreSQLUIObject
    {
        private readonly GISWebAPIManager? gISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIPostgreSQLTerrainPointUpdateTask"/> class.
        /// </summary>
        /// <param name="GISWebAPIManager">The manager the elevation service client is built from.</param>
        /// <param name="GISPostgreSQLConverterManager">The GIS PostgreSQL converter manager used to read the areas and write the points.</param>
        public UIPostgreSQLTerrainPointUpdateTask(GISWebAPIManager? GISWebAPIManager, GISPostgreSQLConverterManager? GISPostgreSQLConverterManager)
            : base(WebAPI.Create.HttpClient_GUGiK(GISWebAPIManager), GISPostgreSQLConverterManager)
        {
            gISWebAPIManager = GISWebAPIManager;
        }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // Asked before the dialog rather than after it: without a client the run cannot sample anything, and
            // the base task would answer that by returning false once the counties had already been chosen.
            if (httpClient is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Elevation service client could not be created - the terrain point run cannot start");
                return false;
            }

            // The dialog is a window, and this runs on a thread pool thread, where a window cannot be created at
            // all. Without an application there is no user interface thread to move it to.
            if (System.Windows.Application.Current is not System.Windows.Application application)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No WPF application is running - the terrain point options cannot be asked for");
                return false;
            }

            AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter = gISPostgreSQLConverterManager.GetPostgreSQLConverter<AdministrativeAreal2DPostgreSQLConverter>();
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return false;
            }

            // References rather than the identifiers GetIdsAsync returns: that method does list every county row
            // - all 406 polygon parts of the 380 codes, which is exactly what the run iterates - but it carries
            // neither code nor name, and a list of 406 bare integers is not one a county can be picked from.
            // uniqueCode stays false, or a multi-part county would collapse to one part and lose the rest of its
            // territory.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.County, parentId: null, uniqueCode: false, cancellationToken: cancellationToken);
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Counties could not be retrieved - the terrain point run cannot be scoped");
                return false;
            }

            List<TerrainPointDensityResult>? terrainPointDensityResults = null;
            if (gISWebAPIManager is not null)
            {
                System.Net.Http.HttpClient? httpClient_Terrain = gISWebAPIManager.CreateHttpClient<TerrainController>(nameof(TerrainController.GetDensitiesByCountyIdsAsync), out string? path_Densities);
                if (httpClient_Terrain is not null && !string.IsNullOrWhiteSpace(path_Densities))
                {
                    List<int> countyIds = [];
                    foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                    {
                        countyIds.Add(administrativeAreal2DReference.Id);
                    }

                    terrainPointDensityResults = [];
                    DiGi.WebAPI.Classes.PostOptions postOptions = new() { RequestResult = true };
                    int batchSize = WebAPI.Constants.Terrain.MaximumDensityCountyCount;

                    for (int i = 0; i < countyIds.Count; i += batchSize)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        int count = Math.Min(batchSize, countyIds.Count - i);
                        List<int> countyIds_Batch = countyIds.GetRange(i, count);

                        System.Text.StringBuilder stringBuilder = new(path_Densities);
                        for (int j = 0; j < countyIds_Batch.Count; j++)
                        {
                            stringBuilder.Append(j == 0 ? "?" : "&");
                            stringBuilder.Append("countyids=").Append(countyIds_Batch[j]);
                        }

                        if (PostgreSQLTerrainPointUpdateOptions.GridSize > 0)
                        {
                            stringBuilder.Append("&gridsize=").Append(PostgreSQLTerrainPointUpdateOptions.GridSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        }

                        try
                        {
                            DiGi.WebAPI.Classes.PostResponse<List<TerrainPointDensityResult>?> postResponse = await DiGi.WebAPI.Query.GetAsync<List<TerrainPointDensityResult>>(httpClient_Terrain, stringBuilder.ToString(), postOptions);
                            if (postResponse is not null && postResponse.Succeeded && postResponse.Result is List<TerrainPointDensityResult> results)
                            {
                                terrainPointDensityResults.AddRange(results);
                            }
                        }
                        catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                        {
                            Serilog.Modify.Log(exception, "Terrain point densities could not be retrieved for batch starting at index {Index}", i);
                        }
                    }
                }
            }

            // Read on this thread, shown on the user interface thread. Reading inside the callback below would
            // hold the interface still for the whole of the query.
            PostgreSQLTerrainPointUpdateOptions? postgreSQLTerrainPointUpdateOptions = null;

            application.Dispatcher.Invoke(() =>
            {
                PostgreSQLTerrainPointUpdateOptionsWindow postgreSQLTerrainPointUpdateOptionsWindow = new(PostgreSQLTerrainPointUpdateOptions, administrativeAreal2DReferences, terrainPointDensityResults);

                if (postgreSQLTerrainPointUpdateOptionsWindow.ShowDialog() is not bool dialogResult || !dialogResult)
                {
                    return;
                }

                postgreSQLTerrainPointUpdateOptions = postgreSQLTerrainPointUpdateOptionsWindow.PostgreSQLTerrainPointUpdateOptions;
            });

            // A cancelled dialog leaves the options of an earlier run as they were - the window works on a copy -
            // and ends the run here rather than starting a national pass nobody asked for.
            if (postgreSQLTerrainPointUpdateOptions is null)
            {
                Serilog.Modify.Log("Terrain point options were cancelled - nothing was sampled");
                return false;
            }

            PostgreSQLTerrainPointUpdateOptions = postgreSQLTerrainPointUpdateOptions;

            return await base.ExecuteAsync(progress, cancellationToken);
        }
    }
}
