using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmdbScrubber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly OmdbContext _context;

        public MovieRepository(OmdbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Movie>> GetMovies(List<string> imdbIds)
        {

            var moviesFromDb = _context.Movies
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .Where(m => imdbIds.Contains(m.ImdbId))
                .ToListAsync();

            return await moviesFromDb;
        }

        /// <inheritdoc/>
        public async Task SaveMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
    }
}

