using DiGi.GIS.YOLO.UI.Classes;
using System.Globalization;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Parses the exclusion buffer typed in the training dialog - metres every exclusion box is grown by on every side (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#39).
        /// <para>Empty or whitespace means the class default, read from a fresh <see cref="YOLOTrainingDatasetOptions"/> so the dialog never carries its own copy of the number. Anything else has to be a finite, non-negative number in invariant culture (a decimal point, never a comma), so the same text means the same buffer on every machine.</para>
        /// </summary>
        /// <param name="text">The text of the buffer box, or null.</param>
        /// <param name="exclusionBuffer">The buffer in metres, or the class default when the text is empty; 0 when the text is refused.</param>
        /// <param name="reason">The reason the text is refused, or null when it is accepted.</param>
        /// <returns>True when the text is empty or a usable buffer.</returns>
        public static bool TryParseYOLOExclusionBuffer(string? text, out double exclusionBuffer, out string? reason)
        {
            reason = null;

            if (string.IsNullOrWhiteSpace(text))
            {
                exclusionBuffer = new YOLOTrainingDatasetOptions().ExclusionBuffer;
                return true;
            }

            if (!double.TryParse(text!.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value) || double.IsNaN(value) || double.IsInfinity(value) || value < 0)
            {
                exclusionBuffer = 0;
                reason = "The exclusion buffer has to be a number of metres of at least 0 (decimal point, not comma), or empty to use the default.";
                return false;
            }

            exclusionBuffer = value;
            return true;
        }
    }
}
