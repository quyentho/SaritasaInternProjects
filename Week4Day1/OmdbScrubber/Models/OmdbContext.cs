using Microsoft.EntityFrameworkCore;

namespace OmdbScrubber.Models
{
    public class OmdbContext : DbContext
    {

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieActor> MovieActors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OmdbContext"/> class.
        /// </summary>
        public OmdbContext() 
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OmdbContext"/> class.
        /// </summary>
        /// <param name="options">Dbcontext options.</param>
        public OmdbContext(DbContextOptions<OmdbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });
           
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);
              
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            modelBuilder.Entity<Movie>().Property(m => m.ImdbRating).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Movie>().Property(m => m.ReleaseDate).HasColumnType("date");
        }

        
    }
}
