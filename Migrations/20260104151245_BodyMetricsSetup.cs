using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FemFitPlus.Migrations
{
    /// <inheritdoc />
    public partial class BodyMetricsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BMI",
                table: "BodyMetrics",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BodyFatPercentage",
                table: "BodyMetrics",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeasurementMethod",
                table: "BodyMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MuscleMassKg",
                table: "BodyMetrics",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMI",
                table: "BodyMetrics");

            migrationBuilder.DropColumn(
                name: "BodyFatPercentage",
                table: "BodyMetrics");

            migrationBuilder.DropColumn(
                name: "MeasurementMethod",
                table: "BodyMetrics");

            migrationBuilder.DropColumn(
                name: "MuscleMassKg",
                table: "BodyMetrics");
        }
    }
}
