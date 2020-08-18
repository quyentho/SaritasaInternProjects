using System;
using System.Threading.Tasks;
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
        public IRestRequest ConfigureIssuesRequest(DateTime date)
        {
            IRestRequest request = this.apiManipulation.ConfigureIssuesRequest(date);

            this.PostProcess(request);

            return request;
        }

        /// <inheritdoc/>
        public IRestRequest ConfigureWorklogsRequest(string issueId)
        {
            IRestRequest request = this.apiManipulation.ConfigureWorklogsRequest(issueId);

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
        public async Task<IRestResponse> GetResponseAsync(IRestRequest request, string username, string token)
        {
            IRestResponse response = await this.apiManipulation.GetResponseAsync(request, username, token);

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
