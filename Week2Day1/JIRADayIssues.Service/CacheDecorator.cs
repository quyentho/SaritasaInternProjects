// <copyright file="CacheDecorator.cs" company="Saritasa, LLC">
// copyright (c) Saritasa, LLC
// </copyright>

using NLog;
using RestSharp;

namespace JiraDayIssues.Service
{
    /// <summary>
    /// Decorator for caching service.
    /// </summary>
    public class CacheDecorator : ApiManipulationDecorator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDecorator"/> class.
        /// </summary>
        /// <param name="apiManipulation">Api Manipulation interface to DI.</param>
        public CacheDecorator(IApiManipulation apiManipulation)
            : base(apiManipulation)
        {
        }

        /// <summary>
        /// Caching request.
        /// </summary>
        /// <param name="request">Request to caching.</param>
        public override void PostProcess(IRestRequest request)
        {
            logger.Info($"Make request.");
            logger.Trace("Request: {resquest}", request);
        }

        /// <summary>
        /// Caching response.
        /// </summary>
        /// <param name="response">Response to caching.</param>
        public override void PostProcess(IRestResponse response)
        {
            logger.Info("Get response.");
            logger.Trace("Response: {response}", response);
        }
    }
}
