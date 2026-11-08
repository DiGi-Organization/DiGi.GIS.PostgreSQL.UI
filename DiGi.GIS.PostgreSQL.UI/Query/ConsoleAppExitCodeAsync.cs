using DiGi.GIS.YOLO.UI.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.PostgreSQL.UI
{
    public static partial class Query
    {
        /// <summary>
        /// Runs the headless <c>DiGi.GIS.YOLO.UI.ConsoleApp</c> with the given arguments, logs everything it prints, reports its progress lines and returns the exit code it ended with.
        /// <para>Both output streams are read as they arrive - a child whose output nobody drains blocks on a full pipe, and these runs talk for hours. Every standard output line is logged, and a <c>[PROGRESS]</c> line is read through the runner's own <see cref="GIS.YOLO.UI.Query.ProgressCount(string?)"/> rather than a format literal, so a format that ever drifts costs the progress reporting and nothing else. Standard error lines are logged as warnings.</para>
        /// <para>The exit code is named rather than numbered, read off the runner's own <see cref="YearBuiltPredictionExitCode"/>; a code this application does not recognise is logged as the number it is and still returned.</para>
        /// <para><b>Cancelling kills the whole process tree</b> rather than winding the run down: the interpreter is a grandchild, and killing only the runner would leave it holding a graphics card with nothing waiting for it. Whatever the run was writing may be half written. A cancelled wait returns <see cref="YearBuiltPredictionExitCode.Cancelled"/>.</para>
        /// </summary>
        /// <param name="consoleAppPath">The full path of the runner's executable. Its folder is the working directory the runner starts in.</param>
        /// <param name="arguments">The arguments, each passed through <see cref="ProcessStartInfo.ArgumentList"/> rather than a quoted string, so a path ending in a separator cannot escape its own closing quote.</param>
        /// <param name="name">The name of the run the log lines are headed with, for example "Year built prediction".</param>
        /// <param name="progress">The receiver of the processed item counts the runner reports, or null.</param>
        /// <param name="information">The receiver of every standard output line, in order, or null. It is called on the output pump, not on the caller's thread.</param>
        /// <param name="cancellationToken">The token that stops the run by killing its process tree.</param>
        /// <returns>The exit code the runner ended with; <see cref="YearBuiltPredictionExitCode.Cancelled"/> when the token stopped it; or null when it could not be started, or failed while it was being watched.</returns>
        public static async Task<YearBuiltPredictionExitCode?> ConsoleAppExitCodeAsync(string consoleAppPath, IEnumerable<string>? arguments, string name, IProgress<long>? progress = null, Action<string>? information = null, CancellationToken cancellationToken = default)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = consoleAppPath,
                WorkingDirectory = System.IO.Path.GetDirectoryName(consoleAppPath) ?? string.Empty,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if (arguments is not null)
            {
                foreach (string argument in arguments)
                {
                    processStartInfo.ArgumentList.Add(argument);
                }
            }

            using Process process = new() { StartInfo = processStartInfo, EnableRaisingEvents = true };

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data is not string line)
                {
                    return;
                }

                Serilog.Modify.Log("{Line}", line);

                information?.Invoke(line);

                if (GIS.YOLO.UI.Query.ProgressCount(line) is long count)
                {
                    progress?.Report(count);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data is string line)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Line}", line);
                }
            };

            Serilog.Modify.Log("{Name} run started - {FileName} {Arguments}", name, consoleAppPath, string.Join(" ", processStartInfo.ArgumentList));

            // Starting is separated from waiting so that a runner which never started is not also reported as one
            // that could not be killed - Process.HasExited throws on an instance no process was ever attached to,
            // and a failure to start is the likeliest failure of the two.
            try
            {
                process.Start();
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Name} run could not be started - {FileName}", name, consoleAppPath);
                return null;
            }

            try
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync(cancellationToken);

                // The parameterless wait is the one that also waits for the asynchronous output handlers to drain,
                // so the last line the runner printed is delivered to the line sink before the exit code is read.
                process.WaitForExit();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                Kill(process);

                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} run was stopped - its process tree was killed, so anything it was writing may be half written", name);
                return YearBuiltPredictionExitCode.Cancelled;
            }
            catch (Exception exception)
            {
                Kill(process);

                Serilog.Modify.Log(exception, "{Name} run failed while it was being watched - {FileName}", name, consoleAppPath);
                return null;
            }

            YearBuiltPredictionExitCode yearBuiltPredictionExitCode = (YearBuiltPredictionExitCode)process.ExitCode;

            string description = Enum.IsDefined(yearBuiltPredictionExitCode)
                ? Core.Query.Description(yearBuiltPredictionExitCode) ?? yearBuiltPredictionExitCode.ToString()
                : string.Format(System.Globalization.CultureInfo.InvariantCulture, "Unrecognised exit code {0}", process.ExitCode);

            if (yearBuiltPredictionExitCode == YearBuiltPredictionExitCode.Succeeded)
            {
                Serilog.Modify.Log("{Name} run finished - {Description}", name, description);
            }
            else
            {
                Serilog.Modify.Log(
                    yearBuiltPredictionExitCode == YearBuiltPredictionExitCode.Cancelled ? Serilog.Enums.LogEventLevel.Warning : Serilog.Enums.LogEventLevel.Error,
                    "{Name} run did not finish - {Description}",
                    name,
                    description);
            }

            return yearBuiltPredictionExitCode;

            static void Kill(Process process)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);

                        // Bounded, because the point is to be able to say the run has actually stopped rather than
                        // that stopping it has been asked for - and an unbounded wait would hand a hung child the
                        // power to hold the task row open indefinitely.
                        process.WaitForExit(5000);
                    }
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(exception, "The run could not be killed");
                }
            }
        }
    }
}
