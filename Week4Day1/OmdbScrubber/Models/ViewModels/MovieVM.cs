using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models.ViewModels
{
    public class MovieVM
    {
        public List<Movie> Movies { get; set; }

        public decimal? RatingAbove { get; set; }

        public int? RuntimeMinsAbove { get; set; }

        public int? RuntimeMinsBelow { get; set; }

        public string? HasActor { get; set; }
    }
}
