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
    public class CachedJiraApiClient : IJiraApiClient
    {
        private readonly IJiraApiClient _jiraApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedJiraApiClient"/> class.
        /// </summary>
        /// <param name="jiraApiClient">Instance of JiraApiClient.</param>
        public CachedJiraApiClient(IJiraApiClient jiraApiClient)
        {
            _jiraApiClient = jiraApiClient;
        }

        public Dictionary<string, List<Worklog>> CachedWorklogs { get; private set; } = new Dictionary<string, List<Worklog>>();

        /// <inheritdoc/>
        public async Task<JiraIssueResponse> GetIssuesAsync(DateTime date, string worklogAuthor, CancellationToken cancellationToken)
        {
            JiraIssueResponse jiraIssuesResponse = await _jiraApiClient.GetIssuesAsync(date, worklogAuthor, cancellationToken);

            CacheWorklogsAsync(jiraIssuesResponse, cancellationToken);

            return jiraIssuesResponse;
        }

        /// <inheritdoc/>
        public async Task<List<Worklog>> GetWorklogsAsync(string issueId, CancellationToken cancellationToken)
        {
            if (CachedWorklogs.ContainsKey(issueId))
            {
                return CachedWorklogs[issueId];
            }

            return await _jiraApiClient.GetWorklogsAsync(issueId, cancellationToken);
        }

        private async Task CacheWorklogsAsync(JiraIssueResponse jiraIssuesResponse, CancellationToken cancellationToken)
        {
            foreach (var issue in jiraIssuesResponse.Issues)
            {
                List<Worklog> worklogs = await GetWorklogsAsync(issue.Id, cancellationToken);

                CachedWorklogs.Add(issue.Id, worklogs);
            }
        }
    }
}
