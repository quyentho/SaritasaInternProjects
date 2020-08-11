using JIRADayIssues.Model;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace JIRADayIssues
{
    [Command(Name = "Saritasa.JiraChecker", Description = "Check user's hours for the day.")]
    [HelpOption("--help")]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<CommandLineOption>(args);
    }
}
