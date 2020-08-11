using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace JIRADayIssues.Model
{
    /// <summary>
    /// Options for command line .
    /// </summary>
    public class CommandLineOption
    {
        /// <summary>
        /// Date to check, if no value, use current date by default.
        /// </summary>
        [Option("-d|--date", CommandOptionType.SingleOrNoValue, Description = "Date to check, current date by default")]
        public (bool hasValue, DateTime value) Date { get; set; }

        /// <summary>
        /// Username option, required.
        /// </summary>
        [Option("-u|--username", CommandOptionType.SingleValue, Description = "Username to check")]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Token option, required.
        /// </summary>
        [Option("-t|--token", CommandOptionType.SingleValue, Description = "Provided token")]
        [Required]
        public string Token { get; set; }

        public void OnExecute()
        {
            Console.WriteLine($"Date {this.Date.value.ToString()}!");
            Console.WriteLine($"Hello {this.Username}!");
            Console.WriteLine($"Token {this.Token}!");
        }
    }
}
