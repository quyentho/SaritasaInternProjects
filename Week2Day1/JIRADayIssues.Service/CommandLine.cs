using JIRADayIssues.Model;
using McMaster.Extensions.CommandLineUtils;
using RestSharp;
using System;
using System.ComponentModel.DataAnnotations;

namespace JIRADayIssues.Service
{
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

        [Option("-t|--token", CommandOptionType.SingleValue, Description = "Token to authentication")]
        [Required]
        public string TokenOption { get; set; }

        private void OnExecute()
        {
            DateTime date = new DateTime(2020, 08, 11).Date;
            if(this.DateOption.hasValue)
            {
                date = DateOption.value.Date;
            }
            
            var manipulation = new APIsManipulation();
            IRestResponse response = manipulation.GetResponse(this.DateOption.value,UserNameOption, TokenOption);

            var responseHandling = new ResponseHandling();
            ResponseObject responseObject = responseHandling.DeserializeResponse(response);
            responseHandling.DisplayResponse(responseObject);
        }
    }
}
