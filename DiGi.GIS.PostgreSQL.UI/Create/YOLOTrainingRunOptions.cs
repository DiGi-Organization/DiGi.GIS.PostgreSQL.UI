using DiGi.GIS.PostgreSQL.UI.Enums;
using DiGi.GIS.YOLO.UI.Classes;
using DiGi.GIS.YOLO.UI.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Create
    {
        /// <summary>
        /// Creates the options of a tray YOLO detector training run with the defaults of the given scenario applied.
        /// <para>Starts from a copy of <paramref name="yOLOTrainingRunOptions"/> - the run the operator had before - or from the class defaults, and overwrites only what the scenario decides, so the counties, the folders, the device and the interpreter survive a switch between scenarios:</para>
        /// <para><see cref="YOLOTrainingScenario.Retrain"/>: the start weights are the deployed model.pt, the epoch ceiling is 150, and the dataset folder is resumed (<see cref="YOLOTrainingDatasetOptions.Resume"/> on), so the chosen counties are appended to it.</para>
        /// <para><see cref="YOLOTrainingScenario.Fresh"/>: the start weights are the pretrained yolo26x.pt, the epoch ceiling is 300, and the dataset folder is not resumed, so the runner refuses a folder that already holds a dataset and the dataset is built fresh.</para>
        /// <para>Both weights files are named relative to the runner and resolved against it here (<see cref="Query.ConsoleAppFilePath(string?, string?)"/>), because the runner resolves a relative start path against its own directory and this application would otherwise check a different file. A file that is not found keeps the absolute path the runner's deployed layout gives it (<see cref="Query.ConsoleAppDeployedPath(string?, string?)"/>), so the preflight names the place it looked rather than a relative name.</para>
        /// <para>The dialog offers no control for most dataset members, and their class defaults are this surface's answer: the confidence, the split, the label check sizes, the legacy cut-off and the request sizes are the values the runner's README describes. Two defaults are set here rather than taken: <see cref="YOLOTrainingDatasetOptions.CountOnly"/> is forced off, because the committed runner template ships it on and a counting run trains nothing; and an empty <see cref="YOLOTrainingRunOptions.Steps"/> is replaced by all five steps written out, because the runner reads an empty list as "all steps" and the dialog would otherwise show no step ticked for a run that runs every one. <see cref="YOLOTrainingDatasetOptions.WeightsPaths"/> defaults to the incumbent model.pt; the runner adds the candidate itself.</para>
        /// </summary>
        /// <param name="yOLOTrainingScenario">The scenario whose defaults are applied.</param>
        /// <param name="consoleAppPath">The full path of the runner's executable the weights are resolved against, or null to leave relative names as they are.</param>
        /// <param name="yOLOTrainingRunOptions">The options to start from, which are copied rather than changed; or null to start from the class defaults.</param>
        /// <returns>A new options instance carrying the scenario's defaults.</returns>
        public static YOLOTrainingRunOptions YOLOTrainingRunOptions(YOLOTrainingScenario yOLOTrainingScenario, string? consoleAppPath, YOLOTrainingRunOptions? yOLOTrainingRunOptions = null)
        {
            YOLOTrainingRunOptions result = yOLOTrainingRunOptions is null ? new YOLOTrainingRunOptions() : new YOLOTrainingRunOptions(yOLOTrainingRunOptions);

            YOLOTrainingDatasetOptions yOLOTrainingDatasetOptions = result.DatasetOptions ?? new YOLOTrainingDatasetOptions();
            result.DatasetOptions = yOLOTrainingDatasetOptions;

            // The library's default is 0 - a library must not retry a training unasked - but this surface is the
            // unattended one: a first run here retries a stall or a crash three times. An operator's own value
            // survives, because only a run built from the class defaults is defaulted here.
            if (yOLOTrainingRunOptions is null)
            {
                result.AutoResumeCount = 3;
            }

            string path_Models = Path.Combine(Constants.DirectoryName.UserFiles, DiGi.YOLO.Constants.DirectoryName.YOLO, DiGi.YOLO.Constants.DirectoryName.Models);

            string? WeightsPath(string path)
            {
                return Query.ConsoleAppFilePath(consoleAppPath, path) ?? Query.ConsoleAppDeployedPath(consoleAppPath, path);
            }

            string? path_Model = WeightsPath(Path.Combine(path_Models, Constants.FileName.Model));

            switch (yOLOTrainingScenario)
            {
                case YOLOTrainingScenario.Fresh:
                    result.StartWeightsPath = WeightsPath(Path.Combine(path_Models, Constants.DirectoryName.BaseModels, Constants.FileName.BaseModel));
                    result.Epochs = 300;
                    yOLOTrainingDatasetOptions.Resume = false;
                    break;

                default:
                    result.StartWeightsPath = path_Model;
                    result.Epochs = 150;
                    yOLOTrainingDatasetOptions.Resume = true;
                    break;
            }

            yOLOTrainingDatasetOptions.CountOnly = false;

            if ((yOLOTrainingDatasetOptions.WeightsPaths is null || yOLOTrainingDatasetOptions.WeightsPaths.Count == 0) && path_Model is not null)
            {
                yOLOTrainingDatasetOptions.WeightsPaths = [path_Model];
            }

            if (result.Steps is null || result.Steps.Count == 0)
            {
                List<YOLOTrainingStep> yOLOTrainingSteps = [];
                foreach (YOLOTrainingStep yOLOTrainingStep in Enum.GetValues<YOLOTrainingStep>())
                {
                    yOLOTrainingSteps.Add(yOLOTrainingStep);
                }

                result.Steps = yOLOTrainingSteps;
            }

            return result;
        }
    }
}
