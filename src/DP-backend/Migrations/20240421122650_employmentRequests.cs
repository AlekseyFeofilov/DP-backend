using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class employmentRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId",
                table: "Employment");

            migrationBuilder.CreateTable(
                name: "EmploymentRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternshipRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploymentRequests_InternshipRequests_InternshipRequestId",
                        column: x => x.InternshipRequestId,
                        principalTable: "InternshipRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploymentRequests_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId",
                table: "Employment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentRequests_InternshipRequestId",
                table: "EmploymentRequests",
                column: "InternshipRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentRequests_StudentId",
                table: "EmploymentRequests",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmploymentRequests");

            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId",
                table: "Employment");

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId",
                table: "Employment",
                column: "StudentId",
                unique: true);
        }
    }
}
