using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class changeEmployment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Employer_Employer_EmployerId",
                table: "Employment");

            migrationBuilder.RenameColumn(
                name: "Employer_EmployerId",
                table: "Employment",
                newName: "EmployerId");

            migrationBuilder.RenameColumn(
                name: "Employer_CustomCompanyName",
                table: "Employment",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Employment_Employer_EmployerId",
                table: "Employment",
                newName: "IX_Employment_EmployerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployerId",
                table: "Employment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vacancy",
                table: "Employment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Employer_EmployerId",
                table: "Employment",
                column: "EmployerId",
                principalTable: "Employer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Employer_EmployerId",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "Vacancy",
                table: "Employment");

            migrationBuilder.RenameColumn(
                name: "EmployerId",
                table: "Employment",
                newName: "Employer_EmployerId");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Employment",
                newName: "Employer_CustomCompanyName");

            migrationBuilder.RenameIndex(
                name: "IX_Employment_EmployerId",
                table: "Employment",
                newName: "IX_Employment_Employer_EmployerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Employer_EmployerId",
                table: "Employment",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Employer_Employer_EmployerId",
                table: "Employment",
                column: "Employer_EmployerId",
                principalTable: "Employer",
                principalColumn: "Id");
        }
    }
}
