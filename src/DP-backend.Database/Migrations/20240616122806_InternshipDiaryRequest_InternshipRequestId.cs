using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class InternshipDiaryRequest_InternshipRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InternshipRequestId",
                table: "InternshipDiaryRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("b2aa13ae-764a-49d5-88f1-5cdfa510a507"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
