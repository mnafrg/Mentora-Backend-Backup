using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RebuildCommunitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_community_post_likes_users_UserId",
                table: "community_post_likes");

            migrationBuilder.DropForeignKey(
                name: "FK_community_post_saves_users_UserId",
                table: "community_post_saves");

            migrationBuilder.DropForeignKey(
                name: "FK_community_post_shares_users_SharedByUserId",
                table: "community_post_shares");

            migrationBuilder.DropIndex(
                name: "IX_topic_tasks_TopicId",
                table: "topic_tasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "community_posts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "community_comments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "communities");

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityCommentId",
                table: "community_reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityPostId",
                table: "community_reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DomainId",
                table: "communities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DomainId1",
                table: "communities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_topic_tasks_TopicId",
                table: "topic_tasks",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_CommunityCommentId",
                table: "community_reports",
                column: "CommunityCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_community_reports_CommunityPostId",
                table: "community_reports",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_communities_DomainId",
                table: "communities",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_communities_DomainId1",
                table: "communities",
                column: "DomainId1");

            migrationBuilder.AddForeignKey(
                name: "FK_communities_domains_DomainId",
                table: "communities",
                column: "DomainId",
                principalTable: "domains",
                principalColumn: "domain_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_communities_domains_DomainId1",
                table: "communities",
                column: "DomainId1",
                principalTable: "domains",
                principalColumn: "domain_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_likes_users_UserId",
                table: "community_post_likes",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_saves_users_UserId",
                table: "community_post_saves",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_shares_users_SharedByUserId",
                table: "community_post_shares",
                column: "SharedByUserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_community_reports_community_comments_CommunityCommentId",
                table: "community_reports",
                column: "CommunityCommentId",
                principalTable: "community_comments",
                principalColumn: "CommunityCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_community_reports_community_posts_CommunityPostId",
                table: "community_reports",
                column: "CommunityPostId",
                principalTable: "community_posts",
                principalColumn: "CommunityPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_communities_domains_DomainId",
                table: "communities");

            migrationBuilder.DropForeignKey(
                name: "FK_communities_domains_DomainId1",
                table: "communities");

            migrationBuilder.DropForeignKey(
                name: "FK_community_post_likes_users_UserId",
                table: "community_post_likes");

            migrationBuilder.DropForeignKey(
                name: "FK_community_post_saves_users_UserId",
                table: "community_post_saves");

            migrationBuilder.DropForeignKey(
                name: "FK_community_post_shares_users_SharedByUserId",
                table: "community_post_shares");

            migrationBuilder.DropForeignKey(
                name: "FK_community_reports_community_comments_CommunityCommentId",
                table: "community_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_community_reports_community_posts_CommunityPostId",
                table: "community_reports");

            migrationBuilder.DropIndex(
                name: "IX_topic_tasks_TopicId",
                table: "topic_tasks");

            migrationBuilder.DropIndex(
                name: "IX_community_reports_CommunityCommentId",
                table: "community_reports");

            migrationBuilder.DropIndex(
                name: "IX_community_reports_CommunityPostId",
                table: "community_reports");

            migrationBuilder.DropIndex(
                name: "IX_communities_DomainId",
                table: "communities");

            migrationBuilder.DropIndex(
                name: "IX_communities_DomainId1",
                table: "communities");

            migrationBuilder.DropColumn(
                name: "CommunityCommentId",
                table: "community_reports");

            migrationBuilder.DropColumn(
                name: "CommunityPostId",
                table: "community_reports");

            migrationBuilder.DropColumn(
                name: "DomainId",
                table: "communities");

            migrationBuilder.DropColumn(
                name: "DomainId1",
                table: "communities");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "community_posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "community_comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "communities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_topic_tasks_TopicId",
                table: "topic_tasks",
                column: "TopicId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_likes_users_UserId",
                table: "community_post_likes",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_saves_users_UserId",
                table: "community_post_saves",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_community_post_shares_users_SharedByUserId",
                table: "community_post_shares",
                column: "SharedByUserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
