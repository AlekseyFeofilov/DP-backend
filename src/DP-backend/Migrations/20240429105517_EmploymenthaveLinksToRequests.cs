using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class EmploymenthaveLinksToRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmploymentRequestId",
                table: "Employment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InternshipRequestId",
                table: "Employment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employment_EmploymentRequestId",
                table: "Employment",
                column: "EmploymentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Employment_InternshipRequestId",
                table: "Employment",
                column: "InternshipRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_EmploymentRequests_EmploymentRequestId",
                table: "Employment",
                column: "EmploymentRequestId",
                principalTable: "EmploymentRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_InternshipRequests_InternshipRequestId",
                table: "Employment",
                column: "InternshipRequestId",
                principalTable: "InternshipRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_EmploymentRequests_EmploymentRequestId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Employment_InternshipRequests_InternshipRequestId",
                table: "Employment");

            migrationBuilder.DropIndex(
                name: "IX_Employment_EmploymentRequestId",
                table: "Employment");

            migrationBuilder.DropIndex(
                name: "IX_Employment_InternshipRequestId",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "EmploymentRequestId",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "InternshipRequestId",
                table: "Employment");
        }
    }
}
