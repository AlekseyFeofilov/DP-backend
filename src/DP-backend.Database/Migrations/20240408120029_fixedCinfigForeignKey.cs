using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class fixedCinfigForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Student_StudentId1",
                table: "Employment");

            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId1",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Employment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "Employment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId1",
                table: "Employment",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Student_StudentId1",
                table: "Employment",
                column: "StudentId1",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
