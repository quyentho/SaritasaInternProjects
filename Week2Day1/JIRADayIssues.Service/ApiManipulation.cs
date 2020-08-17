namespace JiraDayIssues.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
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
        /// Add parameters for resquest.
        /// </summary>
        /// <param name="date">Date to query on request to API.</param>
        /// <returns>Request object.</returns>
        public IRestRequest ConfigureRequest(DateTime date)
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("jql", $"worklogDate = {date.ToString("yyyy-MM-dd").ToString(CultureInfo.CreateSpecificCulture("en-US"))} AND worklogAuthor = IvanKozhin");

            return request;
        }

        /// <summary>
        /// Get response after request with authentication.
        /// </summary>
        /// <param name="request">Request configured.</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <returns>Response object.</returns>
        public IRestResponse GetResponse(IRestRequest request, string username, string token)
        {
            var client = new RestClient($"https://saritasa.atlassian.net/rest/api/2/search");
            client.Timeout = -1;
            client.Authenticator = new HttpBasicAuthenticator(username, token);

            return client.Execute(request);
        }
    }
}
