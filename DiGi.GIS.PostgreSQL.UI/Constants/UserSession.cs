using System;

namespace DiGi.GIS.PostgreSQL.UI.Constants
{
    /// <summary>
    /// Provides the routes and timings of a user session against the user Web API, which shares its host with the GIS Web API.
    /// </summary>
    public static class UserSession
    {
        /// <summary>
        /// Gets the route, relative to the host, that exchanges an email and password for a session token (<c>DiGi.User.WebAPI</c> <c>UserController.LoginAsync</c>).
        /// </summary>
        public const string LoginRoute = "user/login";

        /// <summary>
        /// Gets the route, relative to the host, that issues a new token for the identity of the presented one (<c>UserController.Refresh</c>).
        /// </summary>
        public const string RefreshRoute = "user/session/refresh";

        /// <summary>
        /// Gets how often a running task checks whether its token is due for renewal. The check reads the token's expiry only; a request is sent once the token is within <see cref="RefreshMargin"/>, and while a renewal keeps failing it is retried at this interval.
        /// </summary>
        public static readonly TimeSpan RefreshCheckInterval = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets how long before its expiry a token is replaced. A token lives an hour (<c>DiGi.User.WebAPI</c> <c>Constants.Session.TokenLifetime</c>) and a renewal needs a still valid token, so it is renewed early enough for <see cref="RefreshCheckInterval"/> retries to outlast a short outage.
        /// </summary>
        public static readonly TimeSpan RefreshMargin = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Gets the timeout of a sign-in or refresh request.
        /// </summary>
        public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);
    }
}
