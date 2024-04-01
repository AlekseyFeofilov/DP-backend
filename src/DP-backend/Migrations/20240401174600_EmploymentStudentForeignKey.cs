using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class EmploymentStudentForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Employment_EmploymentId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_EmploymentId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "EmploymentId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Student");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Employment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId",
                table: "Employment",
                column: "StudentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Student_StudentId",
                table: "Employment",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Student_StudentId",
                table: "Employment");

            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Employment");

            migrationBuilder.AddColumn<Guid>(
                name: "EmploymentId",
                table: "Student",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Student",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Student_EmploymentId",
                table: "Student",
                column: "EmploymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Employment_EmploymentId",
                table: "Student",
                column: "EmploymentId",
                principalTable: "Employment",
                principalColumn: "Id");
        }
    }
}
