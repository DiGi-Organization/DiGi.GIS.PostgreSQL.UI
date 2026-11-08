using System;

namespace DiGi.GIS.PostgreSQL.UI.Constants
{
    /// <summary>
    /// Provides the constants of the acquisition-date coverage read behind the county labels of the acquisition backfill options dialog.
    /// </summary>
    public static class OrtoDataAcquisitionCoverage
    {
        /// <summary>
        /// Gets how long the coverage behind the county labels is waited for before the dialog gives up on it.
        /// <para>Sent as the PostgreSQL command timeout too, so a query nobody is waiting for any more also stops on the server. One grouped statement over every county part (DiGi.GIS.PostgreSQL#134): it projects <c>county_id</c> and <c>reference</c> of <c>orto_datas</c> and never <c>object</c>, against the acquisition rows aggregated per series. Not measured against the production store yet (DiGi.GIS.WebAPI#78); on a development store of 33 695 series it answered at once. Matches the backfill's own default command timeout, and the read never holds the dialog up, so the limit only bounds how long a query lingers against a database that has stopped answering.</para>
        /// </summary>
        public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(300);
    }
}
