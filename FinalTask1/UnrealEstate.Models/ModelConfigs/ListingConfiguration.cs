using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Infrastructure.ModelConfigs
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.Property(l => l.StatusId).HasDefaultValue((int) Status.Active);

            builder.Property(l => l.StatingPrice).HasColumnType("decimal(15,2)").HasDefaultValue(0);

            builder.Property(l => l.CurrentHighestBidPrice).HasColumnType("decimal(15,2)").HasDefaultValue(0);
        }
    }
}