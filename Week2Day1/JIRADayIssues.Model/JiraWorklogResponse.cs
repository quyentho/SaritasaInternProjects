// <copyright file="ResponseObject.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represent list worklogs response from API request.
    /// </summary>
    public class JiraWorklogResponse
    {
        /// <summary>
        /// Gets or sets worklogs.
        /// </summary>
        [JsonProperty("worklogs")]
        public List<Worklog> Worklogs { get; set; }
    }
}
