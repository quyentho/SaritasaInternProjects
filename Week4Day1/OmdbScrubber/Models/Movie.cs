using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }

        public string ImdbId { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int RuntimeMins { get; set; }

        public decimal ImdbRating { get; set; }

        public DateTime CreateAt { get; set; }

        public IList<MovieActor> MovieActors { get; set; }
    }
}
