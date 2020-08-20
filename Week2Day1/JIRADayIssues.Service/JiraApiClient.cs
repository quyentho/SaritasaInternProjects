// <copyright file="ApiManipulation.cs" company="Saritasa, LLC">
// copyright (c) Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Service
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using JiraDayIssues.Model;
    using Newtonsoft.Json;
    using NLog;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Perform Jira API Request.
    /// </summary>
    public class JiraApiClient : IJiraApiClient
    {
        private readonly RestClient _client = new RestClient("https://saritasa.atlassian.net");

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="JiraApiClient"/> class.
        /// </summary>
        /// <param name="username">username to authentication.</param>
        /// <param name="token">token to authentication.</param>
        public JiraApiClient(string username, string token)
        {
            SetupClient(username, token);
        }

        /// <inheritdoc/>
        public async Task<JiraIssueResponse> GetIssuesAsync(DateTime date, string worklogAuthor, CancellationToken cancellationToken)
        {
            var request = new RestRequest("/rest/api/2/search", Method.GET);
            request.AddParameter("jql", $"worklogDate = {date.ToString("yyyy-MM-dd").ToString(CultureInfo.CreateSpecificCulture("en-US"))} AND worklogAuthor = {worklogAuthor}");

            var response = await this.ExecuteRequest<JiraIssueResponse>(request, cancellationToken);

            return response;
        }

        /// <inheritdoc/>
        public async Task<JiraWorklogResponse> GetWorklogsAsync(string issueId, CancellationToken cancellationToken)
        {
            var request = new RestRequest("/rest/api/2/issue/{issueId}/worklog", Method.GET);
            request.AddUrlSegment("issueId", issueId);

            var response = await this.ExecuteRequest<JiraWorklogResponse>(request, cancellationToken);

            return response;
        }

        private async Task<T> ExecuteRequest<T>(RestRequest request, CancellationToken cancellationToken)
        {
            _logger.Trace("Request: {request}", request);

            var response = await this._client.ExecuteAsync(request, cancellationToken);

            _logger.Trace("Response: {response}", response);

            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        private void SetupClient(string username, string token)
        {
            _client.Timeout = -1;
            _client.Authenticator = new HttpBasicAuthenticator(username, token);
        }
    }
}
