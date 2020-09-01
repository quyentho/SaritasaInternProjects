using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
