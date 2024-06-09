using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class addStudentToRequestCourseWorkAndIntershipDiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InternshipDiaryRequests_StudentId",
                table: "InternshipDiaryRequests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseWorkRequests_StudentId",
                table: "CourseWorkRequests",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseWorkRequests_Student_StudentId",
                table: "CourseWorkRequests",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipDiaryRequests_Student_StudentId",
                table: "InternshipDiaryRequests",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseWorkRequests_Student_StudentId",
                table: "CourseWorkRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipDiaryRequests_Student_StudentId",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropIndex(
                name: "IX_InternshipDiaryRequests_StudentId",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropIndex(
                name: "IX_CourseWorkRequests_StudentId",
                table: "CourseWorkRequests");
        }
    }
}
