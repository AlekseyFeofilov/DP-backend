using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class employmentRequestsAndRequestsConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentVariant_Employer_Employer_EmployerId",
                table: "EmploymentVariant");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentVariant_Employer_EmployerId",
                table: "EmploymentVariant");

            migrationBuilder.DropColumn(
                name: "Employer_CustomCompanyName",
                table: "EmploymentVariant");

            migrationBuilder.DropColumn(
                name: "Employer_EmployerId",
                table: "EmploymentVariant");

            migrationBuilder.AddColumn<Guid>(
                name: "InternshipRequestId",
                table: "EmploymentVariant",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentVariant_InternshipRequestId",
                table: "EmploymentVariant",
                column: "InternshipRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentVariant_InternshipRequests_InternshipRequestId",
                table: "EmploymentVariant",
                column: "InternshipRequestId",
                principalTable: "InternshipRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentVariant_InternshipRequests_InternshipRequestId",
                table: "EmploymentVariant");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentVariant_InternshipRequestId",
                table: "EmploymentVariant");

            migrationBuilder.DropColumn(
                name: "InternshipRequestId",
                table: "EmploymentVariant");

            migrationBuilder.AddColumn<string>(
                name: "Employer_CustomCompanyName",
                table: "EmploymentVariant",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Employer_EmployerId",
                table: "EmploymentVariant",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentVariant_Employer_EmployerId",
                table: "EmploymentVariant",
                column: "Employer_EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentVariant_Employer_Employer_EmployerId",
                table: "EmploymentVariant",
                column: "Employer_EmployerId",
                principalTable: "Employer",
                principalColumn: "Id");
        }
    }
}
