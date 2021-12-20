using Microsoft.EntityFrameworkCore.Migrations;

namespace Cars.Migrations
{
    public partial class AddedClausesToApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClFile",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ClauseOpt2Accepted",
                table: "Applications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ClauseOptAccepted",
                table: "Applications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClFile",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ClauseOpt2Accepted",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ClauseOptAccepted",
                table: "Applications");
        }
    }
}
