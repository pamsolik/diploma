using Microsoft.EntityFrameworkCore.Migrations;

namespace Cars.Migrations
{
    public partial class AddedCodeQualityProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Bugs",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CognitiveComplexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Complexity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Coverage",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DuplicatedLines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DuplicatedLinesDensity",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Lines",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OverallRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ReliabilityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SecurityHotspots",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SecurityRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SqaleRating",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Violations",
                table: "CodeQualityAssessments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bugs",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "CognitiveComplexity",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "Complexity",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "Coverage",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "DuplicatedLines",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "DuplicatedLinesDensity",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "Lines",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "OverallRating",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "ReliabilityRating",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "SecurityHotspots",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "SecurityRating",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "SqaleRating",
                table: "CodeQualityAssessments");

            migrationBuilder.DropColumn(
                name: "Violations",
                table: "CodeQualityAssessments");
        }
    }
}
