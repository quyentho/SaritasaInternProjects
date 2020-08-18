// <copyright file="ApiManipulation.cs" company="Saritasa, LLC">
// copyright (c) Saritasa, LLC
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
        private readonly RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManipulation"/> class.
        /// </summary>
        /// <param name="client">RestClient instance.</param>
        public ApiManipulation(RestClient client)
        {
            this.client = client;
        }

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
            this.client.BaseUrl = new Uri("https://saritasa.atlassian.net");
            this.client.Timeout = -1;
            this.client.Authenticator = new HttpBasicAuthenticator(username, token);

            return await this.client.ExecuteAsync(request, cancellationToken);
        }
    }
}
