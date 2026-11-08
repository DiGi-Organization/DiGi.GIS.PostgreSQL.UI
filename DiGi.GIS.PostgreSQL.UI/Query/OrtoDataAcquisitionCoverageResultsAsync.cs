using DiGi.GIS.PostgreSQL.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously reads how far the acquisition-date backfill has reached in each of the given county parts, from the database the backfill writes to.
        /// <para>Read through the converter rather than the Web API on purpose: the backfill runs against the database its configuration names, and the coverage beside a county has to describe that same database. Through the API it would describe production even on a development machine, where the backfill writes elsewhere.</para>
        /// <para>One grouped statement (<see cref="OrtoDatasPostgreSQLConverter.GetAcquisitionCoveragesByCountyIdsAsync(IEnumerable{int}?, int, CancellationToken)"/>), bounded by <see cref="Constants.OrtoDataAcquisitionCoverage.Timeout"/>, which is also its command timeout. A part holding no series is absent from the result - a measured "nothing stored", not an unknown.</para>
        /// <para>Never throws for a failed read and is not retried: a missing converter, a connection that cannot be opened, the timeout elapsing or a failing statement answer null. Only a cancellation through <paramref name="cancellationToken"/> is rethrown.</para>
        /// </summary>
        /// <param name="ortoDatasPostgreSQLConverter">The converter of the <c>orto_datas</c> table, which shares its database with <c>orto_data_acquisition</c>.</param>
        /// <param name="countyIds">The county part identifiers, one per part of a multi-part county.</param>
        /// <param name="cancellationToken">Cancels the read, which also cancels the statement on the server.</param>
        /// <returns>The coverage keyed by county part id, or null when it could not be read.</returns>
        public static async Task<Dictionary<int, OrtoDataAcquisitionCoverageResult>?> OrtoDataAcquisitionCoverageResultsAsync(OrtoDatasPostgreSQLConverter? ortoDatasPostgreSQLConverter, IEnumerable<int>? countyIds, CancellationToken cancellationToken = default)
        {
            if (ortoDatasPostgreSQLConverter is null || countyIds is null)
            {
                return null;
            }

            HashSet<int> countyIds_HashSet = [];
            foreach (int countyId in countyIds)
            {
                if (countyId > 0)
                {
                    countyIds_HashSet.Add(countyId);
                }
            }

            if (countyIds_HashSet.Count == 0)
            {
                return null;
            }

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(Constants.OrtoDataAcquisitionCoverage.Timeout);

            try
            {
                List<OrtoDataAcquisitionCoverageResult>? ortoDataAcquisitionCoverageResults = await ortoDatasPostgreSQLConverter.GetAcquisitionCoveragesByCountyIdsAsync(countyIds_HashSet, (int)Constants.OrtoDataAcquisitionCoverage.Timeout.TotalSeconds, cancellationTokenSource.Token).ConfigureAwait(false);
                if (ortoDataAcquisitionCoverageResults is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas acquisition coverage could not be read - the store could not be reached or holds no orto_datas table");
                    return null;
                }

                Dictionary<int, OrtoDataAcquisitionCoverageResult> result = [];
                foreach (OrtoDataAcquisitionCoverageResult ortoDataAcquisitionCoverageResult in ortoDataAcquisitionCoverageResults)
                {
                    if (ortoDataAcquisitionCoverageResult is not null)
                    {
                        result[ortoDataAcquisitionCoverageResult.CountyId] = ortoDataAcquisitionCoverageResult;
                    }
                }

                return result;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas acquisition coverage was not read within {Timeout}", Constants.OrtoDataAcquisitionCoverage.Timeout);
                return null;
            }
            catch (NpgsqlException npgsqlException) when (cancellationToken.IsCancellationRequested)
            {
                // A statement cancelled on the server surfaces as a PostgreSQL error rather than as a cancellation.
                throw new OperationCanceledException(npgsqlException.Message, npgsqlException, cancellationToken);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OrtoDatas acquisition coverage could not be read: {Message}", exception.Message);
                return null;
            }
        }
    }
}
