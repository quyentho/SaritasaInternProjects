using Microsoft.EntityFrameworkCore;

namespace UnrealEstate.Models
{
    public class UnrealEstateDbContext : DbContext
    {
        public UnrealEstateDbContext(DbContextOptions<UnrealEstateDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
    }
}
