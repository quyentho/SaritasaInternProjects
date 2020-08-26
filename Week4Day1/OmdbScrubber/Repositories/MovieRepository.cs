using AutoMapper;
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

        public List<Movie> Movies { get; private set; }

        public async Task<List<Movie>> GetMovies(string input)
        {
            var imdbIds = input.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var moviesFromDb = _context.Movies.ToList();

            // TODO: Use Except


            RestClient client = new RestClient("https://www.omdbapi.com/");

            for (int i = 0; i < imdbIds.Length; i++)
            {
                IRestResponse response = await GetResponse(client, imdbIds[i]);

                MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);

                if (movieResponse.ImdbId != null)
                {
                    Movie movie = _mapper.Map<Movie>(movieResponse);
                    SaveMovies(movie);
                    Movies.Add(movie);
                }
            }

            return Movies;
        }

        public async Task SaveMovies(List<Movie> movies)
        {
            foreach (var movie in movies)
            {
                if (!_context.Movies.Any(m => m.ImdbId == movie.ImdbId)) // Check if not duplicate.
                {
                    _context.Movies.Add(movie);
                }
            }

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
    }
}

