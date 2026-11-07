using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously reads, from the deployed Web API, how densely each of the given county partitions is sampled with terrain points.
        /// <para>The partitions are sent in batches of <c>WebAPI.Constants.Terrain.MaximumDensityCountyCount</c> to <c>terrain/densitiesbycountyids</c>, at most <see cref="Constants.TerrainPointDensity.MaxConcurrentRequests"/> at a time, and the whole read is bounded by <see cref="Constants.TerrainPointDensity.Timeout"/>, which is also sent as the server <c>commandtimeout</c>.</para>
        /// <para>Never throws for a failed read and is not retried: an unreachable server, an error status, the timeout elapsing or a body that does not parse - in <b>any</b> batch - answers null. A partial result is not returned, because a county left out of it would read as one that was never measured (DiGi.GIS.PostgreSQL.UI#41). Only a cancellation through <paramref name="cancellationToken"/> is rethrown.</para>
        /// </summary>
        /// <param name="GISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="countyIds">The county part identifiers, one per part of a multi-part county.</param>
        /// <param name="gridSize">The lattice spacing a sampling run used, in metres, which turns the density into a completeness. Null, or not a positive finite number, leaves it out.</param>
        /// <param name="cancellationToken">Cancels every request, which on the server cancels the queries behind them.</param>
        /// <returns>The densities keyed by part id, or null when they could not be read.</returns>
        public static async Task<Dictionary<int, TerrainPointDensityResult>?> TerrainPointDensityResultsAsync(GISWebAPIManager? GISWebAPIManager, IEnumerable<int>? countyIds, double? gridSize, CancellationToken cancellationToken = default)
        {
            if (GISWebAPIManager is null || countyIds is null)
            {
                return null;
            }

            List<int> countyIds_Distinct = [];
            HashSet<int> countyIds_HashSet = [];
            foreach (int countyId in countyIds)
            {
                if (countyId > 0 && countyIds_HashSet.Add(countyId))
                {
                    countyIds_Distinct.Add(countyId);
                }
            }

            if (countyIds_Distinct.Count == 0)
            {
                return null;
            }

            HttpClient? httpClient = GISWebAPIManager.CreateHttpClient<TerrainController>(nameof(TerrainController.GetDensitiesByCountyIdsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Terrain point densities cannot be requested - the endpoint is not available");
                return null;
            }

            int commandTimeout = (int)Constants.TerrainPointDensity.Timeout.TotalSeconds;
            int batchSize = WebAPI.Constants.Terrain.MaximumDensityCountyCount;

            List<string> requestUris = [];
            for (int i = 0; i < countyIds_Distinct.Count; i += batchSize)
            {
                UrlBuilder urlBuilder = new UrlBuilder(path).AddParameter("countyids", countyIds_Distinct.GetRange(i, Math.Min(batchSize, countyIds_Distinct.Count - i)));
                if (gridSize is double gridSize_Value && double.IsFinite(gridSize_Value) && gridSize_Value > 0)
                {
                    urlBuilder.AddParameter("gridsize", gridSize_Value);
                }

                requestUris.Add(urlBuilder.AddParameter("commandtimeout", commandTimeout).ToString());
            }

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(Constants.TerrainPointDensity.Timeout);

            using SemaphoreSlim semaphoreSlim = new(Constants.TerrainPointDensity.MaxConcurrentRequests);

            try
            {
                List<Task<string?>> tasks = requestUris.ConvertAll(x => StringAsync(httpClient, x, semaphoreSlim, cancellationTokenSource.Token));

                string?[] jsons = await Task.WhenAll(tasks).ConfigureAwait(false);

                Dictionary<int, TerrainPointDensityResult> result = [];
                foreach (string? json in jsons)
                {
                    if (json is null)
                    {
                        return null;
                    }

                    List<TerrainPointDensityResult>? terrainPointDensityResults = Core.Convert.ToDiGi<TerrainPointDensityResult>(json);
                    if (terrainPointDensityResults is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Terrain point densities could not be parsed");
                        return null;
                    }

                    foreach (TerrainPointDensityResult terrainPointDensityResult in terrainPointDensityResults)
                    {
                        if (terrainPointDensityResult is not null)
                        {
                            result[terrainPointDensityResult.CountyId] = terrainPointDensityResult;
                        }
                    }
                }

                return result;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                // Includes the timeout, which surfaces as a cancellation the caller did not ask for.
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Terrain point densities could not be read: {Message}", exception.Message);
                return null;
            }

            static async Task<string?> StringAsync(HttpClient httpClient, string requestUri, SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
            {
                await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Terrain point density request answered {StatusCode}: {RequestUri}", httpResponseMessage.StatusCode, requestUri);
                        return null;
                    }

                    return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
        }
    }
}
