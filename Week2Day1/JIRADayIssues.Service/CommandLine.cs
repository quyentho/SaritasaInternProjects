// <copyright file="CommandLine.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
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
    public class CommandLine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets value for username option.
        /// </summary>
        [Option("-u|--username", CommandOptionType.SingleValue, Description = "username to authentication")]
        [Required]
        public string UserNameOption { get; set; }

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
            DateTime date = this.SetDateOption();

            IRestResponse issuesResponse = await this.MakeIssuesRequestAsync(date);

            this.DisplayIssues(issuesResponse);
        }

        private void DisplayIssues(IRestResponse response)
        {
            var responseHandling = new ResponseHandling();
            ResponseObject responseObject = responseHandling.DeserializeResponse(response);

            responseHandling.DisplayResponse(responseObject);
        }

        private async Task<IRestResponse> MakeIssuesRequestAsync(DateTime date)
        {
            IApiManipulation apiManipulation = new ApiManipulation();
            apiManipulation = new CacheDecorator(apiManipulation);

            IRestRequest request = apiManipulation.ConfigureIssuesRequest(date);

            CancellationTokenSource cancellationToken = this.CheckCancelRequest();

            IRestResponse response = await apiManipulation.GetResponseAsync(request, this.UserNameOption, this.TokenOption, cancellationToken.Token);

            return response;
        }

        private CancellationTokenSource CheckCancelRequest()
        {
            Console.WriteLine("Pending....");
            Console.WriteLine("Press ESC to cancel.");
            bool isCancel = Console.ReadKey(true).Key == ConsoleKey.Escape;
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            if (isCancel == true)
            {
                cancellationToken.Cancel();
                Console.WriteLine("Request canceled.");
            }

            return cancellationToken;
        }

        private DateTime SetDateOption()
        {
            logger.Info("Set date option.");
            DateTime date = DateTime.Now;
            if (this.DateOption.hasValue)
            {
                date = this.DateOption.value.Date;
            }

            return date;
        }
    }
}
