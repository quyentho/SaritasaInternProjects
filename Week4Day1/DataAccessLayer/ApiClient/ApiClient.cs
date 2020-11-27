using AutoMapper;
using Newtonsoft.Json;
using OmdbScrubber.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ApiClient
{
    public class ApiClient
    {
        private IMapper _mapper;

        public ApiClient(IMapper mapper)
        {
            _mapper = mapper;
        }

        private static async Task<IRestResponse> GetResponse(RestClient client, string imdbId)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("i", imdbId);
            request.AddParameter("apikey", "19e14e4");

            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        /// <summary>
        /// Make Api call to get movie, parse in correct type to return.
        /// </summary>
        /// <param name="imdbId">imdb Id to pass as api parameter. </param>
        /// <returns>Movie by default, null if not found</returns>
        public async Task<Movie> GetMovie(string imdbId)
        {
            RestClient client = new RestClient("https://www.omdbapi.com/");

            IRestResponse response = await GetResponse(client, imdbId);

            MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);

            if (movieResponse.ImdbId != null) // Valid movie response.
            {
                Movie movie = _mapper.Map<Movie>(movieResponse); // map response value to movie model.

                return movie;
            }

            return null;
        }
    }
}
