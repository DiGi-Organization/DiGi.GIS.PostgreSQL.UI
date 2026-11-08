using DiGi.Core;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingDataUpdateOptionsWindow.xaml
    /// <para>Asks for the three settings that decide what a run costs and touches - which counties, which kinds of column, and how long a statement may take. The radiuses are carried over from the instance it was given untouched: each one names its own pair of stored columns, so changing them from a dialog would change the shape of the table rather than the numbers in it.</para>
    /// <para>The counties matter most. Left unscoped the run walks every subdivision in the country, which is the pass nobody wants to start by accident while trying one county.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingDataUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingDataUpdateOptions postgreSQLBuildingDataUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingDataUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingDataUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingDataUpdateOptionsWindow(PostgreSQLBuildingDataUpdateOptions? postgreSQLBuildingDataUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingDataUpdateOptions = postgreSQLBuildingDataUpdateOptions is null ? new PostgreSQLBuildingDataUpdateOptions() : new PostgreSQLBuildingDataUpdateOptions(postgreSQLBuildingDataUpdateOptions);

            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingDataUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_UpdateTypes.ItemAdding += ListBoxControl_UpdateTypes_ItemAdding;
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetUpdateTypes();
            SetCounties(administrativeAreal2DReferences);
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            List<BuildingDataUpdateType>? buildingDataUpdateTypes = ListBoxControl_UpdateTypes.GetItems<BuildingDataUpdateType>();
            if (buildingDataUpdateTypes is null || buildingDataUpdateTypes.Count == 0)
            {
                MessageBox.Show("At least one update type has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = ListBoxControl_Counties.GetItems<AdministrativeAreal2DReference>();
            if (administrativeAreal2DReferences is null || administrativeAreal2DReferences.Count == 0)
            {
                // An empty set is not the same as no set: the task takes null as "every county" and an empty set
                // as "none", so neither is a safe thing to leave this window with by accident.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
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

            postgreSQLBuildingDataUpdateOptions.BuildingDataUpdateTypes = [.. buildingDataUpdateTypes];
            postgreSQLBuildingDataUpdateOptions.CountyIds = countyIds;
            postgreSQLBuildingDataUpdateOptions.CommandTimeout = commandTimeout;

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
