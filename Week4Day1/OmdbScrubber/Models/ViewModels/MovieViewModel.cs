using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models.ViewModels
{
    public class MovieViewModel
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<Movie> Movies { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public decimal? RatingAbove { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int? RuntimeMinsAbove { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int? RuntimeMinsBelow { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string? HasActor { get; set; }

        public MovieViewModel()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<Movie> GetMovies()
        {
            return Movies;
        }
    }
}
