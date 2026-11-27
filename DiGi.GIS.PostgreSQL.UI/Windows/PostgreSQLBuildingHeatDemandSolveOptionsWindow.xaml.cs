using DiGi.Analytical.Building.Regulation.Enums;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Energy.Classes;
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
    /// Interaction logic for PostgreSQLBuildingHeatDemandSolveOptionsWindow.xaml
    /// <para>Asks for everything that decides what a batch heat demand run costs and stores: the county parts, the construction flavour, the radius of the stored solar result the irradiation is taken from, whether buildings already calculated are stepped over, and the batch, ceiling, parallelism and timeout settings (DiGi.GIS.PostgreSQL.UI#49).</para>
    /// <para>The counties matter most. An unscoped run is every building in the country - the window never leaves with no county chosen.</para>
    /// <para>The options the window does not show - the engine options, the neighbour radius and the tile size - are carried through as they were, in the settings file as in the copy.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#72), so a run repeated after the tray was restarted is not typed again. Importing fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingHeatDemandSolveOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingHeatDemandSolveOptions postgreSQLBuildingHeatDemandSolveOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingHeatDemandSolveOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingHeatDemandSolveOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingHeatDemandSolveOptionsWindow(PostgreSQLBuildingHeatDemandSolveOptions? postgreSQLBuildingHeatDemandSolveOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingHeatDemandSolveOptions = postgreSQLBuildingHeatDemandSolveOptions is null ? new PostgreSQLBuildingHeatDemandSolveOptions() : new PostgreSQLBuildingHeatDemandSolveOptions(postgreSQLBuildingHeatDemandSolveOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuildingHeatDemandSolveOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingHeatDemandSolveOptions PostgreSQLBuildingHeatDemandSolveOptions
        {
            get
            {
                return postgreSQLBuildingHeatDemandSolveOptions;
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
            // selected writes no county list, and the range rules are OK's gate, not the file's. What is
            // refused is what could not be written back as typed: a box that does not hold its number, or a
            // buildings-per-part box holding something that is not a whole number.
            if (!TryReadOptions(false, out PostgreSQLBuildingHeatDemandSolveOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export heat demand settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "HeatDemand_Solve_Settings.json"
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
                Title = "Import heat demand settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuildingHeatDemandSolveOptions postgreSQLBuildingHeatDemandSolveOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLBuildingHeatDemandSolveOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine.
            HashSet<int> countyIds = postgreSQLBuildingHeatDemandSolveOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and passes the full set of rules OK has always applied - at least one county, a solar radius of at least zero, a batch size of at least one, a non-negative timeout and parallelism, a buildings-per-part ceiling of at least one when given; a refusal leaves them as they were. The members the window has no control for - the engine options, the neighbour radius, the tile size - keep the values it was opened with.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuildingHeatDemandSolveOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuildingHeatDemandSolveOptions.CountyIds = options.CountyIds;
            postgreSQLBuildingHeatDemandSolveOptions.ConstructionFlavour = options.ConstructionFlavour;
            postgreSQLBuildingHeatDemandSolveOptions.SkipCompleted = options.SkipCompleted;
            postgreSQLBuildingHeatDemandSolveOptions.SolarRadius = options.SolarRadius;
            postgreSQLBuildingHeatDemandSolveOptions.BatchSize = options.BatchSize;
            postgreSQLBuildingHeatDemandSolveOptions.CommandTimeout = options.CommandTimeout;
            postgreSQLBuildingHeatDemandSolveOptions.BuildingCountMax = options.BuildingCountMax;
            postgreSQLBuildingHeatDemandSolveOptions.MaxDegreeOfParallelism = options.MaxDegreeOfParallelism;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) applies every rule the window has always applied, in the order it has always applied them, and finishes with those messages. Export reads the same controls without the range rules, so a half-prepared window can be saved: no county selected is written as no county list, an empty buildings-per-part box as no ceiling, and a negative radius or a batch size below one still exports, to be refused by OK on read-back. What is refused on both paths is what could not be written back as typed: a box that does not hold its number. The candidate is built on a copy of the window's options, so the members with no control - the engine options, the neighbour radius, the tile size - travel in the file exactly as the copy holds them and are never changed by a read.</para>
        /// </summary>
        /// <param name="complete">True for OK: every rule applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuildingHeatDemandSolveOptions? options, out string? message)
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

            if (!TextBoxControl_SolarRadius.TryGetValue(out double solarRadius) || !double.IsFinite(solarRadius) || (complete && solarRadius < 0))
            {
                message = "The solar radius has to be a number of metres, zero or greater.";
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

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || (complete && maxDegreeOfParallelism < 0))
            {
                message = "Parallel buildings has to be a whole number, zero (automatic) or greater.";
                return false;
            }

            // Empty is "every building", not zero - the option is null then. A box holding something that is
            // not a whole number cannot be written back as typed; below one is OK's rule alone.
            int? buildingCountMax = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_BuildingCountMax.Value))
            {
                if (!TextBoxControl_BuildingCountMax.TryGetValue(out int buildingCountMax_Value) || (complete && buildingCountMax_Value < 1))
                {
                    message = "Buildings per part has to be empty or a whole number, 1 or greater.";
                    return false;
                }

                buildingCountMax = buildingCountMax_Value;
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLBuildingHeatDemandSolveOptions postgreSQLBuildingHeatDemandSolveOptions_Temp = new(postgreSQLBuildingHeatDemandSolveOptions)
            {
                CountyIds = countyIds,
                ConstructionFlavour = ComboBox_ConstructionFlavour.SelectedIndex == 1 ? ConstructionFlavour.Regulation : ConstructionFlavour.TypicalAsBuilt,
                SkipCompleted = CheckBox_SkipCompleted.IsChecked == true,
                SolarRadius = solarRadius,
                BatchSize = batchSize,
                CommandTimeout = commandTimeout,
                BuildingCountMax = buildingCountMax,
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
            };

            options = postgreSQLBuildingHeatDemandSolveOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection and options with no ceiling empty the buildings-per-part box. The members this window has no
        /// control for are left alone. Internal so a fact can fill a window from a settings file the way Import
        /// does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLBuildingHeatDemandSolveOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuildingHeatDemandSolveOptions postgreSQLBuildingHeatDemandSolveOptions)
        {
            // The captions stay in the XAML - the designer does not run this constructor. Only the values come from
            // the options. The first item is TypicalAsBuilt, the second Regulation; Undefined is shown as the default.
            ComboBox_ConstructionFlavour.SelectedIndex = postgreSQLBuildingHeatDemandSolveOptions.ConstructionFlavour == ConstructionFlavour.Regulation ? 1 : 0;
            CheckBox_SkipCompleted.IsChecked = postgreSQLBuildingHeatDemandSolveOptions.SkipCompleted;
            TextBoxControl_SolarRadius.Value = postgreSQLBuildingHeatDemandSolveOptions.SolarRadius.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_BatchSize.Value = postgreSQLBuildingHeatDemandSolveOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = postgreSQLBuildingHeatDemandSolveOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_MaxDegreeOfParallelism.Value = postgreSQLBuildingHeatDemandSolveOptions.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            // Empty is "every building", not zero - the option is null then.
            int? buildingCountMax = postgreSQLBuildingHeatDemandSolveOptions.BuildingCountMax;
            TextBoxControl_BuildingCountMax.Value = buildingCountMax is int buildingCountMax_Value ? buildingCountMax_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh on every run. SelectItems deselects the rest, so a file with no
            // county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLBuildingHeatDemandSolveOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one heat demand options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuildingHeatDemandSolveOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuildingHeatDemandSolveOptions>? postgreSQLBuildingHeatDemandSolveOptionsList = Core.Convert.ToDiGi<PostgreSQLBuildingHeatDemandSolveOptions>(json);
            if (postgreSQLBuildingHeatDemandSolveOptionsList is null || postgreSQLBuildingHeatDemandSolveOptionsList.Count != 1)
            {
                message = "The file holds no heat demand settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuildingHeatDemandSolveOptionsList[0];
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

            HashSet<int>? countyIds = postgreSQLBuildingHeatDemandSolveOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier: the options carry identifiers, and the references are read afresh on every
            // run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
