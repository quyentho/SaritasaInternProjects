// <copyright file="IApiManipulation.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Interface for API manipulation.
    /// </summary>
    public interface IApiManipulation
    {
        /// <summary>
        /// Configure request to get issues on specific date.
        /// </summary>
        /// <param name="date">Date to get issues.</param>
        /// <returns>Request after config.</returns>
        IRestRequest ConfigureIssuesRequest(DateTime date);

        /// <summary>
        /// Configure request to get worklogs on specific issue.
        /// </summary>
        /// <param name="issueId">Issue to get worklogs.</param>
        /// <returns>Request after config.</returns>
        IRestRequest ConfigureWorklogsRequest(string issueId);

        /// <summary>
        /// Get response after request with authentication, provide capability to cancel request.
        /// </summary>
        /// <param name="request">Request configured.</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <param name="cancellationToken">Token to cancel request.</param>
        /// <returns>Instance of IRestResponse.</returns>
        Task<IRestResponse> GetResponseAsync(IRestRequest request, string username, string token, CancellationToken cancellationToken);
    }
}
