namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Returns the four-digit county code a PRG address point belongs to.
        /// <para>The code is the first four digits of the seven-digit TERYT commune code (<c>TERYTGminy</c>): voivodeship, county, commune and commune type. It is a code, not a key - a county of disconnected territory holds one row per polygon part, so the identifiers are resolved from it with the <c>idsbycode</c> endpoint rather than assumed.</para>
        /// </summary>
        /// <param name="TERYTGminy">The seven-digit TERYT commune code of an address point.</param>
        /// <returns>The four-digit county code; null when the input is null, shorter than four characters or its first four characters are not digits.</returns>
        public static string? CountyCode(string? TERYTGminy)
        {
            if (TERYTGminy is null)
            {
                return null;
            }

            string value = TERYTGminy.Trim();
            if (value.Length < 4)
            {
                return null;
            }

            for (int i = 0; i < 4; i++)
            {
                if (value[i] < '0' || value[i] > '9')
                {
                    return null;
                }
            }

            return value[..4];
        }
    }
}
