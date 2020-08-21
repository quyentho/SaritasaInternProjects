using JiraDayIssues.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Decorator to add caching service for JiraApiClient.
    /// </summary>
    public class IJiraClientDecorator : IJiraApiClient
    {
        private readonly IJiraApiClient _jiraApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="IJiraClientDecorator"/> class.
        /// </summary>
        /// <param name="jiraApiClient">Instance of JiraApiClient.</param>
        public IJiraClientDecorator(IJiraApiClient jiraApiClient)
        {
            _jiraApiClient = jiraApiClient;
        }
        public Dictionary<string, List<Worklog>> CachedWorklogs { get; private set; } = new Dictionary<string, List<Worklog>>();

        /// <inheritdoc/>
        public async Task<JiraIssueResponse> GetIssuesAsync(DateTime date, string worklogAuthor, CancellationToken cancellationToken)
        {
            JiraIssueResponse response = await _jiraApiClient.GetIssuesAsync(date, worklogAuthor, cancellationToken);

            CacheWorklogsAsync(response, cancellationToken);

            return response;
        }

        public async Task<JiraWorklogResponse> GetWorklogsAsync(string issueId, CancellationToken cancellationToken)
        {
            return await _jiraApiClient.GetWorklogsAsync(issueId, cancellationToken);
        }

        private async Task CacheWorklogsAsync(JiraIssueResponse issuesResponse, CancellationToken cancellationToken)
        {
            foreach (var issue in issuesResponse.Issues)
            {
                JiraWorklogResponse worklogResponse = await GetWorklogsAsync(issue.Id, cancellationToken);

                CachedWorklogs.Add(issue.Id, worklogResponse.Worklogs);
            }
        }
    }
}
