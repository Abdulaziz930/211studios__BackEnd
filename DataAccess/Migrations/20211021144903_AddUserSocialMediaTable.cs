using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddUserSocialMediaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserDetailSocials");

            migrationBuilder.CreateTable(
                name: "UserSocialMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AppUserDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSocialMedias_AppUserDetails_AppUserDetailId",
                        column: x => x.AppUserDetailId,
                        principalTable: "AppUserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialMedias_AppUserDetailId",
                table: "UserSocialMedias",
                column: "AppUserDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSocialMedias");

            migrationBuilder.CreateTable(
                name: "AppUserDetailSocials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserDetailId = table.Column<int>(type: "int", nullable: false),
                    SocialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserDetailSocials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserDetailSocials_AppUserDetails_AppUserDetailId",
                        column: x => x.AppUserDetailId,
                        principalTable: "AppUserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserDetailSocials_Socials_SocialId",
                        column: x => x.SocialId,
                        principalTable: "Socials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDetailSocials_AppUserDetailId",
                table: "AppUserDetailSocials",
                column: "AppUserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDetailSocials_SocialId",
                table: "AppUserDetailSocials",
                column: "SocialId");
        }
    }
}
