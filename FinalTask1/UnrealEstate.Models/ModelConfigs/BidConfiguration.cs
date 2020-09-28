using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Infrastructure.ModelConfigs
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.Property(b => b.Price).HasColumnType("decimal(15,2)").HasDefaultValue(0);
        }
    }
}
