// <copyright file="CommandLine.cs" company="Saritasa, LLC">
// copyright (c) Saritasa, LLC
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using JiraDayIssues.Model;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using NLog;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Command line manipulation.
    /// </summary>
    public class CommandLineExecution
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets value for username option.
        /// </summary>
        [Option("-u|--username", CommandOptionType.SingleValue, Description = "username to authentication")]
        [Required]
        public string UserNameOption { get; set; }

        /// <summary>
        /// Gets or sets value for worklog author option.
        /// </summary>
        [Option("-w|--worklogauthor", CommandOptionType.SingleValue, Description = "worklog author to check")]
        [Required]
        public string WorklogAuthorOption { get; set; }

        /// <summary>
        /// Gets or sets date to check, default is current date.
        /// </summary>
        [Option("-d|--date", CommandOptionType.SingleOrNoValue, Description = "Date to check.")]
        public (bool hasValue, DateTime value) DateOption { get; set; }

        /// <summary>
        /// Gets or sets token to authentication.
        /// </summary>
        [Option("-t|--token", CommandOptionType.SingleValue, Description = "Token to authentication")]
        [Required]
        public string TokenOption { get; set; }

        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <returns>Task represent asynchronous process.</returns>
        public async Task OnExecuteAsync()
        {
            DateTime date = this.GetDateOption();

            JiraIssueResponse response = await this.GetResponse(date);

            this.DisplayIssues(response);
        }

        private void DisplayIssues(JiraIssueResponse response)
        {
            var responsePresenter = new ResponsePresenter();

            responsePresenter.DisplayResponse(response);
        }

        private async Task<JiraIssueResponse> GetResponse(DateTime date)
        {
            IJiraApiClient jiraApiClient = new JiraApiClient(this.UserNameOption, this.TokenOption);

            CancellationTokenSource cancellationToken = this.CheckCancelRequest();

            JiraIssueResponse response = await jiraApiClient.GetIssuesAsync(date, this.WorklogAuthorOption, cancellationToken.Token);

            return response;
        }

        private CancellationTokenSource CheckCancelRequest()
        {
            Console.WriteLine("Pending....");
            Console.WriteLine("Press ESC to cancel press any key to continue.");
            
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            bool isCancel = Console.ReadKey(true).Key == ConsoleKey.Escape;
            if (isCancel)
            {
                cancellationToken.Cancel();
                Console.WriteLine("Request canceled.");
            }

            return cancellationToken;
        }

        private DateTime GetDateOption()
        {
            _logger.Info("Set date option.");
            DateTime date = DateTime.Now;
            if (this.DateOption.hasValue)
            {
                date = this.DateOption.value.Date;
            }

            return date;
        }
    }
}
