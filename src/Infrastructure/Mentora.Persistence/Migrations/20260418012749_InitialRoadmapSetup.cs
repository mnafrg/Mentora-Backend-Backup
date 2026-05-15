using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoadmapSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roadmap_steps");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "roadmaps",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillDomainId",
                table: "roadmaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubDomainId",
                table: "roadmaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetLevelFrom",
                table: "roadmaps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetLevelTo",
                table: "roadmaps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "roadmap_Phases",
                columns: table => new
                {
                    RoadmapPhaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RoadmapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roadmap_Phases", x => x.RoadmapPhaseId);
                    table.ForeignKey(
                        name: "FK_roadmap_Phases_roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "roadmaps",
                        principalColumn: "RoadmapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roadmap_technologies",
                columns: table => new
                {
                    RoadmapId = table.Column<int>(type: "int", nullable: false),
                    TechnologyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roadmap_technologies", x => new { x.RoadmapId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_roadmap_technologies_roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "roadmaps",
                        principalColumn: "RoadmapId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roadmap_technologies_technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "technologies",
                        principalColumn: "technology_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RoadmapPhaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_topics_roadmap_Phases_RoadmapPhaseId",
                        column: x => x.RoadmapPhaseId,
                        principalTable: "roadmap_Phases",
                        principalColumn: "RoadmapPhaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaterialType = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_materials_topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topic_tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic_tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_topic_tasks_topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_roadmaps_SkillDomainId",
                table: "roadmaps",
                column: "SkillDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_roadmaps_SubDomainId",
                table: "roadmaps",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_materials_TopicId",
                table: "materials",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_roadmap_Phases_RoadmapId",
                table: "roadmap_Phases",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_roadmap_technologies_TechnologyId",
                table: "roadmap_technologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_topic_tasks_TopicId",
                table: "topic_tasks",
                column: "TopicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topics_RoadmapPhaseId",
                table: "topics",
                column: "RoadmapPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_roadmaps_domains_SkillDomainId",
                table: "roadmaps",
                column: "SkillDomainId",
                principalTable: "domains",
                principalColumn: "domain_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_roadmaps_subdomain_SubDomainId",
                table: "roadmaps",
                column: "SubDomainId",
                principalTable: "subdomain",
                principalColumn: "subdomain_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roadmaps_domains_SkillDomainId",
                table: "roadmaps");

            migrationBuilder.DropForeignKey(
                name: "FK_roadmaps_subdomain_SubDomainId",
                table: "roadmaps");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "roadmap_technologies");

            migrationBuilder.DropTable(
                name: "topic_tasks");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropTable(
                name: "roadmap_Phases");

            migrationBuilder.DropIndex(
                name: "IX_roadmaps_SkillDomainId",
                table: "roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_roadmaps_SubDomainId",
                table: "roadmaps");

            migrationBuilder.DropColumn(
                name: "SkillDomainId",
                table: "roadmaps");

            migrationBuilder.DropColumn(
                name: "SubDomainId",
                table: "roadmaps");

            migrationBuilder.DropColumn(
                name: "TargetLevelFrom",
                table: "roadmaps");

            migrationBuilder.DropColumn(
                name: "TargetLevelTo",
                table: "roadmaps");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "roadmaps",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "roadmap_steps",
                columns: table => new
                {
                    RoadmapStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoadmapId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roadmap_steps", x => x.RoadmapStepId);
                    table.ForeignKey(
                        name: "FK_roadmap_steps_roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "roadmaps",
                        principalColumn: "RoadmapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_roadmap_steps_RoadmapId",
                table: "roadmap_steps",
                column: "RoadmapId");
        }
    }
}
