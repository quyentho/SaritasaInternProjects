using AutoMapper;
using Newtonsoft.Json;
using OmdbScrubber.ApiClient;
using OmdbScrubber.Models;
using RestSharp;
using System.Threading.Tasks;

namespace DataAccessLayer.ApiClient
{
    public class ImdbApiClient : IApiClient
    {
        private IMapper _mapper;

        public ImdbApiClient(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <inheritdoc/>
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

        private Task<IRestResponse> GetResponse(RestClient client, string imdbId)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("i", imdbId);
            request.AddParameter("apikey", "19e14e4");

            return client.ExecuteAsync(request);
        }
    }
}
