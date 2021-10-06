using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cars.Migrations
{
    public partial class AddedClausesQualityAndCities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Applications_RecruitmentId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Recruitments_RecruitmentId1",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RecruitmentId1",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "RecruitmentId1",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Recruitments",
                newName: "ClauseRequired");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Recruitments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClauseOpt1",
                table: "Recruitments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClauseOpt2",
                table: "Recruitments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Applications",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10000)",
                oldMaxLength: 10000);

            migrationBuilder.AddColumn<string>(
                name: "CvFile",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Projects",
                table: "Applications",
                type: "text[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeQualityAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeQualityAssessments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recruitments_CityId",
                table: "Recruitments",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Recruitments_RecruitmentId",
                table: "Applications",
                column: "RecruitmentId",
                principalTable: "Recruitments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recruitments_Cites_CityId",
                table: "Recruitments",
                column: "CityId",
                principalTable: "Cites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Recruitments_RecruitmentId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruitments_Cites_CityId",
                table: "Recruitments");

            migrationBuilder.DropTable(
                name: "Cites");

            migrationBuilder.DropTable(
                name: "CodeQualityAssessments");

            migrationBuilder.DropIndex(
                name: "IX_Recruitments_CityId",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "ClauseOpt1",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "ClauseOpt2",
                table: "Recruitments");

            migrationBuilder.DropColumn(
                name: "CvFile",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Projects",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "ClauseRequired",
                table: "Recruitments",
                newName: "City");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Recruitments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Recruitments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Applications",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruitmentId1",
                table: "Applications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Applications",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RecruitmentId1",
                table: "Applications",
                column: "RecruitmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Applications_RecruitmentId",
                table: "Applications",
                column: "RecruitmentId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Recruitments_RecruitmentId1",
                table: "Applications",
                column: "RecruitmentId1",
                principalTable: "Recruitments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
