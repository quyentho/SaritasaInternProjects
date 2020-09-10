using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Models.Migrations
{
    public partial class AddHighestPriceForListing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentHighestBidPrice",
                table: "Listings",
                type: "decimal(15,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHighestBidPrice",
                table: "Listings");
        }
    }
}
