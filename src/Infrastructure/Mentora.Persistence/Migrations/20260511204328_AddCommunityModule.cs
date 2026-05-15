using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "communities",
                columns: table => new
                {
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communities", x => x.CommunityId);
                    table.ForeignKey(
                        name: "FK_communities_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_members",
                columns: table => new
                {
                    CommunityMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_members", x => x.CommunityMemberId);
                    table.ForeignKey(
                        name: "FK_community_members_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "CommunityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_posts",
                columns: table => new
                {
                    CommunityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_posts", x => x.CommunityPostId);
                    table.ForeignKey(
                        name: "FK_community_posts_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "CommunityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_posts_users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_comments",
                columns: table => new
                {
                    CommunityCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_comments", x => x.CommunityCommentId);
                    table.ForeignKey(
                        name: "FK_community_comments_community_posts_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "community_posts",
                        principalColumn: "CommunityPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_post_likes",
                columns: table => new
                {
                    CommunityPostLikeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_post_likes", x => x.CommunityPostLikeId);
                    table.ForeignKey(
                        name: "FK_community_post_likes_community_posts_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "community_posts",
                        principalColumn: "CommunityPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_post_likes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_post_saves",
                columns: table => new
                {
                    CommunityPostSaveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_post_saves", x => x.CommunityPostSaveId);
                    table.ForeignKey(
                        name: "FK_community_post_saves_community_posts_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "community_posts",
                        principalColumn: "CommunityPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_post_saves_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_post_shares",
                columns: table => new
                {
                    CommunityPostShareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_post_shares", x => x.CommunityPostShareId);
                    table.ForeignKey(
                        name: "FK_community_post_shares_community_posts_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "community_posts",
                        principalColumn: "CommunityPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_post_shares_users_SharedByUserId",
                        column: x => x.SharedByUserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "community_reports",
                columns: table => new
                {
                    CommunityReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportReason = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_reports", x => x.CommunityReportId);
                    table.ForeignKey(
                        name: "FK_community_reports_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "CommunityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_reports_community_comments_TargetCommentId",
                        column: x => x.TargetCommentId,
                        principalTable: "community_comments",
                        principalColumn: "CommunityCommentId");
                    table.ForeignKey(
                        name: "FK_community_reports_community_posts_TargetPostId",
                        column: x => x.TargetPostId,
                        principalTable: "community_posts",
                        principalColumn: "CommunityPostId");
                    table.ForeignKey(
                        name: "FK_community_reports_users_ReporterUserId",
                        column: x => x.ReporterUserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_community_reports_users_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_communities_CreatedByUserId",
                table: "communities",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_comments_CommunityPostId",
                table: "community_comments",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_community_comments_UserId",
                table: "community_comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_members_CommunityId",
                table: "community_members",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_community_members_CommunityId_UserId",
                table: "community_members",
                columns: new[] { "CommunityId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_members_UserId",
                table: "community_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_post_likes_CommunityPostId_UserId",
                table: "community_post_likes",
                columns: new[] { "CommunityPostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_post_likes_UserId",
                table: "community_post_likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_post_saves_CommunityPostId_UserId",
                table: "community_post_saves",
                columns: new[] { "CommunityPostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_post_saves_UserId",
                table: "community_post_saves",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_post_shares_CommunityPostId",
                table: "community_post_shares",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_community_post_shares_SharedByUserId",
                table: "community_post_shares",
                column: "SharedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_posts_AuthorUserId",
                table: "community_posts",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_posts_CommunityId",
                table: "community_posts",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_CommunityId",
                table: "community_reports",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_ReporterUserId",
                table: "community_reports",
                column: "ReporterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_ReviewedByUserId",
                table: "community_reports",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_TargetCommentId",
                table: "community_reports",
                column: "TargetCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_TargetPostId",
                table: "community_reports",
                column: "TargetPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_members");

            migrationBuilder.DropTable(
                name: "community_post_likes");

            migrationBuilder.DropTable(
                name: "community_post_saves");

            migrationBuilder.DropTable(
                name: "community_post_shares");

            migrationBuilder.DropTable(
                name: "community_reports");

            migrationBuilder.DropTable(
                name: "community_comments");

            migrationBuilder.DropTable(
                name: "community_posts");

            migrationBuilder.DropTable(
                name: "communities");
        }
    }
}
