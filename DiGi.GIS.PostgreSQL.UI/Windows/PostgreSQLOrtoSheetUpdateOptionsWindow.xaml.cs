using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLOrtoSheetUpdateOptionsWindow.xaml
    /// <para>Asks for the index years an orthophoto sheet import reads and for its pacing against the GUGiK index service. The whole index (49 years, 1957-2026, 1 337 341 rows on 2026-11-03) took 23 minutes to read; a single year is a quick trial.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#62), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
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

            SetControls(this.postgreSQLOrtoSheetUpdateOptions);
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

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - the year
            // range, the page size floor and the retry floors are OK's gate, not the file's. A value that is
            // there but malformed is still refused, because it could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLOrtoSheetUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export orthophoto sheet index settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "OrtoSheet_Update_Settings.json"
            };

            if (saveFileDialog.ShowDialog(this) != true)
            {
                return;
            }

            string? json = Core.Convert.ToSystem_String(options);
            if (json is null)
            {
                MessageBox.Show(this, "The settings could not be converted to JSON - nothing was written.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                File.WriteAllText(saveFileDialog.FileName, json, Encoding.UTF8);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"The settings could not be written to {saveFileDialog.FileName}: {exception.Message}", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_ImportSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Import orthophoto sheet index settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog(this) != true)
            {
                return;
            }

            string json;
            try
            {
                json = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"The settings could not be read from {openFileDialog.FileName}: {exception.Message}", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLOrtoSheetUpdateOptions postgreSQLOrtoSheetUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Nothing runs until OK: the file fills the controls and OK validates them like typed ones.
            SetControls(postgreSQLOrtoSheetUpdateOptions_Imported);
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (TryGetOptions(out string? message))
            {
                DialogResult = true;
                Close();
                return;
            }

            MessageBox.Show(message ?? "The options cannot start a run.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Reads every control into the window's options and checks them, without closing the window or showing a message - the part of OK a fact can drive, because a message box would hang it.
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLOrtoSheetUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLOrtoSheetUpdateOptions.FirstYear = options.FirstYear;
            postgreSQLOrtoSheetUpdateOptions.LastYear = options.LastYear;
            postgreSQLOrtoSheetUpdateOptions.PageSize = options.PageSize;
            postgreSQLOrtoSheetUpdateOptions.RetryCount = options.RetryCount;
            postgreSQLOrtoSheetUpdateOptions.RetryDelaySeconds = options.RetryDelaySeconds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied - the years from 1900 on with the last not before the first, a page size of at least one row, a non-negative retry count and delay - and finishes with those messages, so the operator is told while the dialog can still be corrected. Export reads the same controls without the range rules, so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a box that does not hold its number, or a retry delay that is not a finite number. A value out of range is refused by OK when it is read back, exactly as when it is typed.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLOrtoSheetUpdateOptions? options, out string? message)
        {
            options = null;
            message = null;

            bool firstYear_Read = TextBoxControl_FirstYear.TryGetValue(out int firstYear);
            bool lastYear_Read = TextBoxControl_LastYear.TryGetValue(out int lastYear);
            bool pageSize_Read = TextBoxControl_PageSize.TryGetValue(out int pageSize);
            bool retryCount_Read = TextBoxControl_RetryCount.TryGetValue(out int retryCount);
            bool retryDelaySeconds_Read = TextBoxControl_RetryDelaySeconds.TryGetValue(out double retryDelaySeconds) && double.IsFinite(retryDelaySeconds);

            // The checks run in the order OK has always run them, so a window failing several rules is told
            // about the first one it shows; the range rules are OK's only.
            if (!firstYear_Read || !lastYear_Read || (complete && (firstYear < 1900 || lastYear < firstYear)))
            {
                message = "The years have to be whole numbers from 1900 on, the last not before the first.";
                return false;
            }

            if (!pageSize_Read || (complete && pageSize < 1))
            {
                message = "Page size has to be a whole number of rows, one or greater.";
                return false;
            }

            if (!retryCount_Read || (complete && retryCount < 0))
            {
                message = "Retry count has to be a whole number, zero or greater.";
                return false;
            }

            if (!retryDelaySeconds_Read || (complete && retryDelaySeconds < 0))
            {
                message = "Retry delay has to be a number of seconds, zero or greater.";
                return false;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLOrtoSheetUpdateOptions postgreSQLOrtoSheetUpdateOptions_Temp = new(postgreSQLOrtoSheetUpdateOptions)
            {
                FirstYear = firstYear,
                LastYear = lastYear,
                PageSize = pageSize,
                RetryCount = retryCount,
                RetryDelaySeconds = retryDelaySeconds,
            };

            options = postgreSQLOrtoSheetUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before. Internal so a fact can fill a window
        /// from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLOrtoSheetUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLOrtoSheetUpdateOptions postgreSQLOrtoSheetUpdateOptions)
        {
            TextBoxControl_FirstYear.Value = postgreSQLOrtoSheetUpdateOptions.FirstYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_LastYear.Value = postgreSQLOrtoSheetUpdateOptions.LastYear.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_PageSize.Value = postgreSQLOrtoSheetUpdateOptions.PageSize.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_RetryCount.Value = postgreSQLOrtoSheetUpdateOptions.RetryCount.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_RetryDelaySeconds.Value = postgreSQLOrtoSheetUpdateOptions.RetryDelaySeconds.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one orthophoto sheet index options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLOrtoSheetUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLOrtoSheetUpdateOptions>? postgreSQLOrtoSheetUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLOrtoSheetUpdateOptions>(json);
            if (postgreSQLOrtoSheetUpdateOptionsList is null || postgreSQLOrtoSheetUpdateOptionsList.Count != 1)
            {
                message = "The file holds no orthophoto sheet index settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLOrtoSheetUpdateOptionsList[0];
        }
    }
}
