using OmdbScrubber.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.ApiClient
{
    public interface IApiClient
    {
        /// <summary>
        /// Makes Api call to get movie, parse in correct type to return.
        /// </summary>
        /// <param name="imdbId">imdb Id to pass as api parameter. </param>
        /// <returns>Movie by default, null if not found</returns>
        Task<Movie> GetMovie(string imdbId);

    }
}
