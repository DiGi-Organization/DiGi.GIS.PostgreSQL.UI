using System;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application context instance for the GIS PostgreSQL tray application.
        /// </summary>
        private Classes.GISPostgreSQLTrayApplicationContext? gISPostgreSQLTrayApplicationContext;

        /// <summary>
        /// Overrides the OnStartup method to initialize application-wide settings and exception handling.
        /// </summary>
        /// <param name="e">The event data for the startup event.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Catch exceptions from the main UI thread
            Current.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Catch exceptions from other threads
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Disable hardware acceleration to diagnose rendering issues
            System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            base.OnStartup(e);

            // Prevent application from shutting down when all windows are closed
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            gISPostgreSQLTrayApplicationContext = new Classes.GISPostgreSQLTrayApplicationContext();
        }

        /// <summary>
        /// Handles unhandled exceptions that occur on the main UI dispatcher thread.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the exception details.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception currentException = e.Exception;
            string errorMessage = string.Format(System.Globalization.CultureInfo.CurrentCulture, "UI Thread Exception: {0}\n{1}", currentException.Message, currentException.StackTrace);

            MessageBox.Show(errorMessage, "Critical UI Error");

            // Prevent the application from crashing immediately to allow for state inspection
            e.Handled = true;
        }

        /// <summary>
        /// Handles unhandled exceptions that occur on non-UI threads within the current application domain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the exception details.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception currentException = (Exception)e.ExceptionObject;
            string errorMessage = string.Format(System.Globalization.CultureInfo.CurrentCulture, "Non-UI Thread Exception: {0}\n{1}", currentException.Message, currentException.StackTrace);

            MessageBox.Show(errorMessage, "Fatal Domain Error");
        }
    }
}