using System;
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
        /// Add parameters for resquest.
        /// </summary>
        /// <param name="date">Date to query on request to API.</param>
        /// <returns>Instance of IRequest.</returns>
        IRestRequest ConfigureRequest(DateTime date);

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
