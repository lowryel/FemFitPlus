using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FemFitPlus.Migrations
{
    /// <inheritdoc />
    public partial class ProfileTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HeightCm",
                table: "Profiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WeightKg",
                table: "Profiles",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightCm",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "Profiles");
        }
    }
}
