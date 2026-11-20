using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Sends a GET whose 404 or 204 means "nothing stored there" rather than a failure, which <c>DiGi.WebAPI.Query.GetAsync</c> cannot tell apart: it throws for every unsuccessful answer, the valid 404 included (DiGi.GIS.PostgreSQL.UI#51).
        /// <para>Any other unsuccessful answer is logged at Error with the request and its status, and a transport failure or the timeout elapsing is logged with its exception; both answer a failure, never an empty result. Only a cancellation through <paramref name="cancellationToken"/> is rethrown.</para>
        /// </summary>
        /// <param name="httpClient">The HTTP client the request is sent with. A null client answers a failure.</param>
        /// <param name="requestUri">The request URI, relative to the client's base address. A blank URI answers a failure.</param>
        /// <param name="commandTimeout">The timeout in seconds of the request; 0 or less leaves it unbounded here.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
        /// <returns>A task whose result is (true, body) for a 2xx answer with a body, (true, null) for 404, 204 or an empty body, and (false, null) for any other answer or a transport failure.</returns>
        public static async Task<(bool Succeeded, string? Json)> JsonResponseAsync(this HttpClient? httpClient, string? requestUri, int commandTimeout, CancellationToken cancellationToken = default)
        {
            if (httpClient is null || string.IsNullOrWhiteSpace(requestUri))
            {
                return (false, null);
            }

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if (commandTimeout > 0)
            {
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(commandTimeout));
            }

            try
            {
                using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri, cancellationTokenSource.Token);
                if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound || httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                {
                    return (true, null);
                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {RequestUri} answered {StatusCode}", nameof(JsonResponseAsync), requestUri, (int)httpResponseMessage.StatusCode);
                    return (false, null);
                }

                string json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationTokenSource.Token);
                return string.IsNullOrWhiteSpace(json) ? (true, null) : (true, json);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}: {RequestUri} could not be read", nameof(JsonResponseAsync), requestUri);
                return (false, null);
            }
        }
    }
}
