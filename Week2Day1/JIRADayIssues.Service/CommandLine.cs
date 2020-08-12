using JIRADayIssues.Model;
using McMaster.Extensions.CommandLineUtils;
using RestSharp;
using System;
using System.ComponentModel.DataAnnotations;

namespace JIRADayIssues.Service
{
    /// <summary>
    /// Command line manipulation.
    /// </summary>
    public class CommandLine
    {
        /// <summary>
        /// Gets or sets value for username option.
        /// </summary>
        [Option("-u|--username",CommandOptionType.SingleValue, Description = "username to authentication")]
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
            DateTime date = SetDateOption();

            IRestResponse response = MakeRequest(date);

            DisplayResponse(response);
        }

        private static void DisplayResponse(IRestResponse response)
        {
            var responseHandling = new ResponseHandling();
            var responseObject = responseHandling.DeserializeResponse(response);
            responseHandling.DisplayResponse(responseObject);
        }

        private IRestResponse MakeRequest(DateTime date)
        {
            var manipulation = new APIsManipulation();
            IRestResponse response = manipulation.GetResponse(date, UserNameOption, TokenOption);
            return response;
        }

        private DateTime SetDateOption()
        {
            DateTime date = DateTime.Now;
            if (this.DateOption.hasValue)
            {
                date = DateOption.value.Date;
            }

            return date; 
        }
    }
}
