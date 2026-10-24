using System;

namespace DiGi.GIS.PostgreSQL.UI.Constants
{
    /// <summary>
    /// Provides the constants of the Year Built prediction options shown by the GIS PostgreSQL UI.
    /// </summary>
    public static class YearBuiltPrediction
    {
        /// <summary>
        /// Gets the share of a county part's buildings that must carry a predicted year built before the part is marked as already predicted.
        /// <para>Strictly exceeded, not reached: a part at exactly this share is not marked. The marker answers only "has this county been run already?", so the threshold sits well above the stray predictions a partial or interrupted run leaves and well below a finished run, which covers almost every building of the part.</para>
        /// </summary>
        public const double CoverageMarkerThreshold = 0.1;

        /// <summary>
        /// Gets how long the coverage behind the markers is waited for before the dialog gives up on it.
        /// <para>The prediction runs are read by unpacking every year built row of the counties asked for, which took 2.6 s for 0.66 million predicted rows and grows with them - at national coverage it is expected to take about a minute. The read never holds the dialog up, so the limit only bounds how long a request lingers against a server that has stopped answering.</para>
        /// </summary>
        public static readonly TimeSpan CoverageTimeout = TimeSpan.FromSeconds(120);
    }
}
