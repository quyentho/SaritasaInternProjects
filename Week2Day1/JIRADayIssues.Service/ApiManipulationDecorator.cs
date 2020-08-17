using System;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Decorator for ApiManipulation class.
    /// </summary>
    public abstract class ApiManipulationDecorator : IApiManipulation
    {
        private readonly IApiManipulation apiManipulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManipulationDecorator"/> class.
        /// </summary>
        /// <param name="apiManipulation">Api Manipulation interface to DI.</param>
        public ApiManipulationDecorator(IApiManipulation apiManipulation)
        {
            this.apiManipulation = apiManipulation;
        }

        /// <summary>
        /// Add parameters for resquest.
        /// </summary>
        /// <param name="date">Date to query on request to API.</param>
        /// <returns>Instance of IRequest.</returns>
        public IRestRequest ConfigureGetIssuesRequest(DateTime date)
        {
            IRestRequest request = this.apiManipulation.ConfigureGetIssuesRequest(date);

            this.PostProcess(request);

            return request;
        }

        /// <summary>
        /// Get response after request with authentication.
        /// </summary>
        /// <param name="request">Request configured.</param>
        /// <param name="username">Username to authentication.</param>
        /// <param name="token">Token to authentication.</param>
        /// <returns>Instance of IRestResponse.</returns>
        public IRestResponse GetResponse(IRestRequest request, string username, string token)
        {
            IRestResponse response = this.apiManipulation.GetResponse(request, username, token);

            this.PostProcess(response);

            return response;
        }

        /// <summary>
        /// Post process after request.
        /// </summary>
        /// <param name="request">Request for post process.</param>
        public abstract void PostProcess(IRestRequest request);

        /// <summary>
        /// Post process after response.
        /// </summary>
        /// <param name="response">response for post process.</param>
        public abstract void PostProcess(IRestResponse response);
    }
}
