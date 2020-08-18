// <copyright file="ApiManipulation.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Service
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Manipulation on API calls.
    /// </summary>
    public class ApiManipulation : IApiManipulation
    {
        /// <inheritdoc/>
        public IRestRequest ConfigureIssuesRequest(DateTime date)
        {
            var request = new RestRequest("/rest/api/2/search", Method.GET);
            request.AddParameter("jql", $"worklogDate = {date.ToString("yyyy-MM-dd").ToString(CultureInfo.CreateSpecificCulture("en-US"))} AND worklogAuthor = IvanKozhin");

            return request;
        }

        /// <inheritdoc/>
        public IRestRequest ConfigureWorklogsRequest(string issueId)
        {
            var request = new RestRequest("/rest/api/2/issue/{issueId}/worklog", Method.GET);
            request.AddUrlSegment("issueId", issueId);

            return request;
        }

        /// <inheritdoc/>
        public async Task<IRestResponse> GetResponseAsync(IRestRequest request, string username, string token, CancellationToken cancellationToken)
        {
            var client = new RestClient($"https://saritasa.atlassian.net");
            client.Timeout = -1;
            client.Authenticator = new HttpBasicAuthenticator(username, token);

            return await client.ExecuteAsync(request, cancellationToken);
        }
    }
}
