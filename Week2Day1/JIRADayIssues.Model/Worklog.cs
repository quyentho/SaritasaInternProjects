using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JiraDayIssues.Model
{
    public class Worklog
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("timeSpent")]
        public string TimeSpent { get; set; }
    }
}
