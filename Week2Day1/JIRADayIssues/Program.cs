// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using JiraDayIssues.Service;
using McMaster.Extensions.CommandLineUtils;

namespace JiraDayIssues
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    [Command(Name = "Saritasa.JiraChecker", Description = "Check user's hours for the day.")]
    [HelpOption("--help")]
    public class Program
    {
        /// <summary>
        /// Appication entry point.
        /// </summary>
        /// <param name="args">Arguments get from console.</param>
        /// <returns>Execution status code.</returns>
        public static int Main(string[] args) => CommandLineApplication.Execute<CommandLine>(args);
    }
}
