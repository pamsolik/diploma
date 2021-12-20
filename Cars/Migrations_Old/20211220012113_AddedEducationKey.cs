using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cars.Migrations
{
    public partial class AddedEducationKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId1",
                table: "Experiences");

            migrationBuilder.DropIndex(
                name: "IX_Experiences_ApplicationUserId1",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Experiences");

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    SmallDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    DateFrom = table.Column<string>(type: "text", nullable: false),
                    DateTo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_ApplicationUserId",
                table: "Educations",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Experiences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Experiences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_ApplicationUserId1",
                table: "Experiences",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId1",
                table: "Experiences",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
