using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace JiraDayIssues.Model
{
    /// <summary>
    /// Represent the project contains the issue.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets name of the project.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}