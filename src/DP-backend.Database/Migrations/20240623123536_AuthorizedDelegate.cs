using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizedDelegate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipDiaryRequests_InternshipRequests_InternshipReques~",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropIndex(
                name: "IX_InternshipDiaryRequests_InternshipRequestId",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropColumn(
                name: "InternshipRequestId",
                table: "InternshipDiaryRequests");

            migrationBuilder.RenameColumn(
                name: "Tutor",
                table: "Employer",
                newName: "AuthorizedDelegate");

            migrationBuilder.AddColumn<string>(
                name: "ManagerFromEmployment",
                table: "InternshipDiaryRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentIndividualTask",
                table: "InternshipDiaryRequests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerFromEmployment",
                table: "InternshipDiaryRequests");

            migrationBuilder.DropColumn(
                name: "StudentIndividualTask",
                table: "InternshipDiaryRequests");

            migrationBuilder.RenameColumn(
                name: "AuthorizedDelegate",
                table: "Employer",
                newName: "Tutor");

            migrationBuilder.AddColumn<Guid>(
                name: "InternshipRequestId",
                table: "InternshipDiaryRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InternshipDiaryRequests_InternshipRequestId",
                table: "InternshipDiaryRequests",
                column: "InternshipRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipDiaryRequests_InternshipRequests_InternshipReques~",
                table: "InternshipDiaryRequests",
                column: "InternshipRequestId",
                principalTable: "InternshipRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
