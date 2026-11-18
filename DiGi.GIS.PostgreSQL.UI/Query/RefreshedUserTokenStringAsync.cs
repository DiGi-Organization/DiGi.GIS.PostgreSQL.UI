using DiGi.GIS.WebAPI.Classes;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Exchanges a still valid session token for a new one at the user Web API (<c>user/session/refresh</c>), so a run longer than a token's hour keeps writing.
        /// <para>The presented token stays valid until its own expiry; the new one starts an independent session. An expired or revoked token is refused with 401 and the user has to sign in again.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager whose GIS client reaches the host.</param>
        /// <param name="tokenString">The current session token.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task whose result contains the new token, or null when the token is blank, was refused or the request failed.</returns>
        public static async Task<string?> RefreshedUserTokenStringAsync(this GISWebAPIManager? gISWebAPIManager, string? tokenString, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || string.IsNullOrWhiteSpace(tokenString))
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient(WebAPI.Constants.Name.Client.GIS);
            if (httpClient is null)
            {
                return null;
            }

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(Constants.UserSession.RequestTimeout);

            try
            {
                // On the request, not on the client: the client may be shared, and a default header would outlive this call.
                using HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, Constants.UserSession.RefreshRoute);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationTokenSource.Token);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "The session could not be renewed: {StatusCode}", (int)httpResponseMessage.StatusCode);
                    return null;
                }

                return TokenString(await httpResponseMessage.Content.ReadAsStringAsync(cancellationTokenSource.Token));
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The session could not be renewed");
                return null;
            }
        }
    }
}
