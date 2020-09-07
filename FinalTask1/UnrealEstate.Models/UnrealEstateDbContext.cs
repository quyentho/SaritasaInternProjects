using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace UnrealEstate.Models
{
    public class UnrealEstateDbContext : IdentityDbContext<User>
    {
        public UnrealEstateDbContext(DbContextOptions<UnrealEstateDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer(@"Server =.; Database=UnrealEstate;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ListingNote> ListingNotes { get; set; }

        public DbSet<ListingPhoTo> ListingPhoTos { get; set; }

        public DbSet<ListingStatus> ListingStatuses { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
    }
}
