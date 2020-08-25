using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OmdbScrubber.Models
{
    public class Movie : MovieBaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public int RuntimeMins { get; set; }

        public DateTime CreateAt { get; set; }

        public IList<MovieActor> MovieActors { get; set; }
    }
}
