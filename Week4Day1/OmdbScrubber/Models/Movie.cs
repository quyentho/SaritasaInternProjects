using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmdbScrubber.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
