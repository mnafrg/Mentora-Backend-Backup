using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskSubmissionSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "task_submissions",
                columns: table => new
                {
                    submission_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_id = table.Column<int>(type: "int", nullable: false),
                    mentee_profile_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    notes_for_mentor = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_submissions", x => x.submission_id);
                    table.ForeignKey(
                        name: "FK_task_submissions_mentee_profile_mentee_profile_id",
                        column: x => x.mentee_profile_id,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_submissions_topic_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "topic_tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission_links",
                columns: table => new
                {
                    link_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    submission_id = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submission_links", x => x.link_id);
                    table.ForeignKey(
                        name: "FK_submission_links_task_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "task_submissions",
                        principalColumn: "submission_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission_reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    submission_id = table.Column<int>(type: "int", nullable: false),
                    mentor_profile_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    feedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsRevisionRequest = table.Column<bool>(type: "bit", nullable: false),
                    reviewed_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submission_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_submission_reviews_mentor_profile_mentor_profile_id",
                        column: x => x.mentor_profile_id,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_submission_reviews_task_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "task_submissions",
                        principalColumn: "submission_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_submission_links_submission_id",
                table: "submission_links",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_submission_reviews_mentor_profile_id",
                table: "submission_reviews",
                column: "mentor_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_submission_reviews_submission_id",
                table: "submission_reviews",
                column: "submission_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_submissions_mentee_profile_id",
                table: "task_submissions",
                column: "mentee_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_submissions_task_id_mentee_profile_id",
                table: "task_submissions",
                columns: new[] { "task_id", "mentee_profile_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "submission_links");

            migrationBuilder.DropTable(
                name: "submission_reviews");

            migrationBuilder.DropTable(
                name: "task_submissions");
        }
    }
}
