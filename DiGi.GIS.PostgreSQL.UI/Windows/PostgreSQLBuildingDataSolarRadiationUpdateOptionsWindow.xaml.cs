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
    /// Interaction logic for PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.xaml
    /// <para>Asks for what a solar radiation summary run writes: the county parts, the radius the summary is built from, whether rows already carrying a total are stepped over, the batch size and the timeout.</para>
    /// <para>The radius is the field to read twice. A building can carry several variants - the isolated building (0) and one or more neighbour radii (DiGi.GIS.PostgreSQL#107) - and left empty the variant written last wins, building by building.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#66), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingDataSolarRadiationUpdateOptions postgreSQLBuildingDataSolarRadiationUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingDataSolarRadiationUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow(PostgreSQLBuildingDataSolarRadiationUpdateOptions? postgreSQLBuildingDataSolarRadiationUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingDataSolarRadiationUpdateOptions = postgreSQLBuildingDataSolarRadiationUpdateOptions is null ? new PostgreSQLBuildingDataSolarRadiationUpdateOptions() : new PostgreSQLBuildingDataSolarRadiationUpdateOptions(postgreSQLBuildingDataSolarRadiationUpdateOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingDataSolarRadiationUpdateOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingDataSolarRadiationUpdateOptions PostgreSQLBuildingDataSolarRadiationUpdateOptions
        {
            get
            {
                return postgreSQLBuildingDataSolarRadiationUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - no county
            // selected writes no county list, an empty radius writes no radius, and the range rules are OK's
            // gate, not the file's. What is refused is what could not be written back as typed: a radius box
            // that does not hold a finite number, a batch size or timeout that is not a whole number.
            if (!TryReadOptions(false, out PostgreSQLBuildingDataSolarRadiationUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export building data solar summary settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "BuildingData_Solar_Summary_Settings.json"
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
                Title = "Import building data solar summary settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingDataSolarRadiationUpdateOptions postgreSQLBuildingDataSolarRadiationUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLBuildingDataSolarRadiationUpdateOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingDataSolarRadiationUpdateOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - at least one county, a radius of at least zero when given, a batch size of at least one, a timeout of at least zero; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingDataSolarRadiationUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuildingDataSolarRadiationUpdateOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.Radius = options.Radius;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.SkipCompleted = options.SkipCompleted;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.BatchSize = options.BatchSize;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.CommandTimeout = options.CommandTimeout;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without the range rules, so a half-prepared window can be saved: no county is written as no county list, an empty radius box as no radius - the latest stored result of any radius is a value of its own - and a radius below zero or a batch size below one still exports, to be refused by OK on read-back. What is refused on both paths is what could not be written back as typed: a radius box that does not hold a finite number, a batch size or timeout that is not a whole number.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingDataSolarRadiationUpdateOptions? options, out string? message)
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

            // Empty is a value of its own here: the latest stored result of any radius.
            double? radius = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_Radius.Value))
            {
                if (!TextBoxControl_Radius.TryGetValue(out double radius_Value) || !double.IsFinite(radius_Value) || (complete && radius_Value < 0))
                {
                    message = "Radius has to be empty or a number of metres, zero or greater.";
                    return false;
                }

                radius = radius_Value;
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

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLBuildingDataSolarRadiationUpdateOptions postgreSQLBuildingDataSolarRadiationUpdateOptions_Temp = new(postgreSQLBuildingDataSolarRadiationUpdateOptions)
            {
                CountyIds = countyIds,
                Radius = radius,
                SkipCompleted = CheckBox_SkipCompleted.IsChecked == true,
                BatchSize = batchSize,
                CommandTimeout = commandTimeout,
            };

            options = postgreSQLBuildingDataSolarRadiationUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection and options with no radius empty the radius box. Internal so a fact can fill a window from a
        /// settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingDataSolarRadiationUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingDataSolarRadiationUpdateOptions postgreSQLBuildingDataSolarRadiationUpdateOptions)
        {
            double? radius = postgreSQLBuildingDataSolarRadiationUpdateOptions.Radius;
            TextBoxControl_Radius.Value = radius is double radius_Value ? radius_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;
            CheckBox_SkipCompleted.IsChecked = postgreSQLBuildingDataSolarRadiationUpdateOptions.SkipCompleted;
            TextBoxControl_BatchSize.Value = postgreSQLBuildingDataSolarRadiationUpdateOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingDataSolarRadiationUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLBuildingDataSolarRadiationUpdateOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one building data solar summary options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingDataSolarRadiationUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingDataSolarRadiationUpdateOptions>? postgreSQLBuildingDataSolarRadiationUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingDataSolarRadiationUpdateOptions>(json);
            if (postgreSQLBuildingDataSolarRadiationUpdateOptionsList is null || postgreSQLBuildingDataSolarRadiationUpdateOptionsList.Count != 1)
            {
                message = "The file holds no building data solar summary settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingDataSolarRadiationUpdateOptionsList[0];
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the run is keyed by, and because two pieces of the same
            // county are told apart by nothing else - they share their code and their name.
            e.Name = $"{administrativeAreal2DReference.Code} {administrativeAreal2DReference.Name} (id {administrativeAreal2DReference.Id})";
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

            HashSet<int>? countyIds = postgreSQLBuildingDataSolarRadiationUpdateOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
