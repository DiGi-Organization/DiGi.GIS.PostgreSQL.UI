using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow.xaml
    /// <para>Asks for the settings that decide what a run touches and how long it takes - which counties, the command timeout, the batch size, whether finished buildings are skipped, whether party walls are measured, and how many batches of a county run at once. The run reads the stored building models of the counties in scope, so the counties decide its reach and its cost; left unscoped it walks every county part in the country.</para>
    /// <para>Every county is selected by default, so opening the dialog confirms the national scope rather than inviting it; deselecting everything is still refused at OK, because an empty selection is not the same as no selection and neither is a safe thing to leave this window with by accident.</para>
    /// <para>Party walls decide most of the cost: with them the run also reads the outlines and the stored models of the neighbours of every batch, without them it only classifies the models of the counties in scope and leaves the party wall columns exactly as they were. The box starts from the options, so it is checked unless a run was configured otherwise.</para>
    /// <para>The parallel batches decide how many batches of one county are read, classified and written at once - 0 takes half of the logical processors, 1 is the sequential run. The stored rows and the counters do not depend on it, but the memory of the run grows with that number times the batch size, so it is worth no more than the host can hold.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingDataExternalComponentsUpdateOptions postgreSQLBuildingDataExternalComponentsUpdateOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingDataExternalComponentsUpdateOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own. All of them are selected when the options carry no county set.</param>
        public PostgreSQLBuildingDataExternalComponentsUpdateOptionsWindow(PostgreSQLBuildingDataExternalComponentsUpdateOptions? postgreSQLBuildingDataExternalComponentsUpdateOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingDataExternalComponentsUpdateOptions = postgreSQLBuildingDataExternalComponentsUpdateOptions is null ? new PostgreSQLBuildingDataExternalComponentsUpdateOptions() : new PostgreSQLBuildingDataExternalComponentsUpdateOptions(postgreSQLBuildingDataExternalComponentsUpdateOptions);

            // The caption stays in the XAML - the designer does not run this constructor, and a caption set here
            // leaves an empty label over an empty box in the preview. Only the values come from the options.
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingDataExternalComponentsUpdateOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_BatchSize.Value = this.postgreSQLBuildingDataExternalComponentsUpdateOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            CheckBox_SkipCompleted.IsChecked = this.postgreSQLBuildingDataExternalComponentsUpdateOptions.SkipCompleted;
            CheckBox_PartyWalls.IsChecked = this.postgreSQLBuildingDataExternalComponentsUpdateOptions.PartyWalls;
            TextBoxControl_MaxDegreeOfParallelism.Value = this.postgreSQLBuildingDataExternalComponentsUpdateOptions.MaxDegreeOfParallelism.ToString(CultureInfo.CurrentCulture);

            // Subscribed before the list is filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;

            SetCounties(administrativeAreal2DReferences);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingDataExternalComponentsUpdateOptions PostgreSQLBuildingDataExternalComponentsUpdateOptions
        {
            get
            {
                return postgreSQLBuildingDataExternalComponentsUpdateOptions;
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
                // An empty set is not the same as no set: the task takes null as "every county" and an empty set
                // as "none", so neither is a safe thing to leave this window with by accident.
                MessageBox.Show("At least one county has to be selected.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_CommandTimeout.TryGetValue(out int commandTimeout) || commandTimeout < 0)
            {
                // Zero is legal - it disables the timeout, which is the contract the options carry - and only a
                // negative value is refused.
                MessageBox.Show("Command timeout has to be a whole number of seconds, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_BatchSize.TryGetValue(out int batchSize) || batchSize < 1)
            {
                MessageBox.Show("Batch size has to be a whole number, 1 or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TextBoxControl_MaxDegreeOfParallelism.TryGetValue(out int maxDegreeOfParallelism) || maxDegreeOfParallelism < 0)
            {
                // Zero is legal - it is the option's own "half of the processors" - and so is 1, the sequential
                // run; only a negative value or something that is not a whole number is refused.
                MessageBox.Show("Parallel batches has to be a whole number, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLBuildingDataExternalComponentsUpdateOptions.CountyIds = countyIds;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.CommandTimeout = commandTimeout;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.BatchSize = batchSize;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.SkipCompleted = CheckBox_SkipCompleted.IsChecked == true;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.PartyWalls = CheckBox_PartyWalls.IsChecked == true;
            postgreSQLBuildingDataExternalComponentsUpdateOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;

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

            HashSet<int>? countyIds = postgreSQLBuildingDataExternalComponentsUpdateOptions.CountyIds;

            // The run reads every stored model of the counties in scope, so the unscoped run is the expensive one
            // and the default: every county is selected, and deselecting everything is what OK still refuses.
            if (countyIds is null || countyIds.Count == 0)
            {
                ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => true);
                return;
            }

            // Matched on the identifier rather than on the references themselves: the options carry identifiers,
            // and the references are read afresh from the database on every run.
            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }
    }
}
