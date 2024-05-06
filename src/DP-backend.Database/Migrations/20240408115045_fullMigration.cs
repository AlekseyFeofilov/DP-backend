using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class fullMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Employer_Employer_EmployerId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Employment_EmploymentId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "EmploymentId",
                table: "Student",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_EmploymentId",
                table: "Student",
                newName: "IX_Student_GroupId");

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

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Employment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "Employment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Vacancy",
                table: "Employment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId",
                table: "Employment",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employment_StudentId1",
                table: "Employment",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Employer_EmployerId",
                table: "Employment",
                column: "EmployerId",
                principalTable: "Employer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Student_StudentId",
                table: "Employment",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Student_StudentId1",
                table: "Employment",
                column: "StudentId1",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Groups_GroupId",
                table: "Student",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Employer_EmployerId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Student_StudentId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Student_StudentId1",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Groups_GroupId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId",
                table: "Employment");

            migrationBuilder.DropIndex(
                name: "IX_Employment_StudentId1",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Employment");

            migrationBuilder.DropColumn(
                name: "Vacancy",
                table: "Employment");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Student",
                newName: "EmploymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_GroupId",
                table: "Student",
                newName: "IX_Student_EmploymentId");

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Student",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Employment_EmploymentId",
                table: "Student",
                column: "EmploymentId",
                principalTable: "Employment",
                principalColumn: "Id");
        }
    }
}
