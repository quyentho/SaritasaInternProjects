﻿using Newtonsoft.Json;
using System;

namespace OmdbScrubber.Models
{
    public abstract class MovieBaseModel
    {
        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Released")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("imdbRating")]
        public decimal ImdbRating { get; set; }
    }
}
