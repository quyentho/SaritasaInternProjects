using Newtonsoft.Json;

namespace JiraDayIssues.Model
{
    /// <summary>
    /// Represent issues come from request.
    /// </summary>
    public class Issue
    {
        [JsonProperty("fields")]
        public Field Field { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}