using DiGi.GIS.WebAPI.Classes;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Signs a user in at the user Web API (<c>user/login</c>, on the GIS Web API's host) and answers the session token a write to the GIS Web API is authorized with.
        /// <para>The body is <c>{"Email":…,"Password":…}</c> with the names <c>DiGi.User.Classes.UserLogin</c> declares, sent with a length rather than chunked. The password is used for this one request and never logged; the login answers 401 for every kind of refusal, so a refusal is not told apart here either (<c>Coding - WebAPI Contracts.md</c> §4).</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager whose GIS client reaches the host.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task whose result contains the session token, or null when an argument is blank, the credentials were refused or the request failed.</returns>
        public static async Task<string?> UserTokenStringAsync(this GISWebAPIManager? gISWebAPIManager, string? email, string? password, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient(WebAPI.Constants.Name.Client.GIS);
            if (httpClient is null)
            {
                return null;
            }

            JsonObject jsonObject = new()
            {
                ["Email"] = email,
                ["Password"] = password
            };

            using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(Constants.UserSession.RequestTimeout);

            try
            {
                using StringContent stringContent = new(jsonObject.ToJsonString(), Encoding.UTF8, "application/json");
                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(Constants.UserSession.LoginRoute, stringContent, cancellationTokenSource.Token);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Sign-in was refused with {StatusCode}", (int)httpResponseMessage.StatusCode);
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
                Serilog.Modify.Log(exception, "Sign-in could not be completed");
                return null;
            }
        }
    }
}
