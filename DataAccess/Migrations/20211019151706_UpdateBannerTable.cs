using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateBannerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BannerId",
                table: "Studios",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Studios_BannerId",
                table: "Studios",
                column: "BannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Studios_Banners_BannerId",
                table: "Studios",
                column: "BannerId",
                principalTable: "Banners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studios_Banners_BannerId",
                table: "Studios");

            migrationBuilder.DropIndex(
                name: "IX_Studios_BannerId",
                table: "Studios");

            migrationBuilder.DropColumn(
                name: "BannerId",
                table: "Studios");
        }
    }
}
