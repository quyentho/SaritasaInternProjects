using McMaster.Extensions.CommandLineUtils;
using JiraDayIssues.Service;
namespace JiraDayIssues
{
    [Command(Name = "Saritasa.JiraChecker", Description = "Check user's hours for the day.")]
    [HelpOption("--help")]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<CommandLine>(args);
    }
}
