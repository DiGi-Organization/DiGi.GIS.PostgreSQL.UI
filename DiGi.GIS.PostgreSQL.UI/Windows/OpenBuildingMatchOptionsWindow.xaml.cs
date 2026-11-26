using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for OpenBuildingMatchOptionsWindow.xaml
    /// <para>Asks what decides a run of the open building matching (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57): which of the five footprint datasets to match to BDOT10k, the scope (a county list or an EPSG:2180 bounding box), the five parameters of the matching rule and whether the links replace the stored snapshot. GHS-OBAT is not offered: it holds centroids and the core task refuses it.</para>
    /// <para>Every dataset checked is stored as "all" (<see cref="PostgreSQLOpenBuildingMatchTaskOptions.DatasetKeys"/> null), so a run started from this window keeps matching every footprint dataset if one is added later. Numbers are read in the invariant culture first and in the current one second, so <c>0.5</c> and <c>0,5</c> both work. What the core task and the rule need is asked through <see cref="Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions?)"/> before OK returns, so the operator is told while the dialog is open.</para>
    /// <para>The settings can be exported to a JSON file and imported again (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#60), so a run repeated after the tray was restarted is not typed again. Importing only fills the controls: nothing runs until OK, and OK validates the imported values like typed ones.</para>
    /// <para>The window works on a copy - the options and their rule parameters - so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class OpenBuildingMatchOptionsWindow : Window
    {
        private readonly PostgreSQLOpenBuildingMatchTaskOptions postgreSQLOpenBuildingMatchTaskOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenBuildingMatchOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLOpenBuildingMatchTaskOptions">The options the controls are filled from; null uses the defaults (every dataset, the proposed rule, links stored).</param>
        /// <param name="administrativeAreal2DReferences">The county parts to choose from, one entry per piece. Nothing is preselected unless the options name parts: the scope is required, and a dialog that opens with every county ticked invites a national run as the default.</param>
        public OpenBuildingMatchOptionsWindow(PostgreSQLOpenBuildingMatchTaskOptions? postgreSQLOpenBuildingMatchTaskOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLOpenBuildingMatchTaskOptions = postgreSQLOpenBuildingMatchTaskOptions is null ? new PostgreSQLOpenBuildingMatchTaskOptions() : new PostgreSQLOpenBuildingMatchTaskOptions(postgreSQLOpenBuildingMatchTaskOptions);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
            SetControls(this.postgreSQLOpenBuildingMatchTaskOptions);

            SetScopeControls();
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLOpenBuildingMatchTaskOptions PostgreSQLOpenBuildingMatchTaskOptions
        {
            get
            {
                return postgreSQLOpenBuildingMatchTaskOptions;
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Defaults_Click(object sender, RoutedEventArgs e)
        {
            SetParameters(new OpenBuildingMatchParameters());
        }

        private void Button_ExportSettings_Click(object sender, RoutedEventArgs e)
        {
            // Not complete: a half-prepared window can be saved and finished after the next import - an export
            // without a scope is written as no scope. A value that is there but malformed is still refused,
            // because it could not be written back as typed.
            if (!TryReadOptions(false, out PostgreSQLOpenBuildingMatchTaskOptions? options, out string? message_Export) || options is null)
            {
                MessageBox.Show(message_Export ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Export open building match settings",
                Filter = "Settings (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                DefaultExt = ".json",
                FileName = "OpenBuilding_Match_Settings.json"
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
                Title = "Import open building match settings",
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

            if (TryReadSettings(json, out string? message_Read) is not PostgreSQLOpenBuildingMatchTaskOptions postgreSQLOpenBuildingMatchTaskOptions_Imported)
            {
                MessageBox.Show(this, message_Read ?? string.Empty, Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetControls(postgreSQLOpenBuildingMatchTaskOptions_Imported);

            // Said rather than left for OK to find: a county the list does not hold cannot be selected, and a key
            // this window offers no check box for is dropped - both are the usual sign the settings came from
            // another machine or another window.
            List<string> notes = [];

            HashSet<int> countyIds = postgreSQLOpenBuildingMatchTaskOptions_Imported.CountyIds ?? [];
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
                    notes.Add(string.Create(CultureInfo.CurrentCulture, $"{countyIds_Missing.Count} county part(s) of the file are not in the list and were not selected: {string.Join(", ", countyIds_Missing)}"));
                }
            }

            // Case as the check boxes read it: a key matches theirs whatever case the file holds it in.
            HashSet<string> datasetKeys_Offered = new(StringComparer.OrdinalIgnoreCase);
            foreach ((CheckBox checkBox, string datasetKey) in DatasetCheckBoxes())
            {
                datasetKeys_Offered.Add(datasetKey);
            }

            List<string> datasetKeys_Unknown = [];
            foreach (string datasetKey in postgreSQLOpenBuildingMatchTaskOptions_Imported.DatasetKeys ?? [])
            {
                if (!datasetKeys_Offered.Contains(datasetKey))
                {
                    datasetKeys_Unknown.Add(datasetKey);
                }
            }

            datasetKeys_Unknown.Sort(StringComparer.Ordinal);
            if (datasetKeys_Unknown.Count != 0)
            {
                notes.Add(string.Create(CultureInfo.CurrentCulture, $"The file names {datasetKeys_Unknown.Count} dataset(s) this window does not offer: {string.Join(", ", datasetKeys_Unknown)} - they were not checked."));
            }

            if (notes.Count != 0)
            {
                MessageBox.Show(this, string.Join(Environment.NewLine + Environment.NewLine, notes), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
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

            MessageBox.Show(this, message ?? "The options cannot start a run.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Reads every control into the window's options and checks them, without closing the window or showing a message - the part of OK a fact can drive, because a message box would hang it.
        /// <para>The options are written only when every control reads and the result passes <see cref="Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions?)"/>; a refusal leaves them as they were.</para>
        /// </summary>
        /// <param name="message">The reason the controls cannot start a run, or null when they can.</param>
        /// <returns>True when the options now hold the controls' values and can start a run.</returns>
        internal bool TryGetOptions(out string? message)
        {
            if (!TryReadOptions(true, out PostgreSQLOpenBuildingMatchTaskOptions? options, out message) || options is null)
            {
                return false;
            }

            postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys = options.DatasetKeys;
            postgreSQLOpenBuildingMatchTaskOptions.OpenBuildingMatchParameters = options.OpenBuildingMatchParameters;
            postgreSQLOpenBuildingMatchTaskOptions.Persist = options.Persist;
            postgreSQLOpenBuildingMatchTaskOptions.CountyIds = options.CountyIds;
            postgreSQLOpenBuildingMatchTaskOptions.BoundingBox2D = options.BoundingBox2D;

            return true;
        }

        /// <summary>
        /// Reads every control into a candidate set of options, writing nothing and showing no message.
        /// <para>Complete (OK) requires at least one dataset and a scope, and finishes with <see cref="Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions?)"/>, so the rule and the scope of the run are checked while the dialog can still be corrected. Export reads the same controls without those requirements - no dataset checked is an empty set, never "all", and an empty scope is left out - so a half-prepared window can be saved; what is still refused is what could not be written back as typed: a rule parameter that is not a finite number and a bounding box with only some of its four coordinates. A file out of the rule's range is refused by OK when it is read back, exactly as when it is typed.</para>
        /// </summary>
        /// <param name="complete">True for OK: a dataset and a scope are required and the whole options set is validated. False for an export: an unfinished window is saved as it stands.</param>
        /// <param name="options">The options the controls describe, or null when they could not be read.</param>
        /// <param name="message">The reason the controls could not be read, or null when they could.</param>
        /// <returns>True when the controls read into options.</returns>
        internal bool TryReadOptions(bool complete, out PostgreSQLOpenBuildingMatchTaskOptions? options, out string? message)
        {
            options = null;
            message = null;

            HashSet<string> datasetKeys = [];
            int count_DatasetCheckBoxes = 0;
            foreach ((CheckBox checkBox, string datasetKey) in DatasetCheckBoxes())
            {
                count_DatasetCheckBoxes++;
                if (checkBox.IsChecked == true)
                {
                    datasetKeys.Add(datasetKey);
                }
            }

            if (datasetKeys.Count == 0 && complete)
            {
                message = "Check at least one dataset to match.";
                return false;
            }

            if (!TryParse(TextBoxControl_IoUThreshold.Value, out double ioUThreshold)
                || !TryParse(TextBoxControl_MinOverlapShare.Value, out double minOverlapShare)
                || !TryParse(TextBoxControl_MinArea.Value, out double minArea)
                || !TryParse(TextBoxControl_MaxCentroidDistance.Value, out double maxCentroidDistance)
                || !TryParse(TextBoxControl_BoundaryMargin.Value, out double boundaryMargin))
            {
                message = "Every rule parameter has to be a number - Defaults puts back the rule as proposed.";
                return false;
            }

            HashSet<int>? countyIds = null;
            BoundingBox2D? boundingBox2D = null;

            if (RadioButton_BoundingBox.IsChecked == true)
            {
                bool blank = string.IsNullOrWhiteSpace(TextBoxControl_X1.Value)
                    && string.IsNullOrWhiteSpace(TextBoxControl_Y1.Value)
                    && string.IsNullOrWhiteSpace(TextBoxControl_X2.Value)
                    && string.IsNullOrWhiteSpace(TextBoxControl_Y2.Value);

                if (blank)
                {
                    if (complete)
                    {
                        message = "The bounding box needs four coordinates: x1, y1, x2, y2.";
                        return false;
                    }

                    // Kept as no scope rather than refused: the box is what the operator means to fill in later.
                }
                else if (!TryParse(TextBoxControl_X1.Value, out double x1) || !TryParse(TextBoxControl_Y1.Value, out double y1) || !TryParse(TextBoxControl_X2.Value, out double x2) || !TryParse(TextBoxControl_Y2.Value, out double y2))
                {
                    message = "The bounding box needs four coordinates: x1, y1, x2, y2.";
                    return false;
                }
                else
                {
                    boundingBox2D = new BoundingBox2D(new Point2D(x1, y1), new Point2D(x2, y2));
                }
            }
            else
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
                if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
                {
                    if (complete)
                    {
                        message = "Select the county parts the run covers, or switch to the bounding box scope.";
                        return false;
                    }
                }
                else
                {
                    countyIds = [];
                    foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                    {
                        countyIds.Add(administrativeAreal2DReference.Id);
                    }
                }
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLOpenBuildingMatchTaskOptions postgreSQLOpenBuildingMatchTaskOptions_Temp = new(postgreSQLOpenBuildingMatchTaskOptions)
            {
                // Every dataset checked is "all": a footprint dataset added later is then matched too.
                DatasetKeys = datasetKeys.Count == count_DatasetCheckBoxes ? null : datasetKeys,
                OpenBuildingMatchParameters = new OpenBuildingMatchParameters(ioUThreshold, minOverlapShare, minArea, maxCentroidDistance, boundaryMargin),
                Persist = CheckBox_Persist.IsChecked == true,
                CountyIds = countyIds,
                BoundingBox2D = boundingBox2D,
            };

            if (complete)
            {
                message = Query.OpenBuildingMatchOptionsMessage(postgreSQLOpenBuildingMatchTaskOptions_Temp);
                if (message is not null)
                {
                    return false;
                }
            }

            options = postgreSQLOpenBuildingMatchTaskOptions_Temp;

            return true;
        }

        /// <summary>
        /// The dataset check boxes with the key each one stands for, in the order they are shown.
        /// </summary>
        /// <returns>The pairs of check box and dataset key.</returns>
        private (CheckBox CheckBox, string DatasetKey)[] DatasetCheckBoxes()
        {
            return
            [
                (CheckBox_EUBUCCO, "eubucco"),
                (CheckBox_Overture, "overture"),
                (CheckBox_OSM, "osm"),
                (CheckBox_DBSM, "dbsm"),
                (CheckBox_Microsoft, "microsoft"),
            ];
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

        private void Scope_Changed(object sender, RoutedEventArgs e)
        {
            SetScopeControls();
        }

        /// <summary>
        /// Fills every control from the options - on opening, and again on an import, so a value the options leave
        /// out clears its control rather than keeping what was typed before. Internal so a fact can fill a window
        /// from a settings file the way Import does, without a file dialog.
        /// </summary>
        /// <param name="postgreSQLOpenBuildingMatchTaskOptions">The options to show.</param>
        internal void SetControls(PostgreSQLOpenBuildingMatchTaskOptions postgreSQLOpenBuildingMatchTaskOptions)
        {
            HashSet<string>? datasetKeys = postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys;
            foreach ((CheckBox checkBox, string datasetKey) in DatasetCheckBoxes())
            {
                checkBox.IsChecked = datasetKeys is null || datasetKeys.Contains(datasetKey) || datasetKeys.Contains(datasetKey.ToUpperInvariant());
            }

            SetParameters(postgreSQLOpenBuildingMatchTaskOptions.OpenBuildingMatchParameters ?? new OpenBuildingMatchParameters());

            CheckBox_Persist.IsChecked = postgreSQLOpenBuildingMatchTaskOptions.Persist;

            // The scope radios carry no IsChecked in the markup: setting it there raises Checked while the window
            // is still being parsed, before the panels the handler switches exist. The state is set here instead,
            // once every control is in place.
            if (postgreSQLOpenBuildingMatchTaskOptions.BoundingBox2D is BoundingBox2D boundingBox2D)
            {
                RadioButton_BoundingBox.IsChecked = true;

                TextBoxControl_X1.Value = boundingBox2D.Min.X.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_Y1.Value = boundingBox2D.Min.Y.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_X2.Value = boundingBox2D.Max.X.ToString(CultureInfo.InvariantCulture);
                TextBoxControl_Y2.Value = boundingBox2D.Max.Y.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                RadioButton_CountyParts.IsChecked = true;

                // Cleared, so coordinates typed before an import do not reappear when the operator switches the
                // scope form by hand.
                TextBoxControl_X1.Value = string.Empty;
                TextBoxControl_Y1.Value = string.Empty;
                TextBoxControl_X2.Value = string.Empty;
                TextBoxControl_Y2.Value = string.Empty;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh every run. Nothing is preselected when the options name no county:
            // the scope is required, and an empty selection is what OK refuses.
            HashSet<int> countyIds = postgreSQLOpenBuildingMatchTaskOptions.CountyIds ?? [];
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        /// <summary>
        /// Shows the five rule parameters in the controls, in the invariant culture.
        /// </summary>
        /// <param name="openBuildingMatchParameters">The parameters to show.</param>
        private void SetParameters(OpenBuildingMatchParameters openBuildingMatchParameters)
        {
            TextBoxControl_IoUThreshold.Value = openBuildingMatchParameters.IoUThreshold.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_MinOverlapShare.Value = openBuildingMatchParameters.MinOverlapShare.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_MinArea.Value = openBuildingMatchParameters.MinArea.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_MaxCentroidDistance.Value = openBuildingMatchParameters.MaxCentroidDistance.ToString(CultureInfo.InvariantCulture);
            TextBoxControl_BoundaryMargin.Value = openBuildingMatchParameters.BoundaryMargin.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Shows the scope form that is checked and hides the other one, so the window never offers a county selection and a box to fill at the same time.
        /// </summary>
        private void SetScopeControls()
        {
            bool county = RadioButton_CountyParts.IsChecked == true;

            Grid_CountyScope.Visibility = county ? Visibility.Visible : Visibility.Collapsed;
            StackPanel_BoundingBox.Visibility = county ? Visibility.Collapsed : Visibility.Visible;
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
                if (administrativeAreal2DReference is not null)
                {
                    administrativeAreal2DReferences_Sorted.Add(administrativeAreal2DReference);
                }
            }

            // By code, so the counties of one voivodeship sit together; by identifier within a code, because the
            // pieces of a multi-part county share their code and their name.
            administrativeAreal2DReferences_Sorted.Sort((x, y) =>
            {
                int result = string.CompareOrdinal(x.Code ?? string.Empty, y.Code ?? string.Empty);

                return result != 0 ? result : x.Id.CompareTo(y.Id);
            });

            ListBoxControl_Counties.SetItems(administrativeAreal2DReferences_Sorted);
        }

        /// <summary>
        /// Reads a number typed in the invariant culture (<c>0.5</c>) or, failing that, in the current one (<c>0,5</c> under <c>pl-PL</c>).
        /// </summary>
        /// <param name="text">The typed text.</param>
        /// <param name="value">The number read, or zero.</param>
        /// <returns>True when the text is a finite number.</returns>
        private static bool TryParse(string? text, out double value)
        {
            string? trimmed = text?.Trim();
            if (string.IsNullOrEmpty(trimmed))
            {
                value = 0;
                return false;
            }

            if (!double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && !double.TryParse(trimmed, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            {
                return false;
            }

            return double.IsFinite(value);
        }

        /// <summary>
        /// Reads the settings an exported file holds. Exactly one open building match options object is accepted: a
        /// file of another window's settings carries a different type name and cannot be read as this one, and a
        /// file holding several objects describes several settings, none of which is this window's.
        /// </summary>
        /// <param name="json">The file content.</param>
        /// <param name="message">The reason nothing was read, or null when exactly one setting was.</param>
        /// <returns>The setting the file holds, or null when it holds none.</returns>
        internal static PostgreSQLOpenBuildingMatchTaskOptions? TryReadSettings(string? json, out string? message)
        {
            List<PostgreSQLOpenBuildingMatchTaskOptions>? postgreSQLOpenBuildingMatchTaskOptionsList = Core.Convert.ToDiGi<PostgreSQLOpenBuildingMatchTaskOptions>(json);
            if (postgreSQLOpenBuildingMatchTaskOptionsList is null || postgreSQLOpenBuildingMatchTaskOptionsList.Count != 1)
            {
                message = "The file holds no open building match settings - nothing was changed.";
                return null;
            }

            message = null;

            return postgreSQLOpenBuildingMatchTaskOptionsList[0];
        }
    }
}
