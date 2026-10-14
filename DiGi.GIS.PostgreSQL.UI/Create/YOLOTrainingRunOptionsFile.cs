using System;
using System.Globalization;
using System.IO;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Create
    {
        /// <summary>
        /// Writes a tray training run's options into a new file beside the run folder and never overwrites an existing one - the file is the record of what a run was asked to do (DiGi-Organization/DiGi.GIS.PostgreSQL.UI#20).
        /// <para><b>A resume</b> writes <c>&lt;RunName&gt;.resume-&lt;yyyyMMdd_HHmmss&gt;.YOLOTrainingRunOptions.json</c>, so the original run's <c>&lt;RunName&gt;.YOLOTrainingRunOptions.json</c> is left untouched. Should two resumes start within the same second, <c>_2</c>, <c>_3</c> ... is appended to the later one.</para>
        /// <para><b>A run with the Training step</b> writes <c>&lt;RunName&gt;.YOLOTrainingRunOptions.json</c>. The preflight has already refused a taken run name, so that file cannot exist; if it does anyway, the run is refused here, naming the path, rather than written under another name.</para>
        /// <para><b>A run without the Training step</b> always writes <c>yyyyMMdd_HHmmss.YOLOTrainingRunOptions.json</c>, whatever the run name box says, because its run name describes nothing it creates. Should two such runs start within the same second, <c>_2</c>, <c>_3</c> ... is appended to the later one.</para>
        /// <para>The file is opened with <see cref="FileMode.CreateNew"/>, so a file that appears between the check and the write is not overwritten either.</para>
        /// </summary>
        /// <param name="directory">The existing directory the file is written into.</param>
        /// <param name="runName">The run name; used when <paramref name="train"/> or <paramref name="resume"/> is true.</param>
        /// <param name="train">Whether the run includes the Training step.</param>
        /// <param name="contents">The serialized options.</param>
        /// <param name="dateTimeOffset">The time a run without training, or a resume, is named after.</param>
        /// <param name="resume">Whether the run continues an interrupted one; a resume's own file is named after it and the original is never touched.</param>
        /// <returns>The path of the written file, or null when a training run's file already exists or no free name was found.</returns>
        public static string? YOLOTrainingRunOptionsFile(string directory, string? runName, bool train, string contents, DateTimeOffset dateTimeOffset, bool resume = false)
        {
            if (resume && !string.IsNullOrWhiteSpace(runName))
            {
                string name_Resume = string.Concat(runName, Constants.FileName.YOLOTrainingRunOptionsResumeInfix, dateTimeOffset.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture));
                for (int i = 1; i <= 100; i++)
                {
                    string path = Path.Combine(directory, (i == 1 ? name_Resume : name_Resume + "_" + i.ToString(CultureInfo.InvariantCulture)) + Constants.FileName.YOLOTrainingRunOptionsSuffix);
                    if (TryWriteNew(path, contents))
                    {
                        return path;
                    }
                }

                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No free YOLO resume options file name for {Name} in {Directory}", name_Resume, directory);
                return null;
            }

            if (train && !string.IsNullOrWhiteSpace(runName))
            {
                string path_Train = Path.Combine(directory, runName + Constants.FileName.YOLOTrainingRunOptionsSuffix);
                if (TryWriteNew(path_Train, contents))
                {
                    return path_Train;
                }

                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "YOLO training refused - the options file {Path} already exists and is not overwritten", path_Train);
                return null;
            }

            string name = dateTimeOffset.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            for (int i = 1; i <= 100; i++)
            {
                string path = Path.Combine(directory, (i == 1 ? name : name + "_" + i.ToString(CultureInfo.InvariantCulture)) + Constants.FileName.YOLOTrainingRunOptionsSuffix);
                if (TryWriteNew(path, contents))
                {
                    if (i != 1)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "The YOLO training options file for {Name} already existed; written to {Path} instead", name, path);
                    }

                    return path;
                }
            }

            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No free YOLO training options file name for {Name} in {Directory}", name, directory);
            return null;
        }

        private static bool TryWriteNew(string path, string contents)
        {
            if (File.Exists(path))
            {
                return false;
            }

            try
            {
                using FileStream fileStream = new(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                using StreamWriter streamWriter = new(fileStream);
                streamWriter.Write(contents);
                return true;
            }
            catch (IOException) when (File.Exists(path))
            {
                return false;
            }
        }
    }
}
