using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cars.Migrations
{
    public partial class AddedForeighKeysToSkillsAndExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cites_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId1",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruitments_Cites_CityId",
                table: "Recruitments");

            migrationBuilder.DropForeignKey(
                name: "FK_TextModel_Applications_RecruitmentApplicationId",
                table: "TextModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TextModel_AspNetUsers_ApplicationUserId",
                table: "TextModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experience",
                table: "Experience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cites",
                table: "Cites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextModel",
                table: "TextModel");

            migrationBuilder.DropIndex(
                name: "IX_TextModel_ApplicationUserId",
                table: "TextModel");

            migrationBuilder.DropIndex(
                name: "IX_TextModel_RecruitmentApplicationId",
                table: "TextModel");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TextModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TextModel");

            migrationBuilder.DropColumn(
                name: "RecruitmentApplicationId",
                table: "TextModel");

            migrationBuilder.RenameTable(
                name: "Experience",
                newName: "Experiences");

            migrationBuilder.RenameTable(
                name: "Cites",
                newName: "Cities");

            migrationBuilder.RenameTable(
                name: "TextModel",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_ApplicationUserId1",
                table: "Experiences",
                newName: "IX_Experiences_ApplicationUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Experience_ApplicationUserId",
                table: "Experiences",
                newName: "IX_Experiences_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Experiences",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Projects",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiences",
                table: "Experiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ApplicationId",
                table: "Projects",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ApplicationUserId",
                table: "Skills",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId",
                table: "Experiences",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId1",
                table: "Experiences",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Applications_ApplicationId",
                table: "Projects",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recruitments_Cities_CityId",
                table: "Recruitments",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AspNetUsers_ApplicationUserId1",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Applications_ApplicationId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruitments_Cities_CityId",
                table: "Recruitments");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiences",
                table: "Experiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ApplicationId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Experiences",
                newName: "Experience");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "Cites");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "TextModel");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_ApplicationUserId1",
                table: "Experience",
                newName: "IX_Experience_ApplicationUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Experiences_ApplicationUserId",
                table: "Experience",
                newName: "IX_Experience_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Experience",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "TextModel",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TextModel",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TextModel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TextModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentApplicationId",
                table: "TextModel",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experience",
                table: "Experience",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cites",
                table: "Cites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextModel",
                table: "TextModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TextModel_ApplicationUserId",
                table: "TextModel",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TextModel_RecruitmentApplicationId",
                table: "TextModel",
                column: "RecruitmentApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cites_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "Cites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId",
                table: "Experience",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_AspNetUsers_ApplicationUserId1",
                table: "Experience",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recruitments_Cites_CityId",
                table: "Recruitments",
                column: "CityId",
                principalTable: "Cites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextModel_Applications_RecruitmentApplicationId",
                table: "TextModel",
                column: "RecruitmentApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TextModel_AspNetUsers_ApplicationUserId",
                table: "TextModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
