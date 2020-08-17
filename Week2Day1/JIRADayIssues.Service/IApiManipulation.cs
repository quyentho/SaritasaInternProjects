﻿using System;
using System.Collections.Generic;
using System.Text;
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
        IRestRequest ConfigureGetIssuesRequest(DateTime date);

        /// <summary>
        /// Configure request to get worklogs on specific issue.
        /// </summary>
        /// <param name="issueId">Issue to get worklogs.</param>
        /// <returns>Request after config.</returns>
        IRestRequest ConfigureGetWorklogRequest(string issueId);

        /// <summary>
        /// Get response after request with authentication.
        /// </summary>
        /// <param name="request">Request configured.</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <returns>Instance of IRestResponse.</returns>
        IRestResponse GetResponse(IRestRequest request, string username, string token);
    }
}
