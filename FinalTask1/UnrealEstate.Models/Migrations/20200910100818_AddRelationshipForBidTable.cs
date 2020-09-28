using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class AddRelationshipForBidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bid_ListingId",
                table: "Bid",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bid_UserId",
                table: "Bid",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bid_Listings_ListingId",
                table: "Bid",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bid_AspNetUsers_UserId",
                table: "Bid",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bid_Listings_ListingId",
                table: "Bid");

            migrationBuilder.DropForeignKey(
                name: "FK_Bid_AspNetUsers_UserId",
                table: "Bid");

            migrationBuilder.DropIndex(
                name: "IX_Bid_ListingId",
                table: "Bid");

            migrationBuilder.DropIndex(
                name: "IX_Bid_UserId",
                table: "Bid");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bid",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
