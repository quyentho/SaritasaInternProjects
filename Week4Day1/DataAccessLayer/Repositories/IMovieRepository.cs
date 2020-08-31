using OmdbScrubber.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmdbScrubber.Repositories
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMovies(List<string> imdbIds);

        Task SaveMovie(Movie movie);
    }
}