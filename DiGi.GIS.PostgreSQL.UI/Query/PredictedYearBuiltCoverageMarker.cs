using System.Globalization;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Returns the marker shown after a county in the Year Built prediction options when its predicted coverage exceeds <see cref="Constants.YearBuiltPrediction.CoverageMarkerThreshold"/>.
        /// <para>The text is built from the threshold, invariantly, so the two cannot drift apart and a Polish culture does not turn it into <c>(&gt;10 %)</c>.</para>
        /// </summary>
        /// <param name="coverageFactor">The share of the county part's buildings carrying a predicted year built, as a fraction. Null when it is not known.</param>
        /// <returns><c>(&gt;10%)</c> when the factor strictly exceeds the threshold; null when it does not, when it is not a number, or when it is unknown - no marker, never a false "not run".</returns>
        public static string? PredictedYearBuiltCoverageMarker(double? coverageFactor)
        {
            if (coverageFactor is not double value || double.IsNaN(value) || value <= Constants.YearBuiltPrediction.CoverageMarkerThreshold)
            {
                return null;
            }

            return string.Format(CultureInfo.InvariantCulture, "(>{0}%)", Constants.YearBuiltPrediction.CoverageMarkerThreshold * 100);
        }
    }
}
