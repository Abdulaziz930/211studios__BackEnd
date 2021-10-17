using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdateBioTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialLink",
                table: "Bios",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SocialIcon",
                table: "Bios",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Bios");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Bios",
                newName: "SocialLink");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Bios",
                newName: "SocialIcon");
        }
    }
}
