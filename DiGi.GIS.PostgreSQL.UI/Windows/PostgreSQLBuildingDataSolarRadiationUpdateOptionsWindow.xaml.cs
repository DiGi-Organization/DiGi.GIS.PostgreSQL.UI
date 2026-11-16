using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingDataSolarRadiationUpdateOptionsWindow.xaml
    /// <para>Asks for what a solar radiation summary run writes: the county parts, the radius the summary is built from, whether rows already carrying a total are stepped over, the batch size and the timeout.</para>
    /// <para>The radius is the field to read twice. A building can carry several variants - the isolated building (0) and one or more neighbour radii (DiGi.GIS.PostgreSQL#107) - and left empty the variant written last wins, building by building.</para>
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

            double? radius = this.postgreSQLBuildingDataSolarRadiationUpdateOptions.Radius;
            TextBoxControl_Radius.Value = radius is double radius_Value ? radius_Value.ToString(CultureInfo.CurrentCulture) : string.Empty;
            CheckBox_SkipCompleted.IsChecked = this.postgreSQLBuildingDataSolarRadiationUpdateOptions.SkipCompleted;
            TextBoxControl_BatchSize.Value = this.postgreSQLBuildingDataSolarRadiationUpdateOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingDataSolarRadiationUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // The task takes null as "every county", so an empty selection is not a safe thing to leave this
                // window with by accident.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Empty is a value of its own here: the latest stored result of any radius.
            double? radius = null;
            if (!string.IsNullOrWhiteSpace(TextBoxControl_Radius.Value))
            {
                if (!TextBoxControl_Radius.TryGetValue(out double radius_Value) || !double.IsFinite(radius_Value) || radius_Value < 0)
                {
                    MessageBox.Show("Radius has to be empty or a number of metres, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                radius = radius_Value;
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

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLBuildingDataSolarRadiationUpdateOptions.CountyIds = countyIds;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.Radius = radius;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.SkipCompleted = CheckBox_SkipCompleted.IsChecked == true;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.BatchSize = batchSize;
            postgreSQLBuildingDataSolarRadiationUpdateOptions.CommandTimeout = commandTimeout;

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

            HashSet<int>? countyIds = postgreSQLBuildingDataSolarRadiationUpdateOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
