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
    /// Interaction logic for PostgreSQLBuilding2DRefreshOptionsWindow.xaml
    /// <para>Asks for what decides the cost and the reach of a Building2D refresh: whether buildings that already carry a <c>subdivision_id</c> are re-derived, whether the run is limited to the counties whose subdivision layer nests, and which counties are walked at all. Unscoped and overriding, the run re-derives every building in the country.</para>
    /// <para>No county selected means every county - the scope the refresh has always had. The nested-layer box names the counties where the previous tie-break produced an arbitrary value (DiGi.GIS.PostgreSQL#77); expect it to exclude very little, because a village and its named parts nest as a city and its districts do. The county list is what makes a trial run small.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#69), so a run repeated after the tray was restarted is not typed again. Importing fills the controls; the members with no control - the keyset anchor and the tolerance - travel in the file as the copy holds them. Nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuilding2DRefreshOptionsWindow : Window
    {
        private readonly PostgreSQLBuilding2DRefreshOptions postgreSQLBuilding2DRefreshOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuilding2DRefreshOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuilding2DRefreshOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuilding2DRefreshOptionsWindow(PostgreSQLBuilding2DRefreshOptions? postgreSQLBuilding2DRefreshOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuilding2DRefreshOptions = postgreSQLBuilding2DRefreshOptions is null ? new PostgreSQLBuilding2DRefreshOptions() : new PostgreSQLBuilding2DRefreshOptions(postgreSQLBuilding2DRefreshOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLBuilding2DRefreshOptions);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuilding2DRefreshOptions PostgreSQLBuilding2DRefreshOptions
        {
            get
            {
                return postgreSQLBuilding2DRefreshOptions;
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
            // selected writes no county list (the refresh's own scope), and the batch size floor is OK's gate,
            // not the file's. A batch size box that does not hold a whole number is still refused, because it
            // could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLBuilding2DRefreshOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export Building2D refresh settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "Building2D_Refresh_Settings.json"
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
                Title = "Import Building2D refresh settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLBuilding2DRefreshOptions postgreSQLBuilding2DRefreshOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLBuilding2DRefreshOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected - the
            // usual sign the settings came from another machine. A file with no county list leaves the
            // selection empty, which is this window's every-county scope and passes OK as it always has.
            HashSet<int> countyIds = postgreSQLBuilding2DRefreshOptions_Imported.CountyIds ?? [];
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
        /// <para>The options are written only when every control reads and the batch size is at least one; a refusal leaves them as they were. The members the window has no control for - the keyset anchor and the tolerance - keep the values it was opened with.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLBuilding2DRefreshOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLBuilding2DRefreshOptions.BatchSize = options.BatchSize;
            postgreSQLBuilding2DRefreshOptions.OverrideExistingSubdivisionIds = options.OverrideExistingSubdivisionIds;
            postgreSQLBuilding2DRefreshOptions.NestedSubdivisionsOnly = options.NestedSubdivisionsOnly;
            postgreSQLBuilding2DRefreshOptions.CountyIds = options.CountyIds;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) requires a batch size of at least one and finishes with the message OK has always given; Export reads the same controls without that requirement, so a half-prepared window can be saved. No county selected is written as no county list (<c>null</c>, which the task reads as every county - the scope the refresh has always had) and never as an empty set, on both paths: unlike the external components window, the empty scope here is legal and passes OK as it always has. A batch size box that does not hold a whole number is refused on both paths. The candidate is built on a copy of the window's options, so the members with no control - the keyset anchor and the tolerance - travel in the file exactly as the copy holds them and are never changed by a read.</para>
        /// </summary>
        /// <param name="complete">True for OK: the batch size range applies. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLBuilding2DRefreshOptions? options, out string? message)
        {
            options = null;
            message = null;

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || (complete && batchSize < 1))
            {
                message = "Batch size has to be a whole number of buildings, one or greater.";
                return false;
            }

            // No selection is "every county", which is what the refresh has always meant - the task takes null
            // as the whole table. An empty set would mean "none" and walk nothing, so it is never handed over -
            // neither by OK nor by an export.
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
            PostgreSQLBuilding2DRefreshOptions postgreSQLBuilding2DRefreshOptions_Temp = new(postgreSQLBuilding2DRefreshOptions)
            {
                BatchSize = batchSize,
                OverrideExistingSubdivisionIds = CheckBox_OverrideExistingSubdivisionIds.IsChecked == true,
                NestedSubdivisionsOnly = CheckBox_NestedSubdivisionsOnly.IsChecked == true,
                CountyIds = countyIds,
            };

            options = postgreSQLBuilding2DRefreshOptions_Temp;

            return true;
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before: options with no county list clear the
        /// selection, which is this window's every-county scope. The members this window has no control for are
        /// left alone. Internal so a fact can fill a window from a settings file the way Import does, without a
        /// file dialog.
        /// </summary>
        /// <param name="postgreSQLBuilding2DRefreshOptions">The options to show.</param>
        internal void SetControls(PostgreSQLBuilding2DRefreshOptions postgreSQLBuilding2DRefreshOptions)
        {
            // The caption stays in the XAML - the designer does not run this constructor. Only the values come
            // from the options.
            TextBoxControl_BatchSize.Value = postgreSQLBuilding2DRefreshOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            CheckBox_OverrideExistingSubdivisionIds.IsChecked = postgreSQLBuilding2DRefreshOptions.OverrideExistingSubdivisionIds;
            CheckBox_NestedSubdivisionsOnly.IsChecked = postgreSQLBuilding2DRefreshOptions.NestedSubdivisionsOnly;

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run. SelectItems deselects the rest,
            // so a file with no county list clears whatever was selected before.
            HashSet<int> countyIds = postgreSQLBuilding2DRefreshOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one Building2D refresh options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLBuilding2DRefreshOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLBuilding2DRefreshOptions>? postgreSQLBuilding2DRefreshOptionsList = Core.Convert.ToDiGi<PostgreSQLBuilding2DRefreshOptions>(json);
            if (postgreSQLBuilding2DRefreshOptionsList is null || postgreSQLBuilding2DRefreshOptionsList.Count != 1)
            {
                message = "The file holds no Building2D refresh settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLBuilding2DRefreshOptionsList[0];
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

            HashSet<int>? countyIds = postgreSQLBuilding2DRefreshOptions.CountyIds;
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
