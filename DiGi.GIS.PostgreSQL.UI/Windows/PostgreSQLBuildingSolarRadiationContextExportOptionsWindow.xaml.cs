using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
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
    /// Interaction logic for PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.xaml
    /// <para>Asks for what the urban-context export writes and where: the county parts (one CSV per part), the stored radii in the column order they are exported, whether only residential buildings are kept, the batch size and timeout, and the output folder (DiGi.GIS.PostgreSQL#136).</para>
    /// <para>The export refuses a run without a folder, so the window proposes <see cref="Query.SolarContextDirectory(string?)"/> when the options carry none, and creates the folder when it is accepted.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#71), so a run repeated after the tray was restarted is not typed again. Importing fills the controls - the radius list included, in the file's order - and nothing runs until OK, which validates the imported values like typed ones and only then creates the output folder.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingSolarRadiationContextExportOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions;

        private readonly List<double> radii = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingSolarRadiationContextExportOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingSolarRadiationContextExportOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(PostgreSQLBuildingSolarRadiationContextExportOptions? postgreSQLBuildingSolarRadiationContextExportOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingSolarRadiationContextExportOptions = postgreSQLBuildingSolarRadiationContextExportOptions is null ? new PostgreSQLBuildingSolarRadiationContextExportOptions() : new PostgreSQLBuildingSolarRadiationContextExportOptions(postgreSQLBuildingSolarRadiationContextExportOptions);

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;
            ListBoxControl_Radii.ItemAdding += ListBoxControl_Radii_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingSolarRadiationContextExportOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingSolarRadiationContextExportOptions PostgreSQLBuildingSolarRadiationContextExportOptions
        {
            get
            {
                return postgreSQLBuildingSolarRadiationContextExportOptions;
            }
        }

        private void Button_AddRadius_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_Radius.TryGetValue(out double radius) || !double.IsFinite(radius) || radius < 0)
            {
                MessageBox.Show("A radius has to be a number of metres, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!radii.Contains(radius))
            {
                radii.Add(radius);
                SetRadii();
            }

            TextBoxControl_Radius.Value = string.Empty;
        }

        private void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new()
            {
                Title = "Select the output folder of the solar urban-context export"
            };

            string? outputDirectory = TextBoxControl_OutputDirectory.Value;
            if (!string.IsNullOrWhiteSpace(outputDirectory) && Directory.Exists(outputDirectory))
            {
                openFolderDialog.InitialDirectory = outputDirectory;
            }

            if (openFolderDialog.ShowDialog(this) != true)
            {
                return;
            }

            TextBoxControl_OutputDirectory.Value = openFolderDialog.FolderName;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - no county
            // and no radius write nothing rather than being refused, the range rules are OK's gate, not the
            // file's, and the output folder is written as typed (it is a machine-specific path; OK keeps
            // checking it on read-back). What is refused is what could not be written back as typed: a batch
            // size or timeout that is not a whole number. Creating the folder stays with OK - an export and an
            // import write files the operator chose, never make directories.
            if (!TryReadOptions(false, out PostgreSQLBuildingSolarRadiationContextExportOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export solar urban-context settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "Solar_Context_Export_Settings.json"
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
                Title = "Import solar urban-context settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Only the controls are filled - the radius list included, in the file's order - and no directory
            // is created: that happens when OK succeeds, never on an import.
            SetControls(postgreSQLBuildingSolarRadiationContextExportOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingSolarRadiationContextExportOptions_Imported.CountyIds ?? [];
            if (countyIds.Count != 0)
            {
                HashSet<int> countyIds_Listed = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>(false) ?? [])
                {
                    countyIds_Listed.Add(administrativeAreal2DReference.Id);
                }

                List<int> countyIds_Missing = [];
                foreach (int countyId in countyIds)
                {
                    if (!countyIds_Listed.Contains(countyId))
                    {
                        countyIds_Missing.Add(countyId);
                    }
                }

                countyIds_Missing.Sort();
                if (countyIds_Missing.Count != 0)
                {
                    MessageBox.Show(this, string.Create(CultureInfo.CurrentCulture, $"{countyIds_Missing.Count} county part(s) of the file are not in the list and were not selected: {string.Join(", ", countyIds_Missing)}"), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            // Read and checked first, folder created second, options written last: a folder that cannot be
            // created leaves the window open with its options exactly as they were, which is what a cancelled
            // dialog after a failure would have to mean anyway.
            if (!TryReadOptions(true, out PostgreSQLBuildingSolarRadiationContextExportOptions? options, out string? message) || options is null)
            {
                MessageBox.Show(message ?? "The options cannot start a run.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Directory.CreateDirectory(options.OutputDirectory!);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "The output folder could not be created: {0}", exception.Message), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetOptions(options);

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Reads every control into the window's options and checks them, without closing the window or showing a message - the part of OK a fact can drive, because a message box would hang it. The filesystem side effect of OK - creating the output folder - stays in the handler, outside every fact.
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - at least one county, at least one radius, a batch size of at least one, a timeout of at least zero, a full output path; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingSolarRadiationContextExportOptions? options, out message) || options is null)
            {
                return false;
            }

            SetOptions(options);

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without those requirements, so a half-prepared window can be saved: no county and no radius write nothing rather than being refused, and the output folder is written as typed - trimmed, blank as no folder - while OK keeps requiring a full path. What is refused on both paths is what could not be written back as typed: a batch size or timeout that is not a whole number. The radius list is read from the window's list field in its current order, which is the column order of the files.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingSolarRadiationContextExportOptions? options, out string? message)
        {
            options = null;
            message = null;

            // No selection is written as no county list (null); the task takes null as "every county" and an
            // empty set as "none", so neither leaves this window by accident.
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();

            HashSet<int>? countyIds = null;
            if (administrativeAreal2DReferences is not null && administrativeAreal2DReferences.Count != 0)
            {
                countyIds = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                {
                    countyIds.Add(administrativeAreal2DReference.Id);
                }
            }

            if (complete && countyIds is null)
            {
                message = "At least one county has to be selected.";
                return false;
            }

            if (complete && radii.Count == 0)
            {
                message = "At least one radius has to be given.";
                return false;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || (complete && batchSize < 1))
            {
                message = "Batch size has to be a whole number, 1 or greater.";
                return false;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || (complete && commandTimeout < 0))
            {
                message = "Command timeout has to be a whole number of seconds, zero or greater.";
                return false;
            }

            // Written as typed, trimmed: OK's own rules - non-blank and fully qualified - stay with it, so a
            // relative or blank folder exports (blank as no folder) and is refused on read-back.
            string? outputDirectory = TextBoxControl_OutputDirectory.Value?.Trim();
            if (string.IsNullOrEmpty(outputDirectory))
            {
                if (complete)
                {
                    message = "The output folder has to be a full path.";
                    return false;
                }

                outputDirectory = null;
            }
            else if (complete && !Path.IsPathFullyQualified(outputDirectory))
            {
                message = "The output folder has to be a full path.";
                return false;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions_Temp = new(postgreSQLBuildingSolarRadiationContextExportOptions)
            {
                CountyIds = countyIds,
                Radii = [.. radii],
                ResidentialOnly = CheckBox_ResidentialOnly.IsChecked == true,
                BatchSize = batchSize,
                CommandTimeout = commandTimeout,
                OutputDirectory = outputDirectory,
            };

            options = postgreSQLBuildingSolarRadiationContextExportOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection, options with no folder show the directory the export proposes, and the radius list is
        /// rebuilt from the file in its order. No directory is created here - only OK does that. Internal so a
        /// fact can fill a window from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingSolarRadiationContextExportOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions)
        {
            CheckBox_ResidentialOnly.IsChecked = postgreSQLBuildingSolarRadiationContextExportOptions.ResidentialOnly;
            TextBoxControl_BatchSize.Value = postgreSQLBuildingSolarRadiationContextExportOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            string? outputDirectory = postgreSQLBuildingSolarRadiationContextExportOptions.OutputDirectory;
            TextBoxControl_OutputDirectory.Value = string.IsNullOrWhiteSpace(outputDirectory) ? Query.SolarContextDirectory() : outputDirectory;

            // Kept in the order given rather than sorted: it is the column order of the files. Values that
            // could never have been added by hand - not finite, negative, a duplicate - are dropped.
            radii.Clear();
            foreach (double radius in postgreSQLBuildingSolarRadiationContextExportOptions.Radii ?? [])
            {
                if (double.IsFinite(radius) && radius >= 0 && !radii.Contains(radius))
                {
                    radii.Add(radius);
                }
            }

            SetRadii();

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Writes read options into the window's options - the commit both OK and a fact-driven read share.
        /// </summary>
        /// <param name="postgreSQLBuildingSolarRadiationContextExportOptions">The options to store.</param>
        private void SetOptions(PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions)
        {
            this.postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds = postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds;
            this.postgreSQLBuildingSolarRadiationContextExportOptions.Radii = postgreSQLBuildingSolarRadiationContextExportOptions.Radii;
            this.postgreSQLBuildingSolarRadiationContextExportOptions.ResidentialOnly = postgreSQLBuildingSolarRadiationContextExportOptions.ResidentialOnly;
            this.postgreSQLBuildingSolarRadiationContextExportOptions.BatchSize = postgreSQLBuildingSolarRadiationContextExportOptions.BatchSize;
            this.postgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout = postgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout;
            this.postgreSQLBuildingSolarRadiationContextExportOptions.OutputDirectory = postgreSQLBuildingSolarRadiationContextExportOptions.OutputDirectory;
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one urban-context export options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingSolarRadiationContextExportOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingSolarRadiationContextExportOptions>? postgreSQLBuildingSolarRadiationContextExportOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingSolarRadiationContextExportOptions>(json);
            if (postgreSQLBuildingSolarRadiationContextExportOptionsList is null || postgreSQLBuildingSolarRadiationContextExportOptionsList.Count != 1)
            {
                message = "The file holds no solar urban-context settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingSolarRadiationContextExportOptionsList[0];
        }

        private void Button_RemoveRadii_Click(object sender, RoutedEventArgs e)
        {
            List<double>? radii_Selected = ListBoxControl_Radii.GetItems<double>();
            if (radii_Selected is null || radii_Selected.Count == 0)
            {
                return;
            }

            radii.RemoveAll(radii_Selected.Contains);
            SetRadii();
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the files are named by, and because two pieces of the same
            // county are told apart by nothing else - they share their code and their name.
            e.Name = $"{administrativeAreal2DReference.Code} {administrativeAreal2DReference.Name} (id {administrativeAreal2DReference.Id})";
        }

        private void ListBoxControl_Radii_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not double radius)
            {
                return;
            }

            e.Name = radius == 0 ? "0 m - isolated building (no neighbours)" : string.Format(CultureInfo.CurrentCulture, "{0} m", radius);
        }

        private void SetCounties(IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            if (administrativeAreal2DReferences is null)
            {
                ListBoxControl_Counties.ClearItems();
                return;
            }

            List<AdministrativeAreal2DReference> administrativeAreal2DReferences_Sorted = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (administrativeAreal2DReference is null)
                {
                    continue;
                }

                administrativeAreal2DReferences_Sorted.Add(administrativeAreal2DReference);
            }

            // By code, so that the counties of one voivodeship sit together; by identifier within a code, because
            // the pieces of a multi-part county share their code and their name.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        private void SetRadii()
        {
            ListBoxControl_Radii.SetItems(radii);

            // The shortfall columns are measured against the isolated building, so without 0 they stay empty.
            Label_RadiiHint.Content = radii.Contains(0) ? string.Empty : "Without 0 m the shortfall columns stay empty";
        }
    }
}
