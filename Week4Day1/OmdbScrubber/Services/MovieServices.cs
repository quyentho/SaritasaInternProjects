using DataAccessLayer.ApiClient;
using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.ApiClient;
using OmdbScrubber.Models;
using OmdbScrubber.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesLayer
{
    public class MovieServices : IMovieServices
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IApiClient _apiClient;

        public MovieServices(IMovieRepository movieRepository, IApiClient apiClient)
        {
            this._movieRepository = movieRepository;
            this._apiClient = apiClient;
        }

        /// <inheritdoc />
        public async Task<List<Movie>> GetMovies(string input)
        {
            List<string> imdbIds = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(id => id.Trim()).ToList();

            var movies = await _movieRepository.GetMovies(imdbIds); // Get movies exist in database.

            var newMovieIds = imdbIds.Except(movies.Select(m => m.ImdbId)).ToList(); // List movie ids not exist in database.

            foreach (var id in newMovieIds) // Make api call if not found in database.
            {
                Movie movie = await _apiClient.GetMovie(id);


                if (movie != null) // valid movie.
                {
                    movies.Add(movie);
                    await _movieRepository.SaveMovie(movie);
                }
            }

            return movies;
        }

        /// <inheritdoc></inheritdoc>/>
        public List<Movie> GetMoviesFiltered(List<Movie> movies, FilterCriterial filterCriterials)
        {
            if (filterCriterials.RatingAbove != null)
            {
                movies = GetMoviesRatingAbove(movies, filterCriterials.RatingAbove);
            }

            if (filterCriterials.RuntimeMinsAbove != null)
            {
                movies = GetMoviesAboveRuntimeMins(movies, filterCriterials.RuntimeMinsAbove);
            }

            if (filterCriterials.RuntimeMinsBelow != null)
            {
                movies = GetMoviesBelowRuntimeMins(movies, filterCriterials.RuntimeMinsBelow);
            }

            if (filterCriterials.ActorName != null)
            {
                movies = GetMoviesHasActor(movies, filterCriterials.ActorName);
            }

            return movies;
        }

        private List<Movie> GetMoviesBelowRuntimeMins(List<Movie> movies, int? runtimeMinsBelow)
            => movies.FindAll(m => m.RuntimeMins <= runtimeMinsBelow);

        private List<Movie> GetMoviesAboveRuntimeMins(List<Movie> movies, int? runtimeMinsAbove)
            => movies.FindAll(m => m.RuntimeMins >= runtimeMinsAbove);

        private List<Movie> GetMoviesHasActor(List<Movie> movies, string hasActor)
            => movies.FindAll(m => m.MovieActors.Select(ma => ma.Actor.Name).Contains(hasActor));

        private List<Movie> GetMoviesRatingAbove(List<Movie> movies, decimal? ratingAbove)
            => movies.FindAll(m => m.ImdbRating >= ratingAbove);
    }
}
