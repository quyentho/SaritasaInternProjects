using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace JIRADayIssues.Model
{
    /// <summary>
    /// Represent issues come from request.
    /// </summary>
    public class Issue
    {
        [JsonProperty("fields")]
        public Field Field { get; set; }
    }
}