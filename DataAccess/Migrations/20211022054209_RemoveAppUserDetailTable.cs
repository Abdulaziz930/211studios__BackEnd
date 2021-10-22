using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemoveAppUserDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialMedias_AppUserDetails_AppUserDetailId",
                table: "UserSocialMedias");

            migrationBuilder.DropTable(
                name: "AppUserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialMedias_AppUserDetailId",
                table: "UserSocialMedias");

            migrationBuilder.DropColumn(
                name: "AppUserDetailId",
                table: "UserSocialMedias");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "UserSocialMedias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialMedias_AppUserId",
                table: "UserSocialMedias",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialMedias_AspNetUsers_AppUserId",
                table: "UserSocialMedias",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialMedias_AspNetUsers_AppUserId",
                table: "UserSocialMedias");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialMedias_AppUserId",
                table: "UserSocialMedias");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserSocialMedias");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AppUserDetailId",
                table: "UserSocialMedias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppUserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserDetails_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialMedias_AppUserDetailId",
                table: "UserSocialMedias",
                column: "AppUserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDetails_AppUserId",
                table: "AppUserDetails",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialMedias_AppUserDetails_AppUserDetailId",
                table: "UserSocialMedias",
                column: "AppUserDetailId",
                principalTable: "AppUserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
