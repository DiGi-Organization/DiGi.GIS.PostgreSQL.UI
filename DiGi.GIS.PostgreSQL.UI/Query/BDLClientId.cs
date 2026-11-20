using DiGi.Core.Classes;
using System;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Returns the BDL API client identifier of this machine: the <c>ClientId</c> key of <c>BDL.conf</c> beside the application, where the build copies the git-ignored <c>user files</c> folder (DiGi-Organization/DiGi.GIS.PostgreSQL#149).
        /// <para>A registered client id raises the BDL limits well above the anonymous ones, which stopped a full write after 8 units. The value is a secret: it is read, never written or logged.</para>
        /// </summary>
        /// <param name="path">The configuration file, or null for <c>BDL.conf</c> in <see cref="AppContext.BaseDirectory"/>.</param>
        /// <returns>The client identifier, or null when the file or the key is missing or empty.</returns>
        public static string? BDLClientId(string? path = null)
        {
            path ??= System.IO.Path.Combine(AppContext.BaseDirectory, "BDL.conf");
            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            ConfigurationFile configurationFile = new();
            if (!configurationFile.Read(path) || !configurationFile.TryGetValue("ClientId", out string? clientId) || string.IsNullOrWhiteSpace(clientId))
            {
                return null;
            }

            return clientId!.Trim();
        }
    }
}
