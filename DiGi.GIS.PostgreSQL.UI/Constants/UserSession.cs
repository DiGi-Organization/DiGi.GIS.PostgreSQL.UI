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
        /// Gets how long before its expiry a token is replaced. A token lives an hour (<c>DiGi.User.WebAPI</c> <c>Constants.Session.TokenLifetime</c>) and a write may take minutes, so it is renewed well before a request could carry an expired one.
        /// </summary>
        public static readonly TimeSpan RefreshMargin = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets the timeout of a sign-in or refresh request.
        /// </summary>
        public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);
    }
}
