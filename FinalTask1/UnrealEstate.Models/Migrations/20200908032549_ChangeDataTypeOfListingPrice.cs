using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class ChangeDataTypeOfListingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StatingPrice",
                table: "Listings",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "StatingPrice",
                table: "Listings",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
