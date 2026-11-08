using DiGi.Core.Classes;
using DiGi.Core.Interfaces;
using DiGi.GIS.Classes;
using DiGi.GIS.Constants;
using DiGi.GIS.PostgreSQL.UI.Interfaces;
using DiGi.GIS.WebAPI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiGi.GIS.PostgreSQL.UI.Classes
{
    /// <summary>
    /// Represents a task that handles the process of posting occupancy data extracted from GIS model files to the PostgreSQL database through the user interface.
    /// </summary>
    public class UIOccupancyDatasFromFilePostTask : OccupancyDatasPostTask, IGISPostgreSQLUIObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIOccupancyDatasFromFilePostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The manager used to interact with the GIS PostgreSQL Web API.</param>
        public UIOccupancyDatasFromFilePostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Concrete implementation of the background work.
        /// </summary>
        /// <param name="progress">The provider for reporting progress of the operation.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the operation completed successfully; otherwise, false.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            if (Values is not null)
            {
                return await base.ExecuteAsync(progress, cancellationToken);
            }

            string? code = Code;

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want select single GIS Model file?", "Selection", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return false;
            }

            List<string> paths_GISModel = [];
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "Select GIS Model file",
                    Filter = string.Format(CultureInfo.CurrentCulture, "{0} (*.{1})|*.{1}|All files (*.*)|*.*", FileTypeName.GISModelFile, FileExtension.GISModelFile)
                };

                bool? dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                paths_GISModel.Add(openFileDialog.FileName);
            }
            else
            {
                OpenFolderDialog openFolderDialog = new();
                bool? dialogResult = openFolderDialog.ShowDialog();
                if (dialogResult == null || !dialogResult.HasValue || !dialogResult.Value)
                {
                    return false;
                }

                string directory = openFolderDialog.FolderName;
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                {
                    return false;
                }

                string[] paths_Input = Directory.GetFiles(directory, "*." + FileExtension.GISModelFile, SearchOption.AllDirectories);
                if (paths_Input == null || paths_Input.Length == 0)
                {
                    return false;
                }

                paths_GISModel = [.. paths_Input];
            }

            if (paths_GISModel is null || paths_GISModel.Count == 0)
            {
                return true;
            }

            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            foreach (string path_GISModel in paths_GISModel)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(path_GISModel) || !File.Exists(path_GISModel))
                {
                    return false;
                }

                using GISModelFile gISModelFile = new(path_GISModel);

                gISModelFile.Open();

                cancellationToken.ThrowIfCancellationRequested();

                GISModel? gISModel = gISModelFile.Value;

                if (gISModel is null)
                {
                    continue;
                }

                #region AdministrativeAreal2Ds

                cancellationToken.ThrowIfCancellationRequested();

                List<AdministrativeAreal2D>? administrativeAreal2Ds = gISModel.GetObjects<AdministrativeAreal2D>();

                if (administrativeAreal2Ds is not null && administrativeAreal2Ds.Count != 0)
                {
                    List<OccupancyData> occupancyDatas_AdministrativeAreal2D = [];

                    Dictionary<IUniqueReference, OccupancyCalculationResult>? dictionary = gISModel.GetRelatedObjectDictionary<OccupancyCalculationResult>(administrativeAreal2Ds);
                    if (dictionary is not null)
                    {
                        foreach (AdministrativeAreal2D administrativeAreal2D in administrativeAreal2Ds)
                        {
                            if (dictionary.TryGetValue(new GuidReference(administrativeAreal2D), out OccupancyCalculationResult? occupancyCalculationResult) && occupancyCalculationResult is not null)
                            {
                                occupancyDatas_AdministrativeAreal2D.Add(new OccupancyData(administrativeAreal2D.Reference, occupancyCalculationResult.OccupancyArea, occupancyCalculationResult.Occupancy));
                            }
                        }
                    }

                    List<OccupancyData>? OccupancyDatas_Split;

                    SizeSplitter<OccupancyData> sizeSplitter = new(occupancyDatas_AdministrativeAreal2D);
                    while ((OccupancyDatas_Split = sizeSplitter.Next(100)) is not null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        bool succeeded = false;
                        try
                        {
                            succeeded = await ExecuteAsync(OccupancyDatas_Split, longProgressWrapper, cancellationToken);
                        }
                        catch
                        {
                            throw;
                        }

                        if (!succeeded)
                        {
                            return false;
                        }
                    }
                }

                #endregion AdministrativeAreal2Ds

                #region Building2Ds

                cancellationToken.ThrowIfCancellationRequested();

                List<Building2D>? building2Ds = gISModel?.GetObjects<Building2D>();

                if (building2Ds is not null && building2Ds.Count != 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string? code_GISModel = gISModel!.Reference;
                    if (!string.IsNullOrWhiteSpace(code_GISModel))
                    {
                        code_GISModel = code_GISModel.ToUpper(CultureInfo.CurrentCulture);
                        int index = code_GISModel.IndexOf('_');
                        if (index != -1)
                        {
                            code_GISModel = code_GISModel[..index];
                        }
                    }

                    List<OccupancyData> occupancyDatas_Building2D = [];

                    Dictionary<IUniqueReference, OccupancyCalculationResult>? dictionary = gISModel.GetRelatedObjectDictionary<OccupancyCalculationResult>(building2Ds);
                    if (dictionary is not null)
                    {
                        foreach (Building2D building2D in building2Ds)
                        {
                            if (dictionary.TryGetValue(new GuidReference(building2D), out OccupancyCalculationResult? occupancyCalculationResult) && occupancyCalculationResult is not null)
                            {
                                occupancyDatas_Building2D.Add(new OccupancyData(building2D.Reference, occupancyCalculationResult.OccupancyArea, occupancyCalculationResult.Occupancy));
                            }
                        }
                    }

                    List<OccupancyData>? OccupancyDatas_Split;

                    SizeSplitter<OccupancyData> sizeSplitter = new(occupancyDatas_Building2D);
                    while ((OccupancyDatas_Split = sizeSplitter.Next(100)) is not null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        bool succeeded = false;
                        try
                        {
                            succeeded = await ExecuteAsync(OccupancyDatas_Split, code_GISModel, longProgressWrapper, cancellationToken);
                        }
                        catch
                        {
                            throw;
                        }

                        if (!succeeded)
                        {
                            return false;
                        }
                    }
                }

                #endregion Building2Ds
            }

            return true;
        }
    }
}
