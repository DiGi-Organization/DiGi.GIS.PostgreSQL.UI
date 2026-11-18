using DiGi.Analytical.Building.Regulation.Enums;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Energy.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingHeatDemandSolveOptionsWindow.xaml
    /// <para>Asks for everything that decides what a batch heat demand run costs and stores: the county parts, the construction flavour, the radius of the stored solar result the irradiation is taken from, whether buildings already calculated are stepped over, and the batch, ceiling, parallelism and timeout settings (DiGi.GIS.PostgreSQL.UI#49).</para>
    /// <para>The counties matter most. An unscoped run is every building in the country - the window never leaves with no county chosen.</para>
    /// <para>The options the window does not show - the engine options, the neighbour radius and the tile size - are carried through as they were. The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
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

            // The captions stay in the XAML - the designer does not run this constructor. Only the values come from
            // the options. The first item is TypicalAsBuilt, the second Regulation; Undefined is shown as the default.
            ComboBox_ConstructionFlavour.SelectedIndex = this.postgreSQLBuildingHeatDemandSolveOptions.ConstructionFlavour == ConstructionFlavour.Regulation ? 1 : 0;
            CheckBox_SkipCompleted.IsChecked = this.postgreSQLBuildingHeatDemandSolveOptions.SkipCompleted;
            TextBoxControl_SolarRadius.Value = this.postgreSQLBuildingHeatDemandSolveOptions.SolarRadius.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_BatchSize.Value = this.postgreSQLBuildingHeatDemandSolveOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingHeatDemandSolveOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_MaxDegreeOfParallelism.Value = this.postgreSQLBuildingHeatDemandSolveOptions.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            // Empty is "every building", not zero - the option is null then.
            int? buildingCountMax = this.postgreSQLBuildingHeatDemandSolveOptions.BuildingCountMax;
            TextBoxControl_BuildingCountMax.Value = buildingCountMax is int buildingCountMax_Value ? buildingCountMax_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // The task takes null as "every county" and an empty set as "none", so neither is a safe thing to
                // leave this window with by accident.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_SolarRadius.TryGetValue(out double solarRadius) || !double.IsFinite(solarRadius) || solarRadius < 0)
            {
                MessageBox.Show("The solar radius has to be a number of metres, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || batchSize < 1)
            {
                MessageBox.Show("Batch size has to be a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || commandTimeout < 0)
            {
                MessageBox.Show("Command timeout has to be a whole number of seconds, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || maxDegreeOfParallelism < 0)
            {
                MessageBox.Show("Parallel buildings has to be a whole number, zero (automatic) or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? buildingCountMax = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_BuildingCountMax.Value))
            {
                if (!TextBoxControl_BuildingCountMax.TryGetValue(out int buildingCountMax_Value) || buildingCountMax_Value < 1)
                {
                    MessageBox.Show("Buildings per part has to be empty or a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                buildingCountMax = buildingCountMax_Value;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLBuildingHeatDemandSolveOptions.CountyIds = countyIds;
            postgreSQLBuildingHeatDemandSolveOptions.ConstructionFlavour = ComboBox_ConstructionFlavour.SelectedIndex == 1 ? ConstructionFlavour.Regulation : ConstructionFlavour.TypicalAsBuilt;
            postgreSQLBuildingHeatDemandSolveOptions.SkipCompleted = CheckBox_SkipCompleted.IsChecked == true;
            postgreSQLBuildingHeatDemandSolveOptions.SolarRadius = solarRadius;
            postgreSQLBuildingHeatDemandSolveOptions.BatchSize = batchSize;
            postgreSQLBuildingHeatDemandSolveOptions.CommandTimeout = commandTimeout;
            postgreSQLBuildingHeatDemandSolveOptions.BuildingCountMax = buildingCountMax;
            postgreSQLBuildingHeatDemandSolveOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;

            DialogResult = true;
            Close();
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
