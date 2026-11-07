using DiGi.GIS.YOLO.UI.Classes;
using System.Collections.Generic;
using System.Globalization;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Convert
    {
        /// <summary>
        /// Formats exclusion boxes as the training dialog's box editor shows them: one box per line, <c>minX; minY; maxX; maxY</c> in EPSG:2180 (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#39).
        /// <para>Numbers are written in invariant culture with the round-trip format, so <see cref="Query.TryParseYOLOExclusionBoundingBoxes(string?, out List{YOLOExclusionBoundingBox}?, out string?)"/> reads back exactly the boxes that were written, on any machine culture.</para>
        /// </summary>
        /// <param name="yOLOExclusionBoundingBoxes">The boxes, or null.</param>
        /// <returns>The text, empty for no boxes; null boxes are skipped.</returns>
        public static string ToSystem_String(this IEnumerable<YOLOExclusionBoundingBox>? yOLOExclusionBoundingBoxes)
        {
            if (yOLOExclusionBoundingBoxes is null)
            {
                return string.Empty;
            }

            List<string> lines = [];
            foreach (YOLOExclusionBoundingBox? yOLOExclusionBoundingBox in yOLOExclusionBoundingBoxes)
            {
                if (yOLOExclusionBoundingBox is null)
                {
                    continue;
                }

                lines.Add(string.Join("; ",
                    yOLOExclusionBoundingBox.MinX.ToString("R", CultureInfo.InvariantCulture),
                    yOLOExclusionBoundingBox.MinY.ToString("R", CultureInfo.InvariantCulture),
                    yOLOExclusionBoundingBox.MaxX.ToString("R", CultureInfo.InvariantCulture),
                    yOLOExclusionBoundingBox.MaxY.ToString("R", CultureInfo.InvariantCulture)));
            }

            return string.Join("\r\n", lines);
        }
    }
}
