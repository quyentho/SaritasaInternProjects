using OmdbScrubber.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmdbScrubber.Repositories
{
    public interface IMovieRepository
    {
        /// <summary>
        /// Gets Movies exist in database.
        /// </summary>
        /// <param name="input">ids input by user.</param>
        /// <returns>List of movies found.</returns>
        Task<List<Movie>> GetMovies(List<string> imdbIds);


        /// <summary>
        /// Save changes to database.
        /// </summary>
        /// <returns>Task represent save action.</returns>
        Task<int> Save();

        /// <summary>
        /// Add movie to context change tracker.
        /// </summary>
        /// <param name="movie">Movie to add.</param>
        void AddMovieToContext(Movie movie);
    }
}