using System;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Maps an open building dataset key to the file type its release comes in: the name the add-files dialog shows and the extensions it accepts (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#55).
        /// <para>The Microsoft release is a tree of <c>part-&lt;n&gt;-&lt;guid&gt;.c000.csv.gz</c> files holding gzipped line-delimited GeoJSON, so it accepts <c>gz</c> as well as <c>geojsonl</c> - the reader recognises gzip by its first bytes, not by the extension. GHS-OBAT lists its GeoPackage only: the release also ships a CSV of the same rows, and importing both would duplicate every row, so the CSV stays reachable through All Files but is never offered or collected by default.</para>
        /// <para>An unknown or empty key answers the wildcard, which makes the dialog and the folder read accept every file.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key the tray row fixed, or null. Case and surrounding white space are ignored.</param>
        /// <returns>The type name and the extensions without the leading dot, lower case; the single extension <c>*</c> when the key names no known dataset.</returns>
        public static (string FileTypeName, string[] Extensions) OpenBuildingFileType(string? datasetKey)
        {
            return datasetKey?.Trim().ToUpperInvariant() switch
            {
                "EUBUCCO" => ("EUBUCCO GeoPackage", ["gpkg"]),
                "OVERTURE" => ("Overture GeoParquet", ["parquet"]),
                "OSM" => ("OpenStreetMap PBF", ["pbf"]),
                "DBSM" => ("DBSM GeoPackage", ["gpkg"]),
                "MICROSOFT" => ("Microsoft building footprints (GeoJSON sequence, gzipped or plain)", ["gz", "geojsonl"]),
                "GHS_OBAT" => ("GHS-OBAT GeoPackage", ["gpkg"]),
                _ => ("Source file", ["*"])
            };
        }
    }
}
