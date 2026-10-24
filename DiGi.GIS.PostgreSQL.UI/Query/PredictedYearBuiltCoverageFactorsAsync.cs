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
        /// Asynchronously reads, from the deployed Web API, the share of each county part's buildings carrying a predicted year built.
        /// <para>Two requests whatever the number of parts, sent together: <c>yearbuiltdata/predictedyearbuiltruns</c> for the runs and <c>building2d/estimatedcountsbycountyids</c> for the building counts, the latter a planner estimate read in one catalogue query. Combined by <see cref="PredictedYearBuiltCoverageFactors"/>.</para>
        /// <para>Never throws for a failed read and is not retried: whatever goes wrong - an unreachable server, an error status, <see cref="Constants.YearBuiltPrediction.CoverageTimeout"/> elapsing, a body that does not parse - answers null, so a caller shows no marker rather than a false "not run". Only a cancellation through <paramref name="cancellationToken"/> is rethrown.</para>
        /// </summary>
        /// <param name="GISWebAPIManager">The manager that creates the HTTP clients.</param>
        /// <param name="countyIds">The county part identifiers, one per part of a multi-part county.</param>
        /// <param name="cancellationToken">Cancels both requests, which on the server cancels the queries behind them.</param>
        /// <returns>The share keyed by part id, or null when it could not be read.</returns>
        public static async Task<Dictionary<int, double>?> PredictedYearBuiltCoverageFactorsAsync(GISWebAPIManager? GISWebAPIManager, IEnumerable<int>? countyIds, CancellationToken cancellationToken = default)
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

            HttpClient? httpClient_Runs = GISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.GetPredictedYearBuiltRunsAsync), out string? path_Runs);
            HttpClient? httpClient_Counts = GISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetEstimatedCountsByCountyIdsAsync), out string? path_Counts);
            if (httpClient_Runs is null || string.IsNullOrWhiteSpace(path_Runs) || httpClient_Counts is null || string.IsNullOrWhiteSpace(path_Counts))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Predicted year built coverage cannot be requested - the endpoints are not available");
                return null;
            }

            // The command timeout matches the wait, so a query nobody is waiting for any more also stops on the server even if the
            // cancelled request never reaches it.
            int commandTimeout = (int)Constants.YearBuiltPrediction.CoverageTimeout.TotalSeconds;
            string requestUri_Runs = new UrlBuilder(path_Runs).AddParameter("countyids", countyIds_Distinct).AddParameter("commandtimeout", commandTimeout).ToString();
            string requestUri_Counts = new UrlBuilder(path_Counts).AddParameter("countyids", countyIds_Distinct).AddParameter("commandtimeout", commandTimeout).ToString();

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(Constants.YearBuiltPrediction.CoverageTimeout);

            try
            {
                Task<string?> task_Runs = StringAsync(httpClient_Runs, requestUri_Runs, cancellationTokenSource.Token);
                Task<string?> task_Counts = StringAsync(httpClient_Counts, requestUri_Counts, cancellationTokenSource.Token);

                await Task.WhenAll(task_Runs, task_Counts).ConfigureAwait(false);

                string? json_Runs = task_Runs.Result;
                string? json_Counts = task_Counts.Result;
                if (json_Runs is null || json_Counts is null)
                {
                    return null;
                }

                List<PredictedYearBuiltRunResult>? predictedYearBuiltRunResults = Core.Convert.ToDiGi<PredictedYearBuiltRunResult>(json_Runs);
                List<long?>? counts = System.Text.Json.JsonSerializer.Deserialize<List<long?>>(json_Counts);
                if (predictedYearBuiltRunResults is null || counts is null || counts.Count != countyIds_Distinct.Count)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Predicted year built coverage could not be parsed");
                    return null;
                }

                // The counts answer by position, one per identifier sent.
                Dictionary<int, long> building2DCounts = [];
                for (int i = 0; i < countyIds_Distinct.Count; i++)
                {
                    if (counts[i] is long count)
                    {
                        building2DCounts[countyIds_Distinct[i]] = count;
                    }
                }

                return PredictedYearBuiltCoverageFactors(predictedYearBuiltRunResults, building2DCounts);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                // Includes the timeout, which surfaces as a cancellation the caller did not ask for.
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Predicted year built coverage could not be read: {Message}", exception.Message);
                return null;
            }

            static async Task<string?> StringAsync(HttpClient httpClient, string requestUri, CancellationToken cancellationToken)
            {
                using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Predicted year built coverage request answered {StatusCode}: {RequestUri}", httpResponseMessage.StatusCode, requestUri);
                    return null;
                }

                return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
