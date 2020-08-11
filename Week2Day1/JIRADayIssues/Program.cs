using McMaster.Extensions.CommandLineUtils;
using JIRADayIssues.Service;
namespace JIRADayIssues
{
    [Command(Name = "Saritasa.JiraChecker", Description = "Check user's hours for the day.")]
    [HelpOption("--help")]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<CommandLine>(args);
    }
}
