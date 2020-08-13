using JiraDayIssues.Model;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Manipulation on API calls.
    /// </summary>
    public class ApiManipulation
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Request to JIRA API to get worklog on specific date.
        /// </summary>
        /// <param name="date">Date gets from user input, default is current</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <returns>Object of type IRestResponse.</returns>
        public IRestResponse GetResponse(DateTime date,string username, string token)
        {
            var client = new RestClient($"https://saritasa.atlassian.net/rest/api/2/search");
            client.Timeout = -1;
            client.Authenticator = new HttpBasicAuthenticator(username, token);

            var request = new RestRequest(Method.GET);
            request.AddParameter("jql", $"worklogDate = { date.ToString("yyyy-MM-dd").ToString(CultureInfo.CreateSpecificCulture("en-US"))} AND worklogAuthor = currentuser()");
            
            logger.Info($"Get issues and work logs for user {username} and date {date}.");
            
            return client.Execute(request);
        }

    }
}
