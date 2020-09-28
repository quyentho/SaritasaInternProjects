using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class AddRelationshipForBidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "UserId",
                "Bid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                "IX_Bid_ListingId",
                "Bid",
                "ListingId");

            migrationBuilder.CreateIndex(
                "IX_Bid_UserId",
                "Bid",
                "UserId");

            migrationBuilder.AddForeignKey(
                "FK_Bid_Listings_ListingId",
                "Bid",
                "ListingId",
                "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Bid_AspNetUsers_UserId",
                "Bid",
                "UserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Bid_Listings_ListingId",
                "Bid");

            migrationBuilder.DropForeignKey(
                "FK_Bid_AspNetUsers_UserId",
                "Bid");

            migrationBuilder.DropIndex(
                "IX_Bid_ListingId",
                "Bid");

            migrationBuilder.DropIndex(
                "IX_Bid_UserId",
                "Bid");

            migrationBuilder.AlterColumn<string>(
                "UserId",
                "Bid",
                "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}