using System.ComponentModel;

namespace DiGi.GIS.PostgreSQL.UI.Enums
{
    /// <summary>
    /// Specifies the operational mode for the GIS PostgreSQL UI.
    /// </summary>
    [Description("Mode")]
    public enum Mode
    {
        /// <summary>
        /// Indicates that the operation is performed on the server side.
        /// </summary>
        [Description("Server")] Server,

        /// <summary>
        /// Indicates that the operation is performed on the client side.
        /// </summary>
        [Description("Client")] Client,

        /// <summary>
        /// Indicates that the operation is performed on both the server and client sides.
        /// </summary>
        [Description("Server And Client")] ServerAndClient,
    }
}