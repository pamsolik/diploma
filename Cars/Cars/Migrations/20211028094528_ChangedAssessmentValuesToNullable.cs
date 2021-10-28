using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cars.Migrations
{
    public partial class ChangedAssessmentValuesToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CodeQualityAssessments_CodeQualityAssessmentId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "CodeQualityAssessmentId",
                table: "Applications",
                newName: "CodeOverallQualityId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_CodeQualityAssessmentId",
                table: "Applications",
                newName: "IX_Applications_CodeOverallQualityId");

            migrationBuilder.AddColumn<int>(
                name: "CodeQualityAssessmentId",
                table: "Projects",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Violations",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "SqaleRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "SecurityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "SecurityHotspots",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "ReliabilityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "OverallRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Lines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "DuplicatedLinesDensity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "DuplicatedLines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Coverage",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Complexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "CognitiveComplexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Bugs",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<float>(
                name: "CodeSmells",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Maintainability",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CodeOverallQuality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    ProjectsCount = table.Column<int>(type: "integer", nullable: false),
                    CompletedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CodeSmells = table.Column<float>(type: "real", nullable: true),
                    Maintainability = table.Column<float>(type: "real", nullable: true),
                    Coverage = table.Column<float>(type: "real", nullable: true),
                    CognitiveComplexity = table.Column<float>(type: "real", nullable: true),
                    Violations = table.Column<float>(type: "real", nullable: true),
                    SecurityRating = table.Column<float>(type: "real", nullable: true),
                    DuplicatedLines = table.Column<float>(type: "real", nullable: true),
                    Lines = table.Column<float>(type: "real", nullable: true),
                    DuplicatedLinesDensity = table.Column<float>(type: "real", nullable: true),
                    Bugs = table.Column<float>(type: "real", nullable: true),
                    SqaleRating = table.Column<float>(type: "real", nullable: true),
                    ReliabilityRating = table.Column<float>(type: "real", nullable: true),
                    Complexity = table.Column<float>(type: "real", nullable: true),
                    SecurityHotspots = table.Column<float>(type: "real", nullable: true),
                    OverallRating = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeOverallQuality", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CodeQualityAssessmentId",
                table: "Projects",
                column: "CodeQualityAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CodeOverallQuality_CodeOverallQualityId",
                table: "Applications",
                column: "CodeOverallQualityId",
                principalTable: "CodeOverallQuality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_CodeQualityAssessments_CodeQualityAssessmentId",
                table: "Projects",
                column: "CodeQualityAssessmentId",
                principalTable: "CodeQualityAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CodeOverallQuality_CodeOverallQualityId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_CodeQualityAssessments_CodeQualityAssessmentId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "CodeOverallQuality");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CodeQualityAssessmentId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CodeQualityAssessmentId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CodeSmells",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "Maintainability",
                table: "CodeQualityAssessments");

            migrationBuilder.RenameColumn(
                name: "CodeOverallQualityId",
                table: "Applications",
                newName: "CodeQualityAssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_CodeOverallQualityId",
                table: "Applications",
                newName: "IX_Applications_CodeQualityAssessmentId");

            migrationBuilder.AlterColumn<float>(
                name: "Violations",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "SqaleRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "SecurityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "SecurityHotspots",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "ReliabilityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "OverallRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Lines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DuplicatedLinesDensity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DuplicatedLines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Coverage",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Complexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "CognitiveComplexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Bugs",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CodeQualityAssessments_CodeQualityAssessmentId",
                table: "Applications",
                column: "CodeQualityAssessmentId",
                principalTable: "CodeQualityAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
