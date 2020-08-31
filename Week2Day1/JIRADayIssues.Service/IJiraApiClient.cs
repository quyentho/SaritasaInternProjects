// <copyright file="IApiManipulation.cs" company="Saritasa, LLC">
// copyright (c) Saritasa, LLC
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JiraDayIssues.Model;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Interface for API manipulation.
    /// </summary>
    public interface IJiraApiClient
    {
        /// <summary>
        /// Configure request to get issues on specific date.
        /// </summary>
        /// <param name="date">Date to get issues.</param>
        /// <returns>Request after config.</returns>
       Task<JiraIssueResponse> GetIssuesAsync(DateTime date, string worklogAuthor, CancellationToken cancellationToken);

        /// <summary>
        /// Configure request to get worklogs on specific issue.
        /// </summary>
        /// <param name="issueId">Issue to get worklogs.</param>
        /// <returns>Request after config.</returns>
       Task<List<Worklog>> GetWorklogsAsync(string issueId, CancellationToken cancellationToken);
    }
}
