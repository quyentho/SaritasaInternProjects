using LinqKit;
using OmdbScrubber.Models;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OmdbScrubber.Extensions
{
    public static class MovieExtension
    {

        /// <summary>
        /// Filter movies by criteria provided.
        /// </summary>
        /// <param name="movies">Class to extend.</param>
        /// <param name="filterCriteria">Criteria to filter.</param>
        /// <returns>List movies filtered.</returns>
        public static List<Movie> Filter(this List<Movie> movies, FilterCriteria filterCriteria)
        {
            var searchConditions = GetSearchConditions(filterCriteria);

            return movies.AsQueryable().Where(searchConditions).ToList();
        }

        private static Expression<Func<Movie, bool>> GetSearchConditions(FilterCriteria filterCriteria)
        {
            var searchConditions = PredicateBuilder.New<Movie>();

            if (filterCriteria.RuntimeMinsBelow.HasValue)
            {
                searchConditions.And(m => m.RuntimeMins <= filterCriteria.RuntimeMinsBelow);
            }
            if (filterCriteria.RuntimeMinsAbove.HasValue)
            {
                searchConditions.And(m => m.RuntimeMins > filterCriteria.RuntimeMinsAbove);
            }
            if (filterCriteria.ActorName.HasValue())
            {
                searchConditions.And(m => m.MovieActors.Select(ma => ma.Actor.Name).Contains(filterCriteria.ActorName));
            }

            return searchConditions;
        }
    }
}

