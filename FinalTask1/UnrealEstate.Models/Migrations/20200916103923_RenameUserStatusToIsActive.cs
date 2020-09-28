using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class RenameUserStatusToIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingPhotos_Listings_ListingId",
                table: "ListingPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingPhotos",
                table: "ListingPhotos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ListingPhotos",
                newName: "ListingPhoTos");

            migrationBuilder.RenameIndex(
                name: "IX_ListingPhotos_ListingId",
                table: "ListingPhoTos",
                newName: "IX_ListingPhoTos_ListingId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingPhoTos",
                table: "ListingPhoTos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingPhoTos_Listings_ListingId",
                table: "ListingPhoTos",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingPhoTos_Listings_ListingId",
                table: "ListingPhoTos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingPhoTos",
                table: "ListingPhoTos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ListingPhoTos",
                newName: "ListingPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_ListingPhoTos_ListingId",
                table: "ListingPhotos",
                newName: "IX_ListingPhotos_ListingId");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingPhotos",
                table: "ListingPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingPhotos_Listings_ListingId",
                table: "ListingPhotos",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
