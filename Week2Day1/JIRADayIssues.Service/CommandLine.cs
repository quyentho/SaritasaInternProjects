namespace JiraDayIssues.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using JiraDayIssues.Model;
    using McMaster.Extensions.CommandLineUtils;
    using Newtonsoft.Json;
    using NLog;
    using RestSharp;

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
        /// Execute command.
        /// </summary>
        public void OnExecute()
        {
            DateTime date = this.SetDateOption();

            IRestResponse issuesResponse = this.MakeIssuesRequest(date);

            List<IRestResponse> worklogsResponses = this.MakeWorklogsRequest(issuesResponse);

            this.DisplayIssues(issuesResponse);
        }

        private List<IRestResponse> MakeWorklogsRequest(IRestResponse issuesResponse)
        {
            var responseHandling = new ResponseHandling();
            ResponseObject responseObject = responseHandling.DeserializeResponse(issuesResponse);

            List<Issue> issues = responseObject.Issues;

            var responseList = new List<IRestResponse>();
            foreach (var issue in issues)
            {
                var apiManipulation = new ApiManipulation();
                IRestRequest request = apiManipulation.ConfigureGetWorklogRequest(issue.Id);
                IRestResponse response = apiManipulation.GetResponse(request, this.UserNameOption, this.TokenOption);
                responseList.Add(response);
            }

            return responseList;
        }

        private void DisplayIssues(IRestResponse response)
        {
            var responseHandling = new ResponseHandling();
            ResponseObject responseObject = responseHandling.DeserializeResponse(response);

            responseHandling.DisplayResponse(responseObject);
        }

        private IRestResponse MakeIssuesRequest(DateTime date)
        {
            IApiManipulation apiManipulation = new ApiManipulation();
            apiManipulation = new CacheDecorator(apiManipulation);

            IRestRequest request = apiManipulation.ConfigureGetIssuesRequest(date);
            IRestResponse response = apiManipulation.GetResponse(request, this.UserNameOption, this.TokenOption);
            return response;
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
