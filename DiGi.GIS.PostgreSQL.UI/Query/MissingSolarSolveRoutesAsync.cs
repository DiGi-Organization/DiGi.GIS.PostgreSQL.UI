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
        /// Finds which of the GIS Web API routes a client-side solar solve needs the host does not serve yet, by comparing the host's <c>information/endpoints</c> catalogue with the routes this build was compiled against (DiGi.GIS.PostgreSQL.UI#48).
        /// <para>TODO [SolarSolveEndpointsDeployed]: remove this check, and its call in <see cref="Classes.WebAPIBuildingSolarRadiationSolveTask"/>, once DiGi-Organization/DiGi.GIS.WebAPI#94 is closed and <c>information/endpoints</c> on api.digiproject.uk lists every route named here. Until then a run against a host that lacks one would fail every county part, or - before the itemsbyreferences fix of the same build - count every building as not solvable.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task whose result contains the routes the host does not list (empty when every one is served), or null when the catalogue cannot be read - never read null as "all deployed".</returns>
        public static async Task<List<string>?> MissingSolarSolveRoutesAsync(this GISWebAPIManager? gISWebAPIManager, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null)
            {
                return null;
            }

            List<string?> routes =
            [
                DiGi.WebAPI.Query.Path<Building2DController>(nameof(Building2DController.GetCentroidsByCountyIdAsync)),
                DiGi.WebAPI.Query.Path<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.GetReferencesByCountyIdAsync)),
                DiGi.WebAPI.Query.Path<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.SetSolarRadiationsAsync)),
                DiGi.WebAPI.Query.Path<EPWFileController>(nameof(EPWFileController.GetEPWFileByIdAsync)),
                DiGi.WebAPI.Query.Path<EPWFileController>(nameof(EPWFileController.GetEPWFileLocationsAsync)),
            ];

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient(WebAPI.Constants.Name.Client.GIS);
            if (httpClient is null)
            {
                return null;
            }

            try
            {
                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, "information/endpoints", new PostOptions() { RequestResult = true });
                cancellationToken.ThrowIfCancellationRequested();
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                List<EndpointInformation>? endpointInformations = Core.Convert.ToDiGi<EndpointInformation>(postResponse.Result);
                if (endpointInformations is null || endpointInformations.Count == 0)
                {
                    return null;
                }

                HashSet<string> routeTemplates = new(StringComparer.OrdinalIgnoreCase);
                foreach (EndpointInformation? endpointInformation in endpointInformations)
                {
                    if (!string.IsNullOrWhiteSpace(endpointInformation?.RouteTemplate))
                    {
                        routeTemplates.Add(endpointInformation!.RouteTemplate!.Trim('/'));
                    }
                }

                List<string> result = [];
                foreach (string? route in routes)
                {
                    // A route this build cannot name is a broken build, not a deployed one.
                    if (string.IsNullOrWhiteSpace(route))
                    {
                        return null;
                    }

                    if (!routeTemplates.Contains(route!.Trim('/')))
                    {
                        result.Add(route);
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
                Serilog.Modify.Log(exception, "The endpoint catalogue of the GIS Web API could not be read");
                return null;
            }
        }
    }
}
