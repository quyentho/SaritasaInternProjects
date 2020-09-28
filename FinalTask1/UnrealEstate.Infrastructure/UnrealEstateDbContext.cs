using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Infrastructure
{
    public class UnrealEstateDbContext : IdentityDbContext<ApplicationUser>
    {
        public UnrealEstateDbContext(DbContextOptions<UnrealEstateDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ListingNote> ListingNotes { get; set; }

        public DbSet<ListingPhoto> ListingPhoTos { get; set; }

        public DbSet<ListingStatus> ListingStatuses { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server =.; Database=UnrealEstate;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}