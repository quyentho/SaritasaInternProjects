// <copyright file="ResponseObject.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represent list issues response from API request.
    /// </summary>
    public class JiraIssueResponse
    {
        /// <summary>
        /// Gets or sets issues.
        /// </summary>
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }
    }
}
