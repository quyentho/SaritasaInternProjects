// <copyright file="ApiManipulationDecorator.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using RestSharp;

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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IRestResponse> GetResponseAsync(IRestRequest request, string username, string token, CancellationToken cancellationToken)
        {
            IRestResponse response = await this.apiManipulation.GetResponseAsync(request, username, token, cancellationToken);

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
