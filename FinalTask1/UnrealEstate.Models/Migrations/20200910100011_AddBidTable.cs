using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UnrealEstate.Models.Migrations
{
    public partial class AddBidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bid",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(15,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bid", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bid");
        }
    }
}
