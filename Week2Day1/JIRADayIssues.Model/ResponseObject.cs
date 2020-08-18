// <copyright file="ResponseObject.cs" company="Saritasa, LLC">
// copyright Saritasa, LLC
// </copyright>

namespace JiraDayIssues.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Represent list issues response from API request.
    /// </summary>
    public class ResponseObject
    {
        /// <summary>
        /// Gets or sets issues.
        /// </summary>
        [JsonProperty]
        public List<Issue> Issues { get; set; }

        /// <summary>
        /// Gets or sets worklogs.
        /// </summary>
        [JsonProperty("worklogs")]
        public List<Worklog> Worklogs { get; set; }
    }
}
