using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class addGradesToRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "InternshipDiaryRequests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "CourseWorkRequests",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "CourseWorkRequests");
        }
    }
}
