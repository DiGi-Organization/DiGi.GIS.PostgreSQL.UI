using System;

namespace DiGi.GIS.PostgreSQL.UI.Constants
{
    /// <summary>
    /// Provides the constants of the terrain point density read behind the county labels of the terrain point options dialogs.
    /// </summary>
    public static class TerrainPointDensity
    {
        /// <summary>
        /// Gets how many density requests are in flight at once.
        /// <para>Each request counts the stored points of up to <c>WebAPI.Constants.Terrain.MaximumDensityCountyCount</c> county partitions on the database server, so the 406 county parts are about nine requests. Sent one after another they are what kept the dialog closed (DiGi.GIS.PostgreSQL.UI#41); sent all at once they are nine aggregates competing with whatever else the server runs. Three is the middle.</para>
        /// </summary>
        public const int MaxConcurrentRequests = 3;

        /// <summary>
        /// Gets how long the densities behind the county labels are waited for before the dialog gives up on them.
        /// <para>Sent as the server <c>commandtimeout</c> too, so a query nobody is waiting for any more also stops on PostgreSQL. Not measured against the deployed estate yet: it replaces a read that had no limit at all (600 s per request, one request after another), and the read never holds the dialog up, so the limit only bounds how long requests linger against a server that has stopped answering.</para>
        /// </summary>
        public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(300);
    }
}
