using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsInEmployement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Employer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommunicationPlace",
                table: "Employer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Employer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlacesQuantity",
                table: "Employer",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Employer");

            migrationBuilder.DropColumn(
                name: "CommunicationPlace",
                table: "Employer");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Employer");

            migrationBuilder.DropColumn(
                name: "PlacesQuantity",
                table: "Employer");
        }
    }
}
