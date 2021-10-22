using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUserDetails_AppUserId",
                table: "AppUserDetails");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDetails_AppUserId",
                table: "AppUserDetails",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUserDetails_AppUserId",
                table: "AppUserDetails");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDetails_AppUserId",
                table: "AppUserDetails",
                column: "AppUserId");
        }
    }
}
