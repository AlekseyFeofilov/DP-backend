using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class gradeToMarkChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "CourseWorkRequests");

            migrationBuilder.AddColumn<float>(
                name: "Mark",
                table: "InternshipDiaryRequests",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Mark",
                table: "CourseWorkRequests",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "CourseWorkRequests");

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
    }
}
