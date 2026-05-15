using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialtoggleMentee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "material_completions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mentee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    material_id = table.Column<int>(type: "int", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_completions", x => x.id);
                    table.ForeignKey(
                        name: "FK_material_completions_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_material_completions_mentee_profile_mentee_id",
                        column: x => x.mentee_id,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_material_completions_material_id",
                table: "material_completions",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_completions_mentee_id_material_id",
                table: "material_completions",
                columns: new[] { "mentee_id", "material_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "material_completions");
        }
    }
}
