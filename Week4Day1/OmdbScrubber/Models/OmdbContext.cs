﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models
{
    public class OmdbContext : DbContext
    {
        public OmdbContext() : base()
        {

        }

        public OmdbContext(DbContextOptions<OmdbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(c => new { c.MovieId, c.ActorId });
            modelBuilder.Entity<Movie>().Property(m => m.ImdbRating).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Movie>().Property(m => m.ReleaseDate).HasColumnType("date");
        }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieActor> MovieActors { get; set; }
    }
}
