using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuilding2DRefreshOptionsWindow.xaml
    /// <para>Asks for what decides the cost and the reach of a Building2D refresh: whether buildings that already carry a <c>subdivision_id</c> are re-derived, whether the run is limited to the counties whose subdivision layer nests, and which counties are walked at all. Unscoped and overriding, the run re-derives every building in the country.</para>
    /// <para>No county selected means every county - the scope the refresh has always had. The nested-layer box names the counties where the previous tie-break produced an arbitrary value (DiGi.GIS.PostgreSQL#77); expect it to exclude very little, because a village and its named parts nest as a city and its districts do. The county list is what makes a trial run small.</para>
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

            // The caption stays in the XAML - the designer does not run this constructor. Only the values come
            // from the options.
            TextBoxControl_BatchSize.Value = this.postgreSQLBuilding2DRefreshOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            CheckBox_OverrideExistingSubdivisionIds.IsChecked = this.postgreSQLBuilding2DRefreshOptions.OverrideExistingSubdivisionIds;
            CheckBox_NestedSubdivisionsOnly.IsChecked = this.postgreSQLBuilding2DRefreshOptions.NestedSubdivisionsOnly;

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
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

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || batchSize < 1)
            {
                MessageBox.Show("Batch size has to be a whole number of buildings, one or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // No selection is "every county", which is what the refresh has always meant - the task takes null
            // as the whole table. An empty set would mean "none" and walk nothing, so it is never handed over.
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

            postgreSQLBuilding2DRefreshOptions.BatchSize = batchSize;
            postgreSQLBuilding2DRefreshOptions.OverrideExistingSubdivisionIds = CheckBox_OverrideExistingSubdivisionIds.IsChecked == true;
            postgreSQLBuilding2DRefreshOptions.NestedSubdivisionsOnly = CheckBox_NestedSubdivisionsOnly.IsChecked == true;
            postgreSQLBuilding2DRefreshOptions.CountyIds = countyIds;

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
