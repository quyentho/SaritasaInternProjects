using OmdbScrubber.Models;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmdbScrubber.Extensions
{
    public static class FilterExtension
    {

        /// <summary>
        /// Filter movies by criteria provided.
        /// </summary>
        /// <param name="movies">Class to extend.</param>
        /// <param name="filterCriteria">Criteria to filter.</param>
        /// <returns>List movies filtered.</returns>
        public static List<Movie> Filter(this List<Movie> movies, FilterCriteria filterCriteria)
        {
            if (filterCriteria.RatingAbove.HasValue)
            {
                movies = GetMoviesRatingAbove(movies, filterCriteria.RatingAbove);
            }

            if (filterCriteria.RuntimeMinsAbove.HasValue)
            {
                movies = GetMoviesAboveRuntimeMins(movies, filterCriteria.RuntimeMinsAbove);
            }

            if (filterCriteria.RuntimeMinsBelow.HasValue)
            {
                movies = GetMoviesBelowRuntimeMins(movies, filterCriteria.RuntimeMinsBelow);
            }

            if (filterCriteria.ActorName.HasValue())
            {
                movies = GetMoviesHasActor(movies, filterCriteria.ActorName);
            }

            return movies;
        }

        private static List<Movie> GetMoviesBelowRuntimeMins(List<Movie> movies, int? runtimeMinsBelow)
            => movies.FindAll(m => m.RuntimeMins <= runtimeMinsBelow);

        private static List<Movie> GetMoviesAboveRuntimeMins(List<Movie> movies, int? runtimeMinsAbove)
            => movies.FindAll(m => m.RuntimeMins >= runtimeMinsAbove);

        private static List<Movie> GetMoviesHasActor(List<Movie> movies, string hasActor)
            => movies.FindAll(m => m.MovieActors.Select(ma => ma.Actor.Name).Contains(hasActor));

        private static List<Movie> GetMoviesRatingAbove(List<Movie> movies, decimal? ratingAbove)
            => movies.FindAll(m => m.ImdbRating >= ratingAbove);
    }
}
