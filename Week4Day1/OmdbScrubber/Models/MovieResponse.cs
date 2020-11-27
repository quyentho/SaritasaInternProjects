using Newtonsoft.Json;

namespace OmdbScrubber.Models
{
    public class MovieResponse : MovieBaseModel
    {
        [JsonProperty("Runtime")]
        public string RuntimeMins { get; set; }

        [JsonProperty("Year")]
        public string CreateAt { get; set; }

        [JsonProperty("Actors")]
        public string MovieActors { get; set; }
    }
}
