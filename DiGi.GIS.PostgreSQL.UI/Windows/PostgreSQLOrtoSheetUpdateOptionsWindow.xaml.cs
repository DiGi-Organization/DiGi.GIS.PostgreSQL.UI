using DiGi.GIS.PostgreSQL.Classes;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLOrtoSheetUpdateOptionsWindow.xaml
    /// <para>Asks for the index years an orthophoto sheet import reads and for its pacing against the GUGiK index service. The whole index (49 years, 1957-2026, 1 337 341 rows on 2026-11-03) took 23 minutes to read; a single year is a quick trial.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLOrtoSheetUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLOrtoSheetUpdateOptions postgreSQLOrtoSheetUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLOrtoSheetUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLOrtoSheetUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        public PostgreSQLOrtoSheetUpdateOptionsWindow(PostgreSQLOrtoSheetUpdateOptions? postgreSQLOrtoSheetUpdateOptions)
        {
            InitializeComponent();

            this.postgreSQLOrtoSheetUpdateOptions = postgreSQLOrtoSheetUpdateOptions is null ? new PostgreSQLOrtoSheetUpdateOptions() : new PostgreSQLOrtoSheetUpdateOptions(postgreSQLOrtoSheetUpdateOptions);

            TextBoxControl_FirstYear.Value = this.postgreSQLOrtoSheetUpdateOptions.FirstYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_LastYear.Value = this.postgreSQLOrtoSheetUpdateOptions.LastYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_PageSize.Value = this.postgreSQLOrtoSheetUpdateOptions.PageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_RetryCount.Value = this.postgreSQLOrtoSheetUpdateOptions.RetryCount.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_RetryDelaySeconds.Value = this.postgreSQLOrtoSheetUpdateOptions.RetryDelaySeconds.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLOrtoSheetUpdateOptions PostgreSQLOrtoSheetUpdateOptions
        {
            get
            {
                return postgreSQLOrtoSheetUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_FirstYear.TryGetValue(out int firstYear) || !TextBoxControl_LastYear.TryGetValue(out int lastYear) || firstYear < 1900 || lastYear < firstYear)
            {
                MessageBox.Show("The years have to be whole numbers from 1900 on, the last not before the first.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_PageSize.TryGetValue(out int pageSize) || pageSize < 1)
            {
                MessageBox.Show("Page size has to be a whole number of rows, one or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_RetryCount.TryGetValue(out int retryCount) || retryCount < 0)
            {
                MessageBox.Show("Retry count has to be a whole number, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_RetryDelaySeconds.TryGetValue(out double retryDelaySeconds) || double.IsNaN(retryDelaySeconds) || retryDelaySeconds < 0)
            {
                MessageBox.Show("Retry delay has to be a number of seconds, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            postgreSQLOrtoSheetUpdateOptions.FirstYear = firstYear;
            postgreSQLOrtoSheetUpdateOptions.LastYear = lastYear;
            postgreSQLOrtoSheetUpdateOptions.PageSize = pageSize;
            postgreSQLOrtoSheetUpdateOptions.RetryCount = retryCount;
            postgreSQLOrtoSheetUpdateOptions.RetryDelaySeconds = retryDelaySeconds;

            DialogResult = true;
            Close();
        }
    }
}
