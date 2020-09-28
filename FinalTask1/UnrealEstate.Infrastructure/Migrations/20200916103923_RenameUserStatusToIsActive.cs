using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class RenameUserStatusToIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ListingPhotos_Listings_ListingId",
                "ListingPhotos");

            migrationBuilder.DropPrimaryKey(
                "PK_ListingPhotos",
                "ListingPhotos");

            migrationBuilder.DropColumn(
                "Status",
                "AspNetUsers");

            migrationBuilder.RenameTable(
                "ListingPhotos",
                newName: "ListingPhoTos");

            migrationBuilder.RenameIndex(
                "IX_ListingPhotos_ListingId",
                table: "ListingPhoTos",
                newName: "IX_ListingPhoTos_ListingId");

            migrationBuilder.AddColumn<bool>(
                "IsActive",
                "AspNetUsers",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                "PK_ListingPhoTos",
                "ListingPhoTos",
                "Id");

            migrationBuilder.AddForeignKey(
                "FK_ListingPhoTos_Listings_ListingId",
                "ListingPhoTos",
                "ListingId",
                "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_ListingPhoTos_Listings_ListingId",
                "ListingPhoTos");

            migrationBuilder.DropPrimaryKey(
                "PK_ListingPhoTos",
                "ListingPhoTos");

            migrationBuilder.DropColumn(
                "IsActive",
                "AspNetUsers");

            migrationBuilder.RenameTable(
                "ListingPhoTos",
                newName: "ListingPhotos");

            migrationBuilder.RenameIndex(
                "IX_ListingPhoTos_ListingId",
                table: "ListingPhotos",
                newName: "IX_ListingPhotos_ListingId");

            migrationBuilder.AddColumn<bool>(
                "Status",
                "AspNetUsers",
                "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                "PK_ListingPhotos",
                "ListingPhotos",
                "Id");

            migrationBuilder.AddForeignKey(
                "FK_ListingPhotos_Listings_ListingId",
                "ListingPhotos",
                "ListingId",
                "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}