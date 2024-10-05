using CodeYesterday.GitFlowMaster.Api.Cli.Helper;

Console.WriteLine("GitFlowMaster Sandbox");

Console.WriteLine($"WorkingDirectory: {Directory.GetCurrentDirectory()}");

var cli = new CliHelper()
{
    Command = "git",
    Arguments = ["status"],
    WorkingDirectory = "."
};

Console.WriteLine($"> {cli.FullCommand}");

await cli.Run();

Console.WriteLine($"ExitCode: {cli.ExitCode}\n" +
                  "FullOutput:\n" +
                  cli.FullOutput + "\n" +
                  "Output:\n" +
                  cli.Output + "\n" +
                  "ErrorOutput:\n" +
                  cli.ErrorOutput + "\n");
