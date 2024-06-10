using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class EntityTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_TargetEntityId",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "TargetEntityId",
                table: "Comment",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "TargetEntityType",
                table: "Comment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EntityType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Usage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_TargetEntityType_TargetEntityId",
                table: "Comment",
                columns: new[] { "TargetEntityType", "TargetEntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityType");

            migrationBuilder.DropIndex(
                name: "IX_Comment_TargetEntityType_TargetEntityId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "TargetEntityType",
                table: "Comment");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetEntityId",
                table: "Comment",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_TargetEntityId",
                table: "Comment",
                column: "TargetEntityId");
        }
    }
}
