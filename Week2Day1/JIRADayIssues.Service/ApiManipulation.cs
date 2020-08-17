namespace JiraDayIssues.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using JiraDayIssues.Model;
    using Newtonsoft.Json;
    using NLog;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Manipulation on API calls.
    /// </summary>
    public class ApiManipulation : IApiManipulation
    {
        /// <summary>
        /// Configure request to get issues on specific date.
        /// </summary>
        /// <param name="date">Date to get issues.</param>
        /// <returns>Request after config.</returns>
        public IRestRequest ConfigureGetIssuesRequest(DateTime date)
        {
            var request = new RestRequest("/rest/api/2/search", Method.GET);
            request.AddParameter("jql", $"worklogDate = {date.ToString("yyyy-MM-dd").ToString(CultureInfo.CreateSpecificCulture("en-US"))} AND worklogAuthor = IvanKozhin");

            return request;
        }

        /// <summary>
        /// Configure request to get worklogs on specific issue.
        /// </summary>
        /// <param name="issueId">Issue to get worklogs.</param>
        /// <returns>Request after config.</returns>
        public IRestRequest ConfigureGetWorklogsRequest(string issueId)
        {
            var request = new RestRequest("/rest/api/2/issue/{issueId}/worklog", Method.GET);
            request.AddUrlSegment("issueId", issueId);

            return request;
        }

        /// <summary>
        /// Get response after request with authentication.
        /// </summary>
        /// <param name="request">Request configured.</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <returns>Response object.</returns>
        public async Task<IRestResponse> GetResponseAsync(IRestRequest request, string username, string token)
        {
            var client = new RestClient($"https://saritasa.atlassian.net");
            client.Timeout = -1;
            client.Authenticator = new HttpBasicAuthenticator(username, token);

            return await client.ExecuteAsync(request);
        }
    }
}
