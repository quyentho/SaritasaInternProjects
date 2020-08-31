using OmdbScrubber.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLayer
{
    public interface IMovieService
    {
        /// <summary>
        /// Gets movies according to imdb ids input . If not found movies in database, make api call to gets from network.
        /// </summary>
        /// <param name="input">Imdb Ids input by user.</param>
        /// <returns>List of movies found.</returns>
        Task<List<Movie>> GetMovies(string input);
    }
}