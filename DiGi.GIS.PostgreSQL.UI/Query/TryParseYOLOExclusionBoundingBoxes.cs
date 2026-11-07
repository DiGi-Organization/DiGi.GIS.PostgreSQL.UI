using DiGi.GIS.YOLO.UI.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Parses the exclusion boxes typed in the training dialog: one EPSG:2180 box per line as <c>minX minY maxX maxY</c> (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#39).
        /// <para>The four numbers are separated by semicolons, tabs or spaces, so the <c>epsg2180_x1..y2</c> columns of the T30 area list paste in as they are. Numbers are read in invariant culture, which is why a comma is not a separator: under pl-PL it is the decimal mark, and reading it either way would silently move a box. Blank lines and lines starting with <c>#</c> are skipped. A line that does not hold exactly four finite numbers refuses the whole text and names its line number - one wrong box would leave a test area in the training set.</para>
        /// <para>Corners are kept as typed: the exclusion test reads the area between them, so a pair given in either order names the same box.</para>
        /// </summary>
        /// <param name="text">The text of the box editor, or null.</param>
        /// <param name="yOLOExclusionBoundingBoxes">The parsed boxes in line order - empty for an empty text - or null when the text is refused.</param>
        /// <param name="reason">The reason the text is refused, or null when it is accepted.</param>
        /// <returns>True when every non-blank line is a box.</returns>
        public static bool TryParseYOLOExclusionBoundingBoxes(string? text, out List<YOLOExclusionBoundingBox>? yOLOExclusionBoundingBoxes, out string? reason)
        {
            yOLOExclusionBoundingBoxes = null;
            reason = null;

            List<YOLOExclusionBoundingBox> yOLOExclusionBoundingBoxes_Parsed = [];
            if (string.IsNullOrWhiteSpace(text))
            {
                yOLOExclusionBoundingBoxes = yOLOExclusionBoundingBoxes_Parsed;
                return true;
            }

            char[] separators = [';', '\t', ' '];

            string[] lines = text!.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                string[] parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                double[] values = new double[4];

                bool valid = parts.Length == 4;
                for (int j = 0; valid && j < 4; j++)
                {
                    valid = double.TryParse(parts[j], NumberStyles.Float, CultureInfo.InvariantCulture, out values[j]) && !double.IsNaN(values[j]) && !double.IsInfinity(values[j]);
                }

                if (!valid)
                {
                    reason = string.Format(CultureInfo.InvariantCulture, "Exclusion box line {0} has to hold four numbers - minX minY maxX maxY in EPSG:2180, separated by ; tab or space, decimal point not comma: {1}", i + 1, line);
                    return false;
                }

                yOLOExclusionBoundingBoxes_Parsed.Add(new YOLOExclusionBoundingBox(values[0], values[1], values[2], values[3]));
            }

            yOLOExclusionBoundingBoxes = yOLOExclusionBoundingBoxes_Parsed;
            return true;
        }
    }
}
