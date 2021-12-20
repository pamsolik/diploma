using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cars.Migrations
{
    public partial class QualityPropertiesAndLanguageTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CodeOverallQuality_CodeOverallQualityId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "CodeOverallQuality");

            migrationBuilder.RenameColumn(
                name: "SqaleRating",
                table: "CodeQualityAssessments",
                newName: "Tests");

            migrationBuilder.RenameColumn(
                name: "Maintainability",
                table: "CodeQualityAssessments",
                newName: "TechnicalDebt");

            migrationBuilder.RenameColumn(
                name: "Lines",
                table: "CodeQualityAssessments",
                newName: "MaintainabilityRating");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Recruitments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "Technology",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expiration",
                table: "PersistedGrants",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PersistedGrants",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConsumedTime",
                table: "PersistedGrants",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expiration",
                table: "DeviceCodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "DeviceCodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "CodeQualityAssessments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CodeQualityAssessments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "LinesOfCode",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectsCount",
                table: "CodeQualityAssessments",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<int>(
                name: "ClauseOptAccepted",
                table: "Applications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "ClauseOpt2Accepted",
                table: "Applications",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Use = table.Column<string>(type: "text", nullable: true),
                    Algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "boolean", nullable: false),
                    DataProtected = table.Column<bool>(type: "boolean", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants",
                column: "ConsumedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Use",
                table: "Keys",
                column: "Use");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CodeQualityAssessments_CodeOverallQualityId",
                table: "Applications",
                column: "CodeOverallQualityId",
                principalTable: "CodeQualityAssessments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CodeQualityAssessments_CodeOverallQualityId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "Technology",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "LinesOfCode",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "ProjectsCount",
                table: "CodeQualityAssessments");

            migrationBuilder.RenameColumn(
                name: "Tests",
                table: "CodeQualityAssessments",
                newName: "SqaleRating");

            migrationBuilder.RenameColumn(
                name: "TechnicalDebt",
                table: "CodeQualityAssessments",
                newName: "Maintainability");

            migrationBuilder.RenameColumn(
                name: "MaintainabilityRating",
                table: "CodeQualityAssessments",
                newName: "Lines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Recruitments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expiration",
                table: "PersistedGrants",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PersistedGrants",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConsumedTime",
                table: "PersistedGrants",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Expiration",
                table: "DeviceCodes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "DeviceCodes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedTime",
                table: "CodeQualityAssessments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Applications",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "ClauseOptAccepted",
                table: "Applications",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "ClauseOpt2Accepted",
                table: "Applications",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "CodeOverallQuality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bugs = table.Column<float>(type: "real", nullable: true),
                    CodeSmells = table.Column<float>(type: "real", nullable: true),
                    CognitiveComplexity = table.Column<float>(type: "real", nullable: true),
                    CompletedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Complexity = table.Column<float>(type: "real", nullable: true),
                    Coverage = table.Column<float>(type: "real", nullable: true),
                    DuplicatedLines = table.Column<float>(type: "real", nullable: true),
                    DuplicatedLinesDensity = table.Column<float>(type: "real", nullable: true),
                    Lines = table.Column<float>(type: "real", nullable: true),
                    Maintainability = table.Column<float>(type: "real", nullable: true),
                    OverallRating = table.Column<float>(type: "real", nullable: true),
                    ProjectsCount = table.Column<int>(type: "integer", nullable: false),
                    ReliabilityRating = table.Column<float>(type: "real", nullable: true),
                    SecurityHotspots = table.Column<float>(type: "real", nullable: true),
                    SecurityRating = table.Column<float>(type: "real", nullable: true),
                    SqaleRating = table.Column<float>(type: "real", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    Violations = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeOverallQuality", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CodeOverallQuality_CodeOverallQualityId",
                table: "Applications",
                column: "CodeOverallQualityId",
                principalTable: "CodeOverallQuality",
                principalColumn: "Id");
        }
    }
}
