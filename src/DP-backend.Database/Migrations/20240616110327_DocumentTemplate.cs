using System;
using DP_backend.Domain.Templating;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateType = table.Column<string>(type: "text", nullable: false),
                    TemplateFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseTemplateContext = table.Column<TemplateContext>(type: "jsonb", nullable: true),
                    FieldIds = table.Column<string[]>(type: "text[]", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentTemplate_FileHandles_TemplateFileId",
                        column: x => x.TemplateFileId,
                        principalTable: "FileHandles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_TemplateFileId",
                table: "DocumentTemplate",
                column: "TemplateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_TemplateType",
                table: "DocumentTemplate",
                column: "TemplateType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTemplate");
        }
    }
}
