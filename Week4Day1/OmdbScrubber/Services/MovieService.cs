using DataAccessLayer.ApiClient;
using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.ApiClient;
using OmdbScrubber.Models;
using OmdbScrubber.Repositories;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesLayer
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IApiClient _apiClient;

        public MovieService(IMovieRepository movieRepository, IApiClient apiClient)
        {
            this._movieRepository = movieRepository;
            this._apiClient = apiClient;
        }

        /// <inheritdoc />
        public async Task<List<Movie>> GetMovies(string input)
        {
            List<string> imdbIds = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(id => id.Trim()).ToList();

            var movies = await _movieRepository.GetMovies(imdbIds).ConfigureAwait(false); // Get movies exist in database.

            var notFoundMovieIds = imdbIds.Except(movies.Select(m => m.ImdbId)).ToList(); // List movie ids not exist in database.

            if (notFoundMovieIds.Count > 0)
            {
                await GetMoviesFromInternetAndSaveToDatabase(movies, notFoundMovieIds).ConfigureAwait(false);
            }

            return movies;
        }

        private async Task GetMoviesFromInternetAndSaveToDatabase(List<Movie> movies, List<string> notFoundMovieIds)
        {
            foreach (var id in notFoundMovieIds) // Make api call if not found in database.
            {
                Movie movie = await _apiClient.GetMovie(id).ConfigureAwait(false);

                if (movie != null) // valid movie.
                {
                    movies.Add(movie);
                    _movieRepository.AddMovieToContext(movie);
                }
            }

            await _movieRepository.Save().ConfigureAwait(false);
        }

       
    }
}
