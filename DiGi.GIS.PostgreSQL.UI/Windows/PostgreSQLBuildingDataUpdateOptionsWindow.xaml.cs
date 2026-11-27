using DiGi.Core;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
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
    /// Interaction logic for PostgreSQLBuildingDataUpdateOptionsWindow.xaml
    /// <para>Asks for the three settings that decide what a run costs and touches - which counties, which kinds of column, and how long a statement may take. The radiuses are carried over from the instance it was given untouched: each one names its own pair of stored columns, so changing them from a dialog would change the shape of the table rather than the numbers in it.</para>
    /// <para>The counties matter most. Left unscoped the run walks every subdivision in the country, which is the pass nobody wants to start by accident while trying one county.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#67), so a run repeated after the tray was restarted is not typed again. Importing fills the controls, and the two members no control shows - <c>Radiuses</c> and <c>Years</c> - travel in the file as the copy holds them, so an imported file can change which columns the run writes; this paragraph is the warning. Nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingDataUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions;

        // The options the controls were last filled from - the window's own copy on opening, the file on an
        // import - so the two members no control shows follow an import instead of staying as typed before it.
        private PostgreSQLBuildingDataUpdateOptions? postgreSQLBuildingDataUpdateOptions_Shown;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingDataUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingDataUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingDataUpdateOptionsWindow(PostgreSQLBuildingDataUpdateOptions? postgreSQLBuildingDataUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingDataUpdateOptions = postgreSQLBuildingDataUpdateOptions is null ? new PostgreSQLBuildingDataUpdateOptions() : new PostgreSQLBuildingDataUpdateOptions(postgreSQLBuildingDataUpdateOptions);

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_UpdateTypes.ItemAdding += ListBoxControl_UpdateTypes_ItemAdding;
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetUpdateTypes();
            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingDataUpdateOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingDataUpdateOptions PostgreSQLBuildingDataUpdateOptions
        {
            get
            {
                return postgreSQLBuildingDataUpdateOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - no update
            // type checked writes an empty set (never "every type"), no county selected writes no county list,
            // and the range rules are OK's gate, not the file's. A timeout box that does not hold a whole number
            // is refused, because it could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLBuildingDataUpdateOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export building data settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "BuildingData_Update_Settings.json"
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
                Title = "Import building data settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // The controls are filled and the two members no control shows are remembered with them, so OK
            // writes what the file held - the table's shape travels with the settings. Nothing runs until OK,
            // and a cancellation still leaves the window's own options untouched.
            SetControls(postgreSQLBuildingDataUpdateOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingDataUpdateOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - at least one update type, at least one county, a timeout of at least zero - and they take the two members no control shows from whatever the controls were last filled from: the options the window was opened with, or the file an import read. A refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingDataUpdateOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuildingDataUpdateOptions.BuildingDataUpdateTypes = options.BuildingDataUpdateTypes;
            postgreSQLBuildingDataUpdateOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingDataUpdateOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLBuildingDataUpdateOptions.Radiuses = options.Radiuses;
            postgreSQLBuildingDataUpdateOptions.Years = options.Years;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied - at least one update type, at least one county, a timeout of at least zero - and finishes with those messages. Export reads the same controls without those requirements, so a half-prepared window can be saved: nothing checked is an empty set (never "every type") and no county is no county list. A timeout box that does not hold a whole number is refused on both paths; a negative timeout exports, to be refused by OK on read-back. The candidate is built on what the controls were last filled from, so the two members with no control - <c>Radiuses</c> and <c>Years</c> - travel in the file exactly as that copy holds them, and after an import OK writes the file's, not the window's.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingDataUpdateOptions? options, out string? message)
        {
            options = null;
            message = null;

            // No selection is written as no county list (null); an empty set would mean "none" and is never
            // handed over - neither by OK nor by an export.
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

            List<BuildingDataUpdateType>? buildingDataUpdateTypes = ListBoxControl_UpdateTypes.GetItems<BuildingDataUpdateType>();
            HashSet<BuildingDataUpdateType>? buildingDataUpdateTypes_Selected = buildingDataUpdateTypes is null || buildingDataUpdateTypes.Count == 0 ? [] : [.. buildingDataUpdateTypes];

            if (complete && buildingDataUpdateTypes_Selected is not { Count: > 0 })
            {
                message = "At least one update type has to be selected.";
                return false;
            }

            if (complete && countyIds is null)
            {
                message = "At least one county has to be selected.";
                return false;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || (complete && commandTimeout < 0))
            {
                message = "Command timeout has to be a whole number of seconds, zero or greater.";
                return false;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were. The
            // base is what the controls were last filled from: on an import that is the file, so its Radiuses
            // and Years are what OK would apply - the table's shape travels with the settings.
            PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions_Temp = new(postgreSQLBuildingDataUpdateOptions_Shown ?? postgreSQLBuildingDataUpdateOptions)
            {
                BuildingDataUpdateTypes = buildingDataUpdateTypes_Selected,
                CountyIds = countyIds,
                CommandTimeout = commandTimeout,
            };

            options = postgreSQLBuildingDataUpdateOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection, options with no update types clear the type list. The options shown are remembered as the
        /// base every later read is built on, which is how the members with no control follow an import. Internal
        /// so a fact can fill a window from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingDataUpdateOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions)
        {
            postgreSQLBuildingDataUpdateOptions_Shown = postgreSQLBuildingDataUpdateOptions;

            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingDataUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            HashSet<BuildingDataUpdateType>? buildingDataUpdateTypes = postgreSQLBuildingDataUpdateOptions.BuildingDataUpdateTypes;
            ListBoxControl_UpdateTypes.SelectItems<BuildingDataUpdateType>(x => buildingDataUpdateTypes is not null && buildingDataUpdateTypes.Contains(x));

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLBuildingDataUpdateOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one building data options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingDataUpdateOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingDataUpdateOptions>? postgreSQLBuildingDataUpdateOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingDataUpdateOptions>(json);
            if (postgreSQLBuildingDataUpdateOptionsList is null || postgreSQLBuildingDataUpdateOptionsList.Count != 1)
            {
                message = "The file holds no building data settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingDataUpdateOptionsList[0];
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

        private void ListBoxControl_UpdateTypes_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not BuildingDataUpdateType buildingDataUpdateType)
            {
                return;
            }

            e.Name = buildingDataUpdateType.Description() ?? buildingDataUpdateType.ToString();
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

            HashSet<int>? countyIds = postgreSQLBuildingDataUpdateOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        private void SetUpdateTypes()
        {
            List<BuildingDataUpdateType> buildingDataUpdateTypes = [.. System.Enum.GetValues<BuildingDataUpdateType>()];

            ListBoxControl_UpdateTypes.SetItems(buildingDataUpdateTypes);

            HashSet<BuildingDataUpdateType>? buildingDataUpdateTypes_Selected = postgreSQLBuildingDataUpdateOptions.BuildingDataUpdateTypes;
            if (buildingDataUpdateTypes_Selected is null || buildingDataUpdateTypes_Selected.Count == 0)
            {
                return;
            }

            ListBoxControl_UpdateTypes.SelectItems<BuildingDataUpdateType>(buildingDataUpdateTypes_Selected.Contains);
        }
    }
}
