using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDayIssues.Model
{
    /// <summary>
    /// Represent list issues response from API request.
    /// </summary>
    public class ResponseObject
    {
        /// <summary>
        /// Gets or sets issues;
        /// </summary>
        [JsonProperty]
        public List<Issue> Issues { get; set; }
    }
}
