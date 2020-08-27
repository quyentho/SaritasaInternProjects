using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OmdbScrubber.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly OmdbContext _context;
        private readonly IMapper _mapper;

        public MovieRepository(OmdbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets Movies from database, query from api if not exist and save to database.
        /// </summary>
        /// <param name="input">ids input by user.</param>
        /// <returns>List of movies found.</returns>
        public async Task<List<Movie>> GetMovies(string input)
        {
            var imdbIds = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(id => id.Trim()).ToList();

            var moviesFromDb = _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .ToList();

            var newMovieIds = imdbIds.Except(moviesFromDb.Select(m => m.ImdbId)).ToList(); // List movie ids not exist in database.

            var movies = new List<Movie>();
            if (newMovieIds.Count > 0)
            {
                movies = await GetMoviesByApiCall(newMovieIds);
            }

            GetMoviesFromDb(imdbIds, moviesFromDb, movies);

            return movies;
        }

        public List<Movie> GetMoviesFiltered(List<Movie> movies, FilterCriterials filterCriterials)
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

        private async Task SaveMovies(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        private static async Task<IRestResponse> GetResponse(RestClient client, string imdbId)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("i", imdbId);
            request.AddParameter("apikey", "19e14e4");

            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        private static void GetMoviesFromDb(IEnumerable<string> imdbIds, List<Movie> moviesFromDb, List<Movie> movies)
        {
            movies.AddRange(moviesFromDb.FindAll(m => imdbIds.Contains(m.ImdbId)));
        }

        private async Task<List<Movie>> GetMoviesByApiCall(IEnumerable<string> newMovieIds)
        {
            RestClient client = new RestClient("https://www.omdbapi.com/");

            var movies = new List<Movie>();
            foreach (var id in newMovieIds)
            {
                IRestResponse response = await GetResponse(client, id);

                MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);

                if (movieResponse.ImdbId != null) // Valid movie response.
                {
                    Movie movie = _mapper.Map<Movie>(movieResponse); // map response value to movie model.

                    await SaveMovies(movie);

                    movies.Add(movie);
                }
            }

            return movies;
        }
    }
}

