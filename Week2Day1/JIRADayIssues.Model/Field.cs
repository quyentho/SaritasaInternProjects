using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIRADayIssues.Model
{
    public class Field
    {
        /// <summary>
        /// Gets or sets the project contains issue.
        /// </summary>
        [JsonProperty("project")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets timespent on the issue.
        /// </summary>
        [JsonProperty("timespent")]
        public double TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets summary of the issue.
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("timeoriginalestimate")]
        public string EstimateTime { get; set; }
    }
}
