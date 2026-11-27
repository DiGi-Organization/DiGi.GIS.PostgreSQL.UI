using DiGi.BDL.Enums;
using DiGi.Core.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.Constants;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Writes the BDL statistical data of every unit of a statistical unit file (<c>*.suf</c>) to a statistical data collection file (<c>*.sdcf</c>), the file <see cref="UIPostgreSQLStatisticalDataCollectionPopulateTask"/> loads into the database (DiGi-Organization/DiGi.GIS.PostgreSQL#149). Moved here from the DiGi.GIS.UI application.
    /// <para>The variables missing from the file are fetched with <c>GIS.Modify.WriteByVariableAsync</c>: one variable for a whole unit level at once, about 40 requests for the gmina level, instead of one request per unit, which the BDL limits stopped after 8 of 4 684 units. The BDL client id comes from <see cref="ClientId"/>, else from <c>BDL.conf</c> beside the application (<see cref="Query.BDLClientId"/>); without one the anonymous limits apply and a warning is logged.</para>
    /// <para>The fetches that failed or were not attempted - a variable not published at a level, an exhausted quota, a blocked address - are counted and sampled, not dropped. The run is reported as failed only when it wrote nothing and something failed.</para>
    /// </summary>
    public class UIStatisticalDataCollectionsWriteTask : ReportableBackgroundTask<long>, IGISPostgreSQLUIObject
    {
        private const int failureSampleCount = 50;

        /// <summary>
        /// Gets or sets the BDL API client identifier, or null to read it from <c>BDL.conf</c>.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets the number of collections the last run wrote.
        /// </summary>
        public int CollectionCount { get; private set; }

        /// <summary>
        /// Gets the number of variable and unit level fetches the last run failed or did not attempt.
        /// </summary>
        public int FailureCount { get; private set; }

        /// <summary>
        /// Gets up to 50 of the failed fetches of the last run, as "variable (id) at unit level n".
        /// </summary>
        public List<string> FailureSamples { get; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether every variable is fetched again, whether or not the file holds it.
        /// </summary>
        public bool OverrideExisting { get; set; }

        /// <summary>
        /// Gets or sets the statistical data collection file to write, or null to ask for it.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the statistical unit file whose units are written, or null to ask for it.
        /// </summary>
        public string? StatisticalUnitFilePath { get; set; }

        /// <summary>
        /// Gets or sets the variables to write, or null for every <see cref="Variable"/>.
        /// </summary>
        public IEnumerable<Variable>? Variables { get; set; }

        /// <summary>
        /// Gets or sets the years to retrieve, or null for 2008 to the current year.
        /// </summary>
        public Range<int>? Years { get; set; }

        /// <summary>
        /// Asks for the unit file and the collection file when not set, then writes the collections.
        /// </summary>
        /// <param name="progress">Receives the number of variable and unit level fetches done.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>True when the run wrote a collection or nothing failed; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            CollectionCount = 0;
            FailureCount = 0;
            FailureSamples.Clear();

            string? statisticalUnitFilePath = StatisticalUnitFilePath;
            string? path = Path;
            if (string.IsNullOrWhiteSpace(statisticalUnitFilePath) || string.IsNullOrWhiteSpace(path))
            {
                if (System.Windows.Application.Current is not System.Windows.Application application)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: no unit file or collection file set and no UI thread to ask for them", nameof(UIStatisticalDataCollectionsWriteTask));
                    return false;
                }

                application.Dispatcher.Invoke(() =>
                {
                    if (string.IsNullOrWhiteSpace(statisticalUnitFilePath))
                    {
                        OpenFileDialog openFileDialog = new()
                        {
                            Title = "Select the statistical unit file",
                            Filter = string.Format(CultureInfo.InvariantCulture, "{0} (*.{1})|*.{1}|All files (*.*)|*.*", FileTypeName.StatisticalUnitFile, FileExtension.StatisticalUnitFile)
                        };

                        if (openFileDialog.ShowDialog() is not true)
                        {
                            return;
                        }

                        statisticalUnitFilePath = openFileDialog.FileName;
                    }

                    if (string.IsNullOrWhiteSpace(path))
                    {
                        SaveFileDialog saveFileDialog = new()
                        {
                            Title = "Select the statistical data collection file to write (an existing file gains only what it lacks)",
                            Filter = string.Format(CultureInfo.InvariantCulture, "{0} (*.{1})|*.{1}|All files (*.*)|*.*", FileTypeName.StatisticalDataCollectionFile, FileExtension.StatisticalDataCollectionFile),
                            FileName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", "StatisticalDataCollections", FileExtension.StatisticalDataCollectionFile),
                            OverwritePrompt = false
                        };

                        if (saveFileDialog.ShowDialog() is true)
                        {
                            path = saveFileDialog.FileName;
                        }
                    }
                });

                if (string.IsNullOrWhiteSpace(statisticalUnitFilePath) || string.IsNullOrWhiteSpace(path))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Information, "{Type}: cancelled - no unit file or collection file selected", nameof(UIStatisticalDataCollectionsWriteTask));
                    CancelRun();
                    return false;
                }
            }

            if (!File.Exists(statisticalUnitFilePath))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the statistical unit file {Path} does not exist", nameof(UIStatisticalDataCollectionsWriteTask), statisticalUnitFilePath);
                return false;
            }

            StatisticalUnit? statisticalUnit;
            using (StatisticalUnitFile statisticalUnitFile = new(statisticalUnitFilePath))
            {
                statisticalUnitFile.Open();
                statisticalUnit = statisticalUnitFile.Value;
            }

            List<StatisticalUnit> statisticalUnits = [.. statisticalUnit?.GetStatisticalUnits(true) ?? []];
            if (statisticalUnits.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: the statistical unit file {Path} holds no units", nameof(UIStatisticalDataCollectionsWriteTask), statisticalUnitFilePath);
                return false;
            }

            string? clientId = string.IsNullOrWhiteSpace(ClientId) ? Query.BDLClientId() : ClientId;
            if (clientId is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}: no BDL client id (BDL.conf ClientId beside the application) - the anonymous BDL limits apply", nameof(UIStatisticalDataCollectionsWriteTask));
            }

            Range<int> years = Years ?? new Range<int>(2008, DateTime.Now.Year);
            List<int> years_List = [];
            for (int year = years.Min; year <= years.Max; year++)
            {
                years_List.Add(year);
            }

            IEnumerable<Variable> variables = Variables ?? Enum.GetValues<Variable>();

            using (StatisticalDataCollectionFile statisticalDataCollectionFile = new(path))
            {
                List<string>? undecodableEntryNames = statisticalDataCollectionFile.GetUndecodableEntryNames();
                if (undecodableEntryNames != null && undecodableEntryNames.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: {Path} holds {EntryCount} collections keyed in a former reference format (e.g. {EntryName}) - migrate the file first; nothing fetched or written", nameof(UIStatisticalDataCollectionsWriteTask), path, undecodableEntryNames.Count, undecodableEntryNames[0]);
                    return false;
                }
            }

            Serilog.Modify.Log("{Type}: writing {UnitCount} units to {Path}", nameof(UIStatisticalDataCollectionsWriteTask), statisticalUnits.Count, path);

            (int CollectionCount, List<(Variable Variable, int UnitLevel)> Failures)? result = await GIS.Modify.WriteByVariableAsync(statisticalUnits, path, variables, years_List, clientId, OverrideExisting, progress: progress, cancellationToken: cancellationToken);
            if (result is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: nothing could be written to {Path}", nameof(UIStatisticalDataCollectionsWriteTask), path);
                return false;
            }

            CollectionCount = result.Value.CollectionCount;
            FailureCount = result.Value.Failures.Count;
            FailureSamples.AddRange(result.Value.Failures.Take(failureSampleCount).Select(x => string.Format(CultureInfo.InvariantCulture, "{0} ({1}) at unit level {2}", x.Variable, (int)x.Variable, x.UnitLevel)));

            Serilog.Modify.Log("{Type}: {CollectionCount} collections written to {Path}, {FailureCount} fetches failed or not attempted", nameof(UIStatisticalDataCollectionsWriteTask), CollectionCount, path, FailureCount);
            if (FailureCount > 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}: failed or not attempted (a level a variable is not published at, a BDL limit or a blocked address): {Failures}", nameof(UIStatisticalDataCollectionsWriteTask), string.Join("; ", FailureSamples));
            }

            if (CollectionCount == 0 && FailureCount > 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}: nothing written and {FailureCount} fetches failed - check the BDL client id, the limits and whether bdl.stat.gov.pl blocks this address", nameof(UIStatisticalDataCollectionsWriteTask), FailureCount);
                return false;
            }

            return true;
        }
    }
}
