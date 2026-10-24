using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Returns, per county part, the share of its buildings carrying a predicted year built.
        /// <para>The share is the count of the part's <b>largest</b> run over its building count, never the sum of its runs. A run does not replace the one before it - a building keeps an entry per run - so county 5 carried 10 437 objects from an old run and 33 584 from the current one out of 33 687 buildings, and summing would read 131 %.</para>
        /// </summary>
        /// <param name="predictedYearBuiltRunResults">The prediction runs, as <c>predictedyearbuiltruns</c> returns them. Null entries are ignored.</param>
        /// <param name="building2DCounts">The building count of each county part, keyed by part id.</param>
        /// <returns>The share keyed by part id, for every part that has a run and a positive building count - a part without a count is left out rather than given a share it cannot have. Empty when either input is null.</returns>
        public static Dictionary<int, double> PredictedYearBuiltCoverageFactors(IEnumerable<PredictedYearBuiltRunResult?>? predictedYearBuiltRunResults, IReadOnlyDictionary<int, long>? building2DCounts)
        {
            Dictionary<int, double> result = [];
            if (predictedYearBuiltRunResults is null || building2DCounts is null)
            {
                return result;
            }

            Dictionary<int, int> counts_Maximum = [];
            foreach (PredictedYearBuiltRunResult? predictedYearBuiltRunResult in predictedYearBuiltRunResults)
            {
                if (predictedYearBuiltRunResult is null)
                {
                    continue;
                }

                int countyId = predictedYearBuiltRunResult.CountyId;
                if (!counts_Maximum.TryGetValue(countyId, out int count) || predictedYearBuiltRunResult.Count > count)
                {
                    counts_Maximum[countyId] = predictedYearBuiltRunResult.Count;
                }
            }

            foreach (KeyValuePair<int, int> keyValuePair in counts_Maximum)
            {
                if (building2DCounts.TryGetValue(keyValuePair.Key, out long building2DCount) && building2DCount > 0)
                {
                    result[keyValuePair.Key] = (double)keyValuePair.Value / building2DCount;
                }
            }

            return result;
        }
    }
}
