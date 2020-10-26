using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class AddHighestPriceForListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                "CurrentHighestBidPrice",
                "Listings",
                "decimal(15,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CurrentHighestBidPrice",
                "Listings");
        }
    }
}