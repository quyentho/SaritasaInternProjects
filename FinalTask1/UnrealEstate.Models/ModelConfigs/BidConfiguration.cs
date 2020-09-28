using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Models.Models;

namespace UnrealEstate.Models.ModelConfigs
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.Property(b => b.Price).HasColumnType("decimal(15,2)").HasDefaultValue(0);
        }
    }
}
