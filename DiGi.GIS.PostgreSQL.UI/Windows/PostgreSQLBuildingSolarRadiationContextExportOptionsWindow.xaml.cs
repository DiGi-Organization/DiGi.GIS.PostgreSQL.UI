using DiGi.GIS.PostgreSQL.Classes;
using DiGi.UI.WPF.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Windows
{
    /// <summary>
    /// Interaction logic for PostgreSQLBuildingSolarRadiationContextExportOptionsWindow.xaml
    /// <para>Asks for what the urban-context export writes and where: the county parts (one CSV per part), the stored radii in the column order they are exported, whether only residential buildings are kept, the batch size and timeout, and the output folder (DiGi.GIS.PostgreSQL#136).</para>
    /// <para>The export refuses a run without a folder, so the window proposes <see cref="Query.SolarContextDirectory(string?)"/> when the options carry none, and creates the folder when it is accepted.</para>
    /// <para>The window works on a copy, so a cancelled dialog leaves the settings of an earlier run exactly as they were.</para>
    /// </summary>
    public partial class PostgreSQLBuildingSolarRadiationContextExportOptionsWindow : Window
    {
        private readonly PostgreSQLBuildingSolarRadiationContextExportOptions postgreSQLBuildingSolarRadiationContextExportOptions;

        private readonly List<double> radii = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSQLBuildingSolarRadiationContextExportOptionsWindow"/> class.
        /// </summary>
        /// <param name="postgreSQLBuildingSolarRadiationContextExportOptions">The options the controls are filled from. When null the defaults are used.</param>
        /// <param name="administrativeAreal2DReferences">The counties to choose from. A county whose territory is in several pieces is one entry per piece, each with its own identifier, and each has to be selectable on its own.</param>
        public PostgreSQLBuildingSolarRadiationContextExportOptionsWindow(PostgreSQLBuildingSolarRadiationContextExportOptions? postgreSQLBuildingSolarRadiationContextExportOptions, IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            InitializeComponent();

            this.postgreSQLBuildingSolarRadiationContextExportOptions = postgreSQLBuildingSolarRadiationContextExportOptions is null ? new PostgreSQLBuildingSolarRadiationContextExportOptions() : new PostgreSQLBuildingSolarRadiationContextExportOptions(postgreSQLBuildingSolarRadiationContextExportOptions);

            CheckBox_ResidentialOnly.IsChecked = this.postgreSQLBuildingSolarRadiationContextExportOptions.ResidentialOnly;
            TextBoxControl_BatchSize.Value = this.postgreSQLBuildingSolarRadiationContextExportOptions.BatchSize.ToString(CultureInfo.CurrentCulture);
            TextBoxControl_CommandTimeout.Value = this.postgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout.ToString(CultureInfo.CurrentCulture);

            string? outputDirectory = this.postgreSQLBuildingSolarRadiationContextExportOptions.OutputDirectory;
            TextBoxControl_OutputDirectory.Value = string.IsNullOrWhiteSpace(outputDirectory) ? Query.SolarContextDirectory() : outputDirectory;

            // Subscribed before the lists are filled - the text of an item is decided as it is added.
            ListBoxControl_Counties.ItemAdding += ListBoxControl_Counties_ItemAdding;
            ListBoxControl_Radii.ItemAdding += ListBoxControl_Radii_ItemAdding;

            // Kept in the order given rather than sorted: it is the column order of the files.
            foreach (double radius in this.postgreSQLBuildingSolarRadiationContextExportOptions.Radii ?? [])
            {
                if (double.IsFinite(radius) && radius >= 0 && !radii.Contains(radius))
                {
                    radii.Add(radius);
                }
            }

            SetRadii();
            SetCounties(administrativeAreal2DReferences);
        }

        /// <summary>
        /// Gets the options the window holds. They carry the values of the controls only once the dialog has been closed with OK; until then, and after a cancellation, they are the values it was opened with.
        /// </summary>
        public PostgreSQLBuildingSolarRadiationContextExportOptions PostgreSQLBuildingSolarRadiationContextExportOptions
        {
            get
            {
                return postgreSQLBuildingSolarRadiationContextExportOptions;
            }
        }

        private void Button_AddRadius_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxControl_Radius.TryGetValue(out double radius) || !double.IsFinite(radius) || radius < 0)
            {
                MessageBox.Show("A radius has to be a number of metres, zero or greater.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!radii.Contains(radius))
            {
                radii.Add(radius);
                SetRadii();
            }

            TextBoxControl_Radius.Value = string.Empty;
        }

        private void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new()
            {
                Title = "Select the output folder of the solar urban-context export"
            };

            string? outputDirectory = TextBoxControl_OutputDirectory.Value;
            if (!string.IsNullOrWhiteSpace(outputDirectory) && Directory.Exists(outputDirectory))
            {
                openFolderDialog.InitialDirectory = outputDirectory;
            }

            if (openFolderDialog.ShowDialog(this) != true)
            {
                return;
            }

            TextBoxControl_OutputDirectory.Value = openFolderDialog.FolderName;
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

            if (radii.Count == 0)
            {
                MessageBox.Show("At least one radius has to be given.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
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

            string? outputDirectory = TextBoxControl_OutputDirectory.Value?.Trim();
            if (string.IsNullOrWhiteSpace(outputDirectory) || !Path.IsPathFullyQualified(outputDirectory))
            {
                MessageBox.Show("The output folder has to be a full path.", Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "The output folder could not be created: {0}", exception.Message), Title ?? string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            HashSet<int> countyIds = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                countyIds.Add(administrativeAreal2DReference.Id);
            }

            postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds = countyIds;
            postgreSQLBuildingSolarRadiationContextExportOptions.Radii = [.. radii];
            postgreSQLBuildingSolarRadiationContextExportOptions.ResidentialOnly = CheckBox_ResidentialOnly.IsChecked == true;
            postgreSQLBuildingSolarRadiationContextExportOptions.BatchSize = batchSize;
            postgreSQLBuildingSolarRadiationContextExportOptions.CommandTimeout = commandTimeout;
            postgreSQLBuildingSolarRadiationContextExportOptions.OutputDirectory = outputDirectory;

            DialogResult = true;
            Close();
        }

        private void Button_RemoveRadii_Click(object sender, RoutedEventArgs e)
        {
            List<double>? radii_Selected = ListBoxControl_Radii.GetItems<double>();
            if (radii_Selected is null || radii_Selected.Count == 0)
            {
                return;
            }

            radii.RemoveAll(radii_Selected.Contains);
            SetRadii();
        }

        private void ListBoxControl_Counties_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not AdministrativeAreal2DReference administrativeAreal2DReference)
            {
                return;
            }

            // The identifier is shown because it is what the files are named by, and because two pieces of the same
            // county are told apart by nothing else - they share their code and their name.
            e.Name = $"{administrativeAreal2DReference.Code} {administrativeAreal2DReference.Name} (id {administrativeAreal2DReference.Id})";
        }

        private void ListBoxControl_Radii_ItemAdding(object sender, ListBoxItemAddingEventArgs e)
        {
            if (e.Item is not double radius)
            {
                return;
            }

            e.Name = radius == 0 ? "0 m - isolated building (no neighbours)" : string.Format(CultureInfo.CurrentCulture, "{0} m", radius);
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

            HashSet<int>? countyIds = postgreSQLBuildingSolarRadiationContextExportOptions.CountyIds;
            if (countyIds is null || countyIds.Count == 0)
            {
                return;
            }

            ListBoxControl_Counties.SelectItems<AdministrativeAreal2DReference>(x => countyIds.Contains(x.Id));
        }

        private void SetRadii()
        {
            ListBoxControl_Radii.SetItems(radii);

            // The shortfall columns are measured against the isolated building, so without 0 they stay empty.
            Label_RadiiHint.Content = radii.Contains(0) ? string.Empty : "Without 0 m the shortfall columns stay empty";
        }
    }
}
