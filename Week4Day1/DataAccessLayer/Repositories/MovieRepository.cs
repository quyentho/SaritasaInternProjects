using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Movie>> GetMovies(List<string> imdbIds)
        {

            var moviesFromDb = _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .Where(m=> imdbIds.Contains(m.ImdbId))
                .ToListAsync();

            return await moviesFromDb;
        }

        public async Task SaveMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
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

     
    }
}

