using DiGi.EPW.Classes;
using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.PostgreSQL.Solar.Classes;
using DiGi.GIS.WebAPI;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildingModel = DiGi.Analytical.Building.Classes.BuildingModel;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Solves the annual solar radiation of every building of the chosen county parts on this machine, reading the inputs from and writing the results to the GIS Web API instead of the storage database, so a Client machine can take the solve off the database server (DiGi.GIS.PostgreSQL.UI#48).
    /// <para>The run - scope, tiling, batching, parallel solve, counting and logging - is <see cref="BuildingSolarRadiationSolveTask"/>'s, shared with <see cref="PostgreSQLBuildingSolarRadiationSolveTask"/>; this class supplies the reads and the write, each the HTTP counterpart of the converter read the server task makes: <c>gis/building2d/centroidsbycountyid</c>, <c>gis/buildingsolarradiation/referencesbycountyid</c>, <c>gis/buildingmodel/itemsbyreferences</c> and <c>itemsbycircle</c>, <c>gis/epwfile/locations</c> and <c>itembyid</c>, and <c>gis/buildingsolarradiation/setsolarradiations</c>.</para>
    /// <para>A 404 from <c>itemsbyreferences</c> or <c>itemsbycircle</c> means nothing is stored there and reads as empty; any other failure is null and fails the county part, as a failed database read does. The receivers of a batch are read in requests of a bounded query length, so a large <see cref="PostgreSQLBuildingSolarRadiationSolveOptions.BatchSize"/> never meets a reverse proxy's request-line limit. Each statement's <see cref="PostgreSQLBuildingSolarRadiationSolveOptions.CommandTimeout"/> travels as the endpoint's <c>commandtimeout</c> where it takes one and bounds the request either way.</para>
    /// <para>The write needs a signed-in user's bearer token (<see cref="TokenString"/>) and <c>AllowUpdateSolarRadiationData</c> on the host; the host stamps the user email and time. Each write request is all-or-nothing on the server, so a stopped run leaves only whole requests stored, and <see cref="PostgreSQLBuildingSolarRadiationSolveOptions.SkipExisting"/> resumes it.</para>
    /// </summary>
    public class WebAPIBuildingSolarRadiationSolveTask : BuildingSolarRadiationSolveTask
    {
        /// <summary>
        /// The largest length, in characters, of the encoded <c>references</c> part of one <c>itemsbyreferences</c> request; a batch above it is read in several requests.
        /// </summary>
        private const int referencesQueryLengthMax = 2000;

        /// <summary>
        /// The GIS Web API manager used to create the HTTP clients.
        /// </summary>
        protected readonly GISWebAPIManager gISWebAPIManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAPIBuildingSolarRadiationSolveTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The GIS Web API manager used to read the counties, buildings, models and weather and to write the results.</param>
        public WebAPIBuildingSolarRadiationSolveTask(GISWebAPIManager gISWebAPIManager)
        {
            this.gISWebAPIManager = gISWebAPIManager ?? throw new ArgumentNullException(nameof(gISWebAPIManager));
        }

        /// <summary>
        /// Gets or sets the signed-in user's bearer token the results are written with; never stored or logged.
        /// <para>A token lives an hour and a run takes many, so before each write a token within <see cref="Constants.UserSession.RefreshMargin"/> of its expiry is exchanged for a new one (<see cref="Query.RefreshedUserTokenStringAsync"/>) and replaces this value. A renewal that fails leaves the old token in use while it is still valid; once it has expired, the write fails and so does its county part.</para>
        /// </summary>
        public string? TokenString { get; set; }

        /// <inheritdoc />
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            // TODO [SolarSolveEndpointsDeployed]: remove this check once DiGi-Organization/DiGi.GIS.WebAPI#94 is closed and information/endpoints on api.digiproject.uk lists every route of Query.MissingSolarSolveRoutesAsync.
            List<string>? routes_Missing = await gISWebAPIManager.MissingSolarSolveRoutesAsync(cancellationToken);
            if (routes_Missing is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the GIS Web API endpoint catalogue could not be read - it cannot be told whether the solve endpoints are deployed", GetType().Name);
                return false;
            }

            if (routes_Missing.Count != 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the GIS Web API does not serve {Routes} yet (DiGi.GIS.WebAPI#94) - nothing was solved", GetType().Name, string.Join(", ", routes_Missing));
                return false;
            }

            return await base.ExecuteAsync(progress, cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task<List<BuildingModel?>?> ReadBuildingModelsAsync(int countyId, List<string> references, int commandTimeout, CancellationToken cancellationToken)
        {
            string? path = DiGi.WebAPI.Query.Path<BuildingModelController>(nameof(BuildingModelController.GetItemsByReferencesAsync));
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            List<BuildingModel?> result = [];

            // The references go in the query string, so a batch is split before its query outgrows a request line.
            StringBuilder stringBuilder = new();
            for (int i = 0; i < references.Count; i++)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append('&');
                }

                stringBuilder.Append("references=").Append(WebUtility.UrlEncode(references[i]));
                if (stringBuilder.Length < referencesQueryLengthMax && i + 1 < references.Count)
                {
                    continue;
                }

                string requestUri = string.Format(CultureInfo.InvariantCulture, "{0}?{1}&countyid={2}", path, stringBuilder, countyId);
                stringBuilder.Clear();

                (bool succeeded, string? json) = await ReadAsync(requestUri, commandTimeout, cancellationToken);
                if (!succeeded)
                {
                    return null;
                }

                if (json is null)
                {
                    continue;
                }

                List<BuildingModel>? buildingModels = Core.Convert.ToDiGi<BuildingModel>(json);
                if (buildingModels is null)
                {
                    return null;
                }

                result.AddRange(buildingModels);
            }

            return result;
        }

        /// <inheritdoc />
        protected override async Task<List<Building2DCentroid>?> ReadBuilding2DCentroidsAsync(int countyId, int commandTimeout, CancellationToken cancellationToken)
        {
            return await gISWebAPIManager.Building2DCentroidsByCountyIdAsync(countyId, commandTimeout, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task<List<AdministrativeAreal2DReference>?> ReadCountyReferencesAsync(int commandTimeout, CancellationToken cancellationToken)
        {
            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<AdministrativeAreal2DController>(nameof(AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            // The type is not nullable on the server - omitted, it binds to Country. Every part, not one per code.
            string requestUri = new UrlBuilder(path).AddParameter("administrativearealtype", (int)AdministrativeArealType.County).AddParameter("uniquecode", "false").ToString();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                PostResponse<List<AdministrativeAreal2DReference>?> postResponse = await DiGi.WebAPI.Query.GetAsync<List<AdministrativeAreal2DReference>>(httpClient, requestUri, PostOptions(commandTimeout));
                return postResponse is not null && postResponse.Succeeded ? postResponse.Result : null;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}: the county parts could not be read", GetType().Name);
                return null;
            }
        }

        /// <inheritdoc />
        protected override async Task<EPWFile?> ReadEPWFileAsync(int id, int commandTimeout, CancellationToken cancellationToken)
        {
            return await gISWebAPIManager.EPWFileByIdAsync(id, PostOptions(commandTimeout), cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task<List<(int Id, double X, double Y)>?> ReadEPWFileLocationsAsync(int commandTimeout, CancellationToken cancellationToken)
        {
            List<EPWFileLocationResult>? ePWFileLocationResults = await gISWebAPIManager.EPWFileLocationsAsync(PostOptions(commandTimeout), cancellationToken);
            if (ePWFileLocationResults is null)
            {
                return null;
            }

            List<(int Id, double X, double Y)> result = [];
            foreach (EPWFileLocationResult? ePWFileLocationResult in ePWFileLocationResults)
            {
                if (ePWFileLocationResult is not null)
                {
                    result.Add((ePWFileLocationResult.Id, ePWFileLocationResult.X, ePWFileLocationResult.Y));
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override async Task<List<BuildingModel?>?> ReadNeighbourBuildingModelsAsync(Circle2D circle2D, int commandTimeout, CancellationToken cancellationToken)
        {
            string? path = DiGi.WebAPI.Query.Path<BuildingModelController>(nameof(BuildingModelController.GetItemsByCircleAsync));
            if (string.IsNullOrWhiteSpace(path) || circle2D?.Center is not Point2D center)
            {
                return null;
            }

            // The tolerance the server-side read uses (BuildingModelsByCircle2DAsync), sent rather than left to the route's default.
            string requestUri = new UrlBuilder(path).AddParameter("x", center.X).AddParameter("y", center.Y).AddParameter("radius", circle2D.Radius).AddParameter("tolerance", Core.Constants.Tolerance.Distance).ToString();

            (bool succeeded, string? json) = await ReadAsync(requestUri, commandTimeout, cancellationToken);
            if (!succeeded)
            {
                return null;
            }

            if (json is null)
            {
                return [];
            }

            List<BuildingModel>? buildingModels = Core.Convert.ToDiGi<BuildingModel>(json);
            return buildingModels is null ? null : [.. buildingModels];
        }

        /// <inheritdoc />
        protected override async Task<HashSet<string>?> ReadSolvedReferencesAsync(int countyId, double radius, int solverVersion, int commandTimeout, CancellationToken cancellationToken)
        {
            return await gISWebAPIManager.BuildingSolarRadiationReferencesAsync(countyId, radius, solverVersion, commandTimeout, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        protected override bool TryInitialize()
        {
            if (string.IsNullOrWhiteSpace(TokenString))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no user is signed in - the results cannot be written", GetType().Name);
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        protected override async Task<int?> WriteAsync(int countyId, List<GIS.Classes.BuildingSolarRadiationResult> buildingSolarRadiationResults, int commandTimeout, CancellationToken cancellationToken)
        {
            // Renewed here, not on a timer: writes are the only requests that carry the token, and they run one at a time.
            if (Query.TokenExpiresAt(TokenString) is DateTimeOffset expiresAt && expiresAt - DateTimeOffset.UtcNow <= Constants.UserSession.RefreshMargin)
            {
                string? tokenString_Refreshed = await gISWebAPIManager.RefreshedUserTokenStringAsync(TokenString, cancellationToken);
                if (tokenString_Refreshed is not null)
                {
                    TokenString = tokenString_Refreshed;
                    Serilog.Modify.Log("{Type}: the session was renewed", GetType().Name);
                }
                else if (expiresAt <= DateTimeOffset.UtcNow)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the session expired and could not be renewed - sign in again and rerun; the stored results are skipped", GetType().Name);
                    return null;
                }
                else
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}: the session could not be renewed - writing with the current token until it expires at {ExpiresAt}", GetType().Name, expiresAt);
                }
            }

            return await gISWebAPIManager.SetBuildingSolarRadiationsAsync(buildingSolarRadiationResults, TokenString, commandTimeout, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates the options of a request bounded by the statement timeout, as the client helpers bound theirs.
        /// </summary>
        /// <param name="commandTimeout">The timeout in seconds; 0 leaves the request unbounded here.</param>
        /// <returns>The options, with the result requested.</returns>
        private static PostOptions PostOptions(int commandTimeout)
        {
            return new PostOptions() { Delay = commandTimeout <= 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(commandTimeout), RequestResult = true };
        }

        /// <summary>
        /// Sends a GET whose 404 means "nothing stored there" rather than a failure, which <c>DiGi.WebAPI.Query.GetAsync</c> cannot tell apart.
        /// </summary>
        /// <param name="requestUri">The request URI, relative to the GIS Web API.</param>
        /// <param name="commandTimeout">The timeout in seconds of the request; 0 leaves it unbounded here.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task whose result is (true, body) for a 2xx answer, (true, null) for 404 or 204, and (false, null) for any other answer or a transport failure.</returns>
        private async Task<(bool Succeeded, string? Json)> ReadAsync(string requestUri, int commandTimeout, CancellationToken cancellationToken)
        {
            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient(WebAPI.Constants.Name.Client.GIS);
            if (httpClient is null)
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
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {RequestUri} answered {StatusCode}", GetType().Name, requestUri, (int)httpResponseMessage.StatusCode);
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
                Serilog.Modify.Log(exception, "{Type}: {RequestUri} could not be read", GetType().Name, requestUri);
                return (false, null);
            }
        }
    }
}
