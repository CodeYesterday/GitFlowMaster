using System.Diagnostics;
using System.Text;

namespace CodeYesterday.GitFlowMaster.Api.Cli.Helper;

/// <summary>
/// A Helper class to run CLI commands and retrieve the output.
/// </summary>
[PublicAPI]
public class CliHelper
{
    /// <summary>
    /// Gets or sets the CLI command.
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Gets or sets the CLI arguments.
    /// </summary>
    public required ICollection<string> Arguments { get; init; }

    /// <summary>
    /// Gets or sets the working directory.
    /// </summary>
    public required string WorkingDirectory { get; init; }

    /// <summary>
    /// Gets the exit code of the process.
    /// </summary>
    /// <remarks>
    /// If the process did not exit, <see cref="ExitCode"/> is <see cref="int.MinValue"/>.
    /// </remarks>
    public int ExitCode { get; private set; } = int.MinValue;

    /// <summary>
    /// Gets the full output (StdOut + StdError) of the process.
    /// </summary>
    /// <remarks>
    /// If the process did not exit, <see cref="FullOutput"/> is `null`.
    /// </remarks>
    public string? FullOutput { get; private set; }

    /// <summary>
    /// Gets the output (StdOut) of the process.
    /// </summary>
    /// <remarks>
    /// If the process did not exit, <see cref="Output"/> is `null`.
    /// </remarks>
    public string? Output { get; private set; }

    /// <summary>
    /// Gets the error output (StdError) of the process.
    /// </summary>
    /// <remarks>
    /// If the process did not exit, <see cref="ErrorOutput"/> is `null`.
    /// </remarks>
    public string? ErrorOutput { get; private set; }

    /// <summary>
    /// Gets the full command including the arguments.
    /// </summary>
    public string FullCommand =>
        $"{Command} {string.Join(" ", Arguments.Select(a => a.Contains(' ') ? $"\"{a}\"" : a))}".Trim();

    /// <summary>
    /// Runs the process ad waits for it to exit.
    /// </summary>
    /// <returns>Returns the exit code of the process.</returns>
    public async Task<int> Run()
    {
        var process = new Process()
        {
            StartInfo = new(Command, Arguments)
            {
                WorkingDirectory = WorkingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        var fullOutput = new StringBuilder();
        var output = new StringBuilder();
        var errorOutput = new StringBuilder();

        process.OutputDataReceived += (_, args) =>
        {
            if (args.Data != null)
            {
                fullOutput.AppendLine(args.Data);
                output.AppendLine(args.Data);
            }
        };

        process.ErrorDataReceived += (_, args) =>
        {
            if (args.Data != null)
            {
                fullOutput.AppendLine(args.Data);
                errorOutput.AppendLine(args.Data);
            }
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync().ConfigureAwait(false);

        FullOutput = fullOutput.ToString();
        Output = output.ToString();
        ErrorOutput = errorOutput.ToString();

        ExitCode = process.ExitCode;

        return process.ExitCode;
    }
}
