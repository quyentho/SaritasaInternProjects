using DataAccessLayer.ApiClient;
using OmdbScrubber.Models;
using OmdbScrubber.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesLayer
{
    public class MovieServies
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ApiClient _apiClient;

        public MovieServies(IMovieRepository movieRepository, ApiClient apiClient)
        {
            this._movieRepository = movieRepository;
            this._apiClient = apiClient;
        }

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
    }
}
