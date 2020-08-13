<<<<<<< HEAD
﻿using System;

namespace JIRADayIssues
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
=======
﻿using JiraDayIssues.Service;
using McMaster.Extensions.CommandLineUtils;

namespace JiraDayIssues
{
    [Command(Name = "Saritasa.JiraChecker", Description = "Check user's hours for the day.")]
    [HelpOption("--help")]
    public class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<CommandLine>(args);
>>>>>>> feature/Week2Day1
    }
}
