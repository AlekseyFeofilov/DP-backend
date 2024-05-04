using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class newEmploymentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tutor",
                table: "Employment",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tutor",
                table: "Employment");
        }
    }
}
