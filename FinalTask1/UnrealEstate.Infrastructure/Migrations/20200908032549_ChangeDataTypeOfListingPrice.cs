using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class ChangeDataTypeOfListingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                "StatingPrice",
                "Listings",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                "StatingPrice",
                "Listings",
                "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}