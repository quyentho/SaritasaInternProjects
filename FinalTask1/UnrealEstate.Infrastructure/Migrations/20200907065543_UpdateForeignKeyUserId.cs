using Microsoft.EntityFrameworkCore.Migrations;

namespace UnrealEstate.Infrastructure.Migrations
{
    public partial class UpdateForeignKeyUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Comments_AspNetUsers_UserId1",
                "Comments");

            migrationBuilder.DropIndex(
                "IX_Comments_UserId1",
                "Comments");

            migrationBuilder.DropColumn(
                "UserId1",
                "Comments");

            migrationBuilder.AlterColumn<string>(
                "UserId",
                "Comments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                "IX_Comments_UserId",
                "Comments",
                "UserId");

            migrationBuilder.AddForeignKey(
                "FK_Comments_AspNetUsers_UserId",
                "Comments",
                "UserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Comments_AspNetUsers_UserId",
                "Comments");

            migrationBuilder.DropIndex(
                "IX_Comments_UserId",
                "Comments");

            migrationBuilder.AlterColumn<int>(
                "UserId",
                "Comments",
                "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                "UserId1",
                "Comments",
                "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_Comments_UserId1",
                "Comments",
                "UserId1");

            migrationBuilder.AddForeignKey(
                "FK_Comments_AspNetUsers_UserId1",
                "Comments",
                "UserId1",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}