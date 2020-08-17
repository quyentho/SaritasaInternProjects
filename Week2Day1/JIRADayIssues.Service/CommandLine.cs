namespace JiraDayIssues.Service
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using McMaster.Extensions.CommandLineUtils;
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

            IRestResponse response = this.MakeRequest(date);

            this.DisplayResponse(response);
        }

        private void DisplayResponse(IRestResponse response)
        {
            var responseHandling = new ResponseHandling();
            var responseObject = responseHandling.DeserializeResponse(response);
            responseHandling.DisplayResponse(responseObject);
        }

        private IRestResponse MakeRequest(DateTime date)
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
