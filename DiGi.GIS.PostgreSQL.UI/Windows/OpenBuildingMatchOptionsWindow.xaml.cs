using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for OpenBuildingMatchOptionsWindow.xaml
    /// <para>Asks what decides a run of the open building matching (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#57): which of the five footprint datasets to match to BDOT10k, the scope (a county list or an EPSG:2180 bounding box), the five parameters of the matching rule and whether the links replace the stored snapshot. GHS-OBAT is not offered: it holds centroids and the core task refuses it.</para>
    /// <para>Every dataset checked is stored as "all" (<see cref="PostgreSQLOpenBuildingMatchTaskOptions.DatasetKeys"/> null), so a run started from this window keeps matching every footprint dataset if one is added later. Numbers are read in the invariant culture first and in the current one second, so <c>0.5</c> and <c>0,5</c> both work. What the core task and the rule need is asked through <see cref="Query.OpenBuildingMatchOptionsMessage(PostgreSQLOpenBuildingMatchTaskOptions?)"/> before OK returns, so the operator is told while the dialog is open.</para>
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

            HashSet<string>? datasetKeys = this.postgreSQLOpenBuildingMatchTaskOptions.DatasetKeys;
            foreach ((CheckBox checkBox, string datasetKey) in DatasetCheckBoxes())
            {
                checkBox.IsChecked = datasetKeys is null || datasetKeys.Contains(datasetKey) || datasetKeys.Contains(datasetKey.ToUpperInvariant());
            }

            SetParameters(this.postgreSQLOpenBuildingMatchTaskOptions.OpenBuildingMatchParameters ?? new OpenBuildingMatchParameters());

            CheckBox_Persist.IsChecked = this.postgreSQLOpenBuildingMatchTaskOptions.Persist;

            // The scope radios carry no IsChecked in the markup: setting it there raises Checked while the window
            // is still being parsed, before the panels the handler switches exist.
            if (this.postgreSQLOpenBuildingMatchTaskOptions.BoundingBox2D is BoundingBox2D boundingBox2D)
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
            }

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);

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

            if (datasetKeys.Count == 0)
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
                if (!TryParse(TextBoxControl_X1.Value, out double x1) || !TryParse(TextBoxControl_Y1.Value, out double y1) || !TryParse(TextBoxControl_X2.Value, out double x2) || !TryParse(TextBoxControl_Y2.Value, out double y2))
                {
                    message = "The bounding box needs four coordinates: x1, y1, x2, y2.";
                    return false;
                }

                boundingBox2D = new BoundingBox2D(new Point2D(x1, y1), new Point2D(x2, y2));
            }
            else
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
                if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
                {
                    message = "Select the county parts the run covers, or switch to the bounding box scope.";
                    return false;
                }

                countyIds = [];
                foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
                {
                    countyIds.Add(administrativeAreal2DReference.Id);
                }
            }

            // Checked on a candidate first, so a refusal leaves the window's options exactly as they were.
            PostgreSQLOpenBuildingMatchTaskOptions options = new(postgreSQLOpenBuildingMatchTaskOptions)
            {
                // Every dataset checked is "all": a footprint dataset added later is then matched too.
                DatasetKeys = datasetKeys.Count == count_DatasetCheckBoxes ? null : datasetKeys,
                OpenBuildingMatchParameters = new OpenBuildingMatchParameters(ioUThreshold, minOverlapShare, minArea, maxCentroidDistance, boundaryMargin),
                Persist = CheckBox_Persist.IsChecked == true,
                CountyIds = countyIds,
                BoundingBox2D = boundingBox2D,
            };

            message = Query.OpenBuildingMatchOptionsMessage(options);
            if (message is not null)
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

            HashSet<int>? countyIds = postgreSQLOpenBuildingMatchTaskOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            // Matched on the identifier: the options carry identifiers, the references are read afresh every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
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
    }
}
