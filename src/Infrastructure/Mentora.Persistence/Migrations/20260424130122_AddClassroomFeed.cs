using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomFeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classroom_posts",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    classroom_id = table.Column<int>(type: "int", nullable: false),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    is_pinned = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classroom_posts", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_classroom_posts_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalTable: "classrooms",
                        principalColumn: "classroom_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classroom_posts_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classroom_comments",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parent_comment_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classroom_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_classroom_comments_classroom_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "classroom_comments",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK_classroom_comments_classroom_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "classroom_posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classroom_comments_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classroom_post_likes",
                columns: table => new
                {
                    like_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    post_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classroom_post_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_classroom_post_likes_classroom_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "classroom_posts",
                        principalColumn: "post_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classroom_post_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classroom_comment_likes",
                columns: table => new
                {
                    like_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classroom_comment_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_classroom_comment_likes_classroom_comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "classroom_comments",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classroom_comment_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classroom_comment_likes_comment_id_user_id",
                table: "classroom_comment_likes",
                columns: new[] { "comment_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classroom_comment_likes_user_id",
                table: "classroom_comment_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_comments_author_id",
                table: "classroom_comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_comments_parent_comment_id",
                table: "classroom_comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_comments_post_id",
                table: "classroom_comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_post_likes_post_id_user_id",
                table: "classroom_post_likes",
                columns: new[] { "post_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classroom_post_likes_user_id",
                table: "classroom_post_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_posts_author_id",
                table: "classroom_posts",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_classroom_posts_classroom_id_is_pinned_created_at",
                table: "classroom_posts",
                columns: new[] { "classroom_id", "is_pinned", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classroom_comment_likes");

            migrationBuilder.DropTable(
                name: "classroom_post_likes");

            migrationBuilder.DropTable(
                name: "classroom_comments");

            migrationBuilder.DropTable(
                name: "classroom_posts");
        }
    }
}
