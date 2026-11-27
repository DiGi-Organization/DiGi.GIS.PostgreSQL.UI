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
    /// Interaction logic for PostgreSQLUpdateOccupancyOptionsWindow.xaml
    /// <para>Asks which side of the occupancy update runs - the administrative roll-up, the per-building distribution, or both - whether the stored rows are cleared first, and which counties the building side is limited to. The roll-up is a sum over the whole hierarchy and stays nationwide whatever counties are selected.</para>
    /// <para>No county selected means every county, which is what the task has always meant; with counties selected a clear removes only their buildings' rows, so the rest of the table is left as it stands.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#63), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLUpdateOccupancyOptionsWindow : Window
    {
        private readonly PostgreSQLUpdateOccupancyOptions postgreSQLUpdateOccupancyOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLUpdateOccupancyOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLUpdateOccupancyOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLUpdateOccupancyOptionsWindow(PostgreSQLUpdateOccupancyOptions? postgreSQLUpdateOccupancyOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLUpdateOccupancyOptions = postgreSQLUpdateOccupancyOptions is null ? new PostgreSQLUpdateOccupancyOptions() : new PostgreSQLUpdateOccupancyOptions(postgreSQLUpdateOccupancyOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLUpdateOccupancyOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLUpdateOccupancyOptions PostgreSQLUpdateOccupancyOptions
        {
            get
            {
                return postgreSQLUpdateOccupancyOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - a window
            // with neither side checked is written as it stands, and no county selection is written as no
            // county list. The window holds no free numbers, so there is no value that could not be written
            // back as typed; "At least one side has to be selected" stays OK's gate.
            if (!TryReadOptions(false, out PostgreSQLUpdateOccupancyOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export occupancy settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "Occupancy_Update_Settings.json"
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
                Title = "Import occupancy settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLUpdateOccupancyOptions postgreSQLUpdateOccupancyOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLUpdateOccupancyOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLUpdateOccupancyOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and at least one side is selected; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLUpdateOccupancyOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLUpdateOccupancyOptions.IncludeAdministrativeAreal2Ds = options.IncludeAdministrativeAreal2Ds;
            postgreSQLUpdateOccupancyOptions.IncludeBuilding2Ds = options.IncludeBuilding2Ds;
            postgreSQLUpdateOccupancyOptions.Clear = options.Clear;
            postgreSQLUpdateOccupancyOptions.CountyIds = options.CountyIds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) requires at least one side to be selected. Export reads the same controls without that requirement - a window with neither side checked is saved as it stands - because a half-prepared window can be finished after the next import. This window holds no free numbers, so an export never fails on a value: what it writes is always what could be written back as typed. No county selected is written as no county list (<c>null</c>, which the task reads as every county), never as an empty set, on both paths.</para>
        /// </summary>
        /// <param name="complete">True for OK: at least one side has to be selected. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLUpdateOccupancyOptions? options, out string? message)
        {
            options = null;
            message = null;

            bool includeAdministrativeAreal2Ds = CheckBox_IncludeAdministrativeAreal2Ds.IsChecked == true;
            bool includeBuilding2Ds = CheckBox_IncludeBuilding2Ds.IsChecked == true;

            if (complete && !includeAdministrativeAreal2Ds && !includeBuilding2Ds)
            {
                message = "At least one side has to be selected.";
                return false;
            }

            // No selection is "every county" - the task takes null as the whole table. An empty set would mean
            // "none" and write nothing, so it is never handed over - neither by OK nor by an export.
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

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLUpdateOccupancyOptions postgreSQLUpdateOccupancyOptions_Temp = new(postgreSQLUpdateOccupancyOptions)
            {
                IncludeAdministrativeAreal2Ds = includeAdministrativeAreal2Ds,
                IncludeBuilding2Ds = includeBuilding2Ds,
                Clear = CheckBox_Clear.IsChecked == true,
                CountyIds = countyIds,
            };

            options = postgreSQLUpdateOccupancyOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection, which is this window's every-county state. Internal so a fact can fill a window from a
        /// settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLUpdateOccupancyOptions">The options to show.</param>
        internal void SetControls(PostgreSQLUpdateOccupancyOptions postgreSQLUpdateOccupancyOptions)
        {
            CheckBox_IncludeAdministrativeAreal2Ds.IsChecked = postgreSQLUpdateOccupancyOptions.IncludeAdministrativeAreal2Ds;
            CheckBox_IncludeBuilding2Ds.IsChecked = postgreSQLUpdateOccupancyOptions.IncludeBuilding2Ds;
            CheckBox_Clear.IsChecked = postgreSQLUpdateOccupancyOptions.Clear;

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLUpdateOccupancyOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one occupancy options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLUpdateOccupancyOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLUpdateOccupancyOptions>? postgreSQLUpdateOccupancyOptionsList = Core.Convert.ToDiGi<PostgreSQLUpdateOccupancyOptions>(json);
            if (postgreSQLUpdateOccupancyOptionsList is null || postgreSQLUpdateOccupancyOptionsList.Count != 1)
            {
                message = "The file holds no occupancy settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLUpdateOccupancyOptionsList[0];
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

            // By code, so that the counties of one voivodeship sit together; by identifier within a code,
            // because the pieces of a multi-part county share their code and their name and the identifier is
            // all that separates them.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);

            HashSet<int>? countyIds = postgreSQLUpdateOccupancyOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
