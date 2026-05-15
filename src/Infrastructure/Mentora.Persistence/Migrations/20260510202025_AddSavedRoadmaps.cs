using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedRoadmaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "roadmap_Phases");

            migrationBuilder.AddColumn<string>(
                name: "TaskAttachmentUrl",
                table: "topic_tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "programs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProgramImageUrl",
                table: "programs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "saved_roadmaps",
                columns: table => new
                {
                    SavedRoadmapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_roadmaps", x => x.SavedRoadmapId);
                    table.ForeignKey(
                        name: "FK_saved_roadmaps_roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "roadmaps",
                        principalColumn: "RoadmapId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saved_roadmaps_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_saved_roadmaps_RoadmapId",
                table: "saved_roadmaps",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_saved_roadmaps_UserId",
                table: "saved_roadmaps",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "saved_roadmaps");

            migrationBuilder.DropColumn(
                name: "TaskAttachmentUrl",
                table: "topic_tasks");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "programs");

            migrationBuilder.DropColumn(
                name: "ProgramImageUrl",
                table: "programs");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "topics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "roadmap_Phases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
