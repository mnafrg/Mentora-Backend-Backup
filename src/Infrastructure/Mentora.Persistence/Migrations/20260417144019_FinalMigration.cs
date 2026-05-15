using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mentora.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    achievement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievements", x => x.achievement_id);
                });

            migrationBuilder.CreateTable(
                name: "career_goal",
                columns: table => new
                {
                    career_goal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_career_goal", x => x.career_goal_id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    country_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "domains",
                columns: table => new
                {
                    domain_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domains", x => x.domain_id);
                });

            migrationBuilder.CreateTable(
                name: "learning_style",
                columns: table => new
                {
                    learning_style_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_learning_style", x => x.learning_style_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "subdomain",
                columns: table => new
                {
                    subdomain_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    domain_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subdomain", x => x.subdomain_id);
                    table.ForeignKey(
                        name: "FK_subdomain_domains_domain_id",
                        column: x => x.domain_id,
                        principalTable: "domains",
                        principalColumn: "domain_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "email_verification_tokens",
                columns: table => new
                {
                    token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_verification_tokens", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_email_verification_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    follower_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    following_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    followed_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follows", x => x.id);
                    table.ForeignKey(
                        name: "FK_follows_users_follower_id",
                        column: x => x.follower_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_follows_users_following_id",
                        column: x => x.following_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "mentee_profile",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    domain_id = table.Column<int>(type: "int", nullable: false),
                    current_level = table.Column<int>(type: "int", nullable: false),
                    education_status = table.Column<int>(type: "int", nullable: false),
                    career_goal_id = table.Column<int>(type: "int", nullable: true),
                    learning_style_id = table.Column<int>(type: "int", nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    profile_picture_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_email_verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentee_profile", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_mentee_profile_career_goal_career_goal_id",
                        column: x => x.career_goal_id,
                        principalTable: "career_goal",
                        principalColumn: "career_goal_id");
                    table.ForeignKey(
                        name: "FK_mentee_profile_countries_country_code",
                        column: x => x.country_code,
                        principalTable: "countries",
                        principalColumn: "country_code");
                    table.ForeignKey(
                        name: "FK_mentee_profile_domains_domain_id",
                        column: x => x.domain_id,
                        principalTable: "domains",
                        principalColumn: "domain_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mentee_profile_learning_style_learning_style_id",
                        column: x => x.learning_style_id,
                        principalTable: "learning_style",
                        principalColumn: "learning_style_id");
                    table.ForeignKey(
                        name: "FK_mentee_profile_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mentor_profile",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    domain_id = table.Column<int>(type: "int", nullable: false),
                    years_of_experience = table.Column<int>(type: "int", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    linkedin_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profile_picture_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    past_experience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_verified = table.Column<bool>(type: "bit", nullable: false),
                    average_rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    total_reviews = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    country_code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    is_email_verified = table.Column<bool>(type: "bit", nullable: false),
                    CvUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentor_profile", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_mentor_profile_countries_country_code",
                        column: x => x.country_code,
                        principalTable: "countries",
                        principalColumn: "country_code");
                    table.ForeignKey(
                        name: "FK_mentor_profile_domains_domain_id",
                        column: x => x.domain_id,
                        principalTable: "domains",
                        principalColumn: "domain_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mentor_profile_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_reset_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_password_reset_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_links",
                columns: table => new
                {
                    link_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_links", x => x.link_id);
                    table.ForeignKey(
                        name: "FK_profile_links_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_revoked = table.Column<bool>(type: "bit", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_RegistrationSessions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reported_items",
                columns: table => new
                {
                    reported_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    target_type = table.Column<int>(type: "int", nullable: false),
                    target_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    report_score = table.Column<int>(type: "int", nullable: false),
                    report_threshold = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    content_action = table.Column<int>(type: "int", nullable: false),
                    user_action = table.Column<int>(type: "int", nullable: false),
                    ban_expires_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    admin_notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    resolved_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    resolved_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reported_items", x => x.reported_item_id);
                    table.ForeignKey(
                        name: "FK_reported_items_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    achievement_id = table.Column<int>(type: "int", nullable: false),
                    date_achieved = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_achievements", x => new { x.user_id, x.achievement_id });
                    table.ForeignKey(
                        name: "FK_user_achievements_achievements_achievement_id",
                        column: x => x.achievement_id,
                        principalTable: "achievements",
                        principalColumn: "achievement_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_achievements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_bans",
                columns: table => new
                {
                    ban_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_permanent = table.Column<bool>(type: "bit", nullable: false),
                    is_revoked = table.Column<bool>(type: "bit", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    reported_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    issued_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    revoked_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_bans", x => x.ban_id);
                    table.ForeignKey(
                        name: "FK_user_bans_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_education",
                columns: table => new
                {
                    education_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    institution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    faculty = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    degree = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    start_year = table.Column<int>(type: "int", nullable: true),
                    graduation_year = table.Column<int>(type: "int", nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_education", x => x.education_id);
                    table.ForeignKey(
                        name: "FK_user_education_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_warnings",
                columns: table => new
                {
                    warning_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    reported_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    issued_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_warnings", x => x.warning_id);
                    table.ForeignKey(
                        name: "FK_user_warnings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "apps_cancellation",
                columns: table => new
                {
                    CancelledId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    MenteeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apps_cancellation", x => x.CancelledId);
                    table.ForeignKey(
                        name: "FK_apps_cancellation_mentee_profile_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenteeSubDomains",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDomainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteeSubDomains", x => new { x.UserId, x.SubDomainId });
                    table.ForeignKey(
                        name: "FK_MenteeSubDomains_mentee_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenteeSubDomains_subdomain_SubDomainId",
                        column: x => x.SubDomainId,
                        principalTable: "subdomain",
                        principalColumn: "subdomain_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mentorships_cancellation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorshipId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    MenteeProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CancellationActor = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    CancellationReasonValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentorships_cancellation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mentorships_cancellation_mentee_profile_MenteeProfileId",
                        column: x => x.MenteeProfileId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mentorships_cancellation_mentor_profile_MentorProfileId",
                        column: x => x.MentorProfileId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MentorSubDomains",
                columns: table => new
                {
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDomainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorSubDomains", x => new { x.MentorId, x.SubDomainId });
                    table.ForeignKey(
                        name: "FK_MentorSubDomains_mentor_profile_MentorId",
                        column: x => x.MentorId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorSubDomains_subdomain_SubDomainId",
                        column: x => x.SubDomainId,
                        principalTable: "subdomain",
                        principalColumn: "subdomain_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roadmaps",
                columns: table => new
                {
                    RoadmapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    MentorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roadmaps", x => x.RoadmapId);
                    table.ForeignKey(
                        name: "FK_roadmaps_mentor_profile_MentorProfileId",
                        column: x => x.MentorProfileId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    report_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reported_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reporter_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_reports_reported_items_reported_item_id",
                        column: x => x.reported_item_id,
                        principalTable: "reported_items",
                        principalColumn: "reported_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reports_users_reporter_id",
                        column: x => x.reporter_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "programs",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Availability = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ProgramPostStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false),
                    SubDomainId = table.Column<int>(type: "int", nullable: false),
                    RoadmapId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programs", x => x.ProgramId);
                    table.ForeignKey(
                        name: "FK_programs_domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "domains",
                        principalColumn: "domain_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_programs_mentor_profile_MentorProfileId",
                        column: x => x.MentorProfileId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_programs_roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "roadmaps",
                        principalColumn: "RoadmapId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_programs_subdomain_SubDomainId",
                        column: x => x.SubDomainId,
                        principalTable: "subdomain",
                        principalColumn: "subdomain_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roadmap_steps",
                columns: table => new
                {
                    RoadmapStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RoadmapId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetRequirements = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    MenteeProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_applications_mentee_profile_MenteeProfileId",
                        column: x => x.MenteeProfileId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_applications_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mentorships",
                columns: table => new
                {
                    MentorshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenteeProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentorships", x => x.MentorshipId);
                    table.ForeignKey(
                        name: "FK_mentorships_mentee_profile_MenteeProfileId",
                        column: x => x.MenteeProfileId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mentorships_mentor_profile_MentorProfileId",
                        column: x => x.MentorProfileId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mentorships_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_likes",
                columns: table => new
                {
                    LikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_post_likes_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_likes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post-Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post-Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Post-Comment_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post-Comment_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "program_questions",
                columns: table => new
                {
                    ProgramQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AnswerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    MaxSelections = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_questions", x => x.ProgramQuestionId);
                    table.ForeignKey(
                        name: "FK_program_questions_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saved_posts",
                columns: table => new
                {
                    SaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_posts", x => x.SaveId);
                    table.ForeignKey(
                        name: "FK_saved_posts_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saved_posts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shared_posts",
                columns: table => new
                {
                    ShareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_posts", x => x.ShareId);
                    table.ForeignKey(
                        name: "FK_shared_posts_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shared_posts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "technologies",
                columns: table => new
                {
                    technology_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subdomain_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technologies", x => x.technology_id);
                    table.ForeignKey(
                        name: "FK_technologies_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId");
                    table.ForeignKey(
                        name: "FK_technologies_subdomain_subdomain_id",
                        column: x => x.subdomain_id,
                        principalTable: "subdomain",
                        principalColumn: "subdomain_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenteeProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_feedbacks_mentee_profile_MenteeProfileId",
                        column: x => x.MenteeProfileId,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_feedbacks_mentor_profile_MentorProfileId",
                        column: x => x.MentorProfileId,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_feedbacks_mentorships_MentorshipId",
                        column: x => x.MentorshipId,
                        principalTable: "mentorships",
                        principalColumn: "MentorshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_mentorships_MentorshipId",
                        column: x => x.MentorshipId,
                        principalTable: "mentorships",
                        principalColumn: "MentorshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_answers",
                columns: table => new
                {
                    ApplicationAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    ProgramQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_answers", x => x.ApplicationAnswerId);
                    table.ForeignKey(
                        name: "FK_application_answers_applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_application_answers_program_questions_ProgramQuestionId",
                        column: x => x.ProgramQuestionId,
                        principalTable: "program_questions",
                        principalColumn: "ProgramQuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "question_options",
                columns: table => new
                {
                    QuestionOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProgramQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_options", x => x.QuestionOptionId);
                    table.ForeignKey(
                        name: "FK_question_options_program_questions_ProgramQuestionId",
                        column: x => x.ProgramQuestionId,
                        principalTable: "program_questions",
                        principalColumn: "ProgramQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mentee_interests",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    technology_id = table.Column<int>(type: "int", nullable: false),
                    experience_level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentee_interests", x => new { x.user_id, x.technology_id });
                    table.ForeignKey(
                        name: "FK_mentee_interests_mentee_profile_user_id",
                        column: x => x.user_id,
                        principalTable: "mentee_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mentee_interests_technologies_technology_id",
                        column: x => x.technology_id,
                        principalTable: "technologies",
                        principalColumn: "technology_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mentor_expertise",
                columns: table => new
                {
                    mentor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    technology_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentor_expertise", x => new { x.mentor_id, x.technology_id });
                    table.ForeignKey(
                        name: "FK_mentor_expertise_mentor_profile_mentor_id",
                        column: x => x.mentor_id,
                        principalTable: "mentor_profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mentor_expertise_technologies_technology_id",
                        column: x => x.technology_id,
                        principalTable: "technologies",
                        principalColumn: "technology_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mentorship_requirements",
                columns: table => new
                {
                    MentorshipRequirementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequiredExperienceLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    TechnologyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentorship_requirements", x => x.MentorshipRequirementId);
                    table.ForeignKey(
                        name: "FK_mentorship_requirements_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "programs",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mentorship_requirements_technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "technologies",
                        principalColumn: "technology_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "career_goal",
                columns: new[] { "career_goal_id", "name" },
                values: new object[,]
                {
                    { 1, "Grow and Advance in My Current Field" },
                    { 2, "Explore a New Career Path" },
                    { 3, "Start My Own Business or Project" },
                    { 4, "Get Guidance on My Career Journey" },
                    { 5, "Prepare for Leadership or Management Roles" },
                    { 6, "Something Else" }
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "country_code", "country_name" },
                values: new object[,]
                {
                    { "AE", "United Arab Emirates" },
                    { "BH", "Bahrain" },
                    { "DJ", "Djibouti" },
                    { "DZ", "Algeria" },
                    { "EG", "Egypt" },
                    { "IQ", "Iraq" },
                    { "JO", "Jordan" },
                    { "KM", "Comoros" },
                    { "KW", "Kuwait" },
                    { "LB", "Lebanon" },
                    { "LY", "Libya" },
                    { "MA", "Morocco" },
                    { "MR", "Mauritania" },
                    { "OM", "Oman" },
                    { "PS", "Palestine" },
                    { "QA", "Qatar" },
                    { "SA", "Saudi Arabia" },
                    { "SD", "Sudan" },
                    { "SO", "Somalia" },
                    { "SY", "Syria" },
                    { "TN", "Tunisia" },
                    { "YE", "Yemen" }
                });

            migrationBuilder.InsertData(
                table: "domains",
                columns: new[] { "domain_id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Building software systems and applications", "Software Engineering" },
                    { 2, "Machine learning and data-driven systems", "AI & Data Science" },
                    { 3, "User-centered and visual design disciplines", "Design" },
                    { 4, "Product strategy, management, and business skills", "Product & Business" }
                });

            migrationBuilder.InsertData(
                table: "learning_style",
                columns: new[] { "learning_style_id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Learns best through visuals, diagrams, and examples", "Visual" },
                    { 2, "Learns best through listening, discussions, and explanations", "Auditory" },
                    { 3, "Prefers text-based materials, notes, and documentation", "Reading/Writing" },
                    { 4, "Learns best through hands-on practice and experimentation", "Kinesthetic" },
                    { 5, "Learns best by building real projects", "Project-Based" },
                    { 6, "Prefers structured guidance and regular mentor feedback", "Guided Mentorship" },
                    { 7, "Prefers independent learning at own pace", "Self-Paced" },
                    { 8, "Learns best through group discussions and collaboration", "Collaborative" }
                });

            migrationBuilder.InsertData(
                table: "subdomain",
                columns: new[] { "subdomain_id", "domain_id", "name" },
                values: new object[,]
                {
                    { 1, 1, "Backend Development" },
                    { 2, 1, "Frontend Development" },
                    { 3, 1, "Full Stack Development" },
                    { 4, 1, "Mobile Development" },
                    { 5, 1, "DevOps & Cloud" },
                    { 6, 1, "System Design" },
                    { 7, 2, "Machine Learning" },
                    { 8, 2, "Deep Learning" },
                    { 9, 2, "Computer Vision" },
                    { 10, 2, "Natural Language Processing" },
                    { 11, 2, "Data Analysis" },
                    { 12, 2, "Data Science" },
                    { 13, 2, "Data Engineering" },
                    { 14, 3, "UI/UX Design" },
                    { 15, 3, "Product Design" },
                    { 16, 3, "Graphic Design" },
                    { 17, 3, "Motion Design" },
                    { 18, 3, "Branding" },
                    { 19, 4, "Product Management" },
                    { 20, 4, "Business Analysis" },
                    { 21, 4, "Project Management" },
                    { 22, 4, "Career Coaching" },
                    { 23, 4, "Entrepreneurship" }
                });

            migrationBuilder.InsertData(
                table: "technologies",
                columns: new[] { "technology_id", "name", "ProgramId", "subdomain_id" },
                values: new object[,]
                {
                    { 1, "Node.js", null, 1 },
                    { 2, ".NET / ASP.NET Core", null, 1 },
                    { 3, "Spring Boot", null, 1 },
                    { 4, "Django", null, 1 },
                    { 5, "Flask", null, 1 },
                    { 6, "Laravel", null, 1 },
                    { 7, "Express.js", null, 1 },
                    { 8, "FastAPI", null, 1 },
                    { 9, "NestJS", null, 1 },
                    { 10, "HTML", null, 2 },
                    { 11, "CSS", null, 2 },
                    { 12, "JavaScript", null, 2 },
                    { 13, "TypeScript", null, 2 },
                    { 14, "React", null, 2 },
                    { 15, "Angular", null, 2 },
                    { 16, "Vue.js", null, 2 },
                    { 17, "Next.js", null, 2 },
                    { 18, "Nuxt.js", null, 2 },
                    { 19, "SASS / SCSS", null, 2 },
                    { 20, "Tailwind CSS", null, 2 },
                    { 21, "Bootstrap", null, 2 },
                    { 22, "MERN Stack", null, 3 },
                    { 23, "MEAN Stack", null, 3 },
                    { 24, "LAMP Stack", null, 3 },
                    { 25, "Django + React", null, 3 },
                    { 26, "Next.js Full Stack", null, 3 },
                    { 27, "REST APIs", null, 3 },
                    { 28, "GraphQL", null, 3 },
                    { 29, "Flutter", null, 4 },
                    { 30, "React Native", null, 4 },
                    { 31, "Android (Kotlin)", null, 4 },
                    { 32, "Android (Java)", null, 4 },
                    { 33, "iOS (Swift)", null, 4 },
                    { 34, "Xamarin", null, 4 },
                    { 35, "Ionic", null, 4 },
                    { 36, "Docker", null, 5 },
                    { 37, "Kubernetes", null, 5 },
                    { 38, "AWS", null, 5 },
                    { 39, "Azure", null, 5 },
                    { 40, "Google Cloud Platform", null, 5 },
                    { 41, "CI/CD Pipelines", null, 5 },
                    { 42, "Jenkins", null, 5 },
                    { 43, "GitHub Actions", null, 5 },
                    { 44, "GitLab CI", null, 5 },
                    { 45, "Terraform", null, 5 },
                    { 46, "Ansible", null, 5 },
                    { 47, "Nginx", null, 5 },
                    { 48, "High Level Design (HLD)", null, 6 },
                    { 49, "Low Level Design (LLD)", null, 6 },
                    { 50, "Microservices Architecture", null, 6 },
                    { 51, "REST Architecture", null, 6 },
                    { 52, "Event-Driven Architecture", null, 6 },
                    { 53, "Design Patterns", null, 6 },
                    { 54, "Scalability & Load Balancing", null, 6 },
                    { 55, "Caching (Redis, Memcached)", null, 6 },
                    { 56, "Message Queues (Kafka, RabbitMQ)", null, 6 },
                    { 57, "Python", null, 7 },
                    { 58, "Scikit-learn", null, 7 },
                    { 59, "TensorFlow", null, 7 },
                    { 60, "PyTorch", null, 7 },
                    { 61, "Keras", null, 7 },
                    { 62, "XGBoost", null, 7 },
                    { 63, "LightGBM", null, 7 },
                    { 64, "CatBoost", null, 7 },
                    { 65, "TensorFlow", null, 8 },
                    { 66, "PyTorch", null, 8 },
                    { 67, "Keras", null, 8 },
                    { 68, "CNNs", null, 8 },
                    { 69, "RNNs", null, 8 },
                    { 70, "Transformers", null, 8 },
                    { 71, "OpenCV", null, 9 },
                    { 72, "TensorFlow", null, 9 },
                    { 73, "PyTorch", null, 9 },
                    { 74, "YOLO", null, 9 },
                    { 75, "MediaPipe", null, 9 },
                    { 76, "NLTK", null, 10 },
                    { 77, "SpaCy", null, 10 },
                    { 78, "Hugging Face Transformers", null, 10 },
                    { 79, "BERT", null, 10 },
                    { 80, "GPT Models", null, 10 },
                    { 81, "Python", null, 11 },
                    { 82, "Pandas", null, 11 },
                    { 83, "NumPy", null, 11 },
                    { 84, "SQL", null, 11 },
                    { 85, "Excel", null, 11 },
                    { 86, "Power BI", null, 11 },
                    { 87, "Tableau", null, 11 },
                    { 88, "Google Sheets", null, 11 },
                    { 89, "Python", null, 12 },
                    { 90, "R", null, 12 },
                    { 91, "Jupyter Notebook", null, 12 },
                    { 92, "Matplotlib", null, 12 },
                    { 93, "Seaborn", null, 12 },
                    { 94, "SQL", null, 12 },
                    { 95, "Machine Learning Libraries", null, 12 },
                    { 96, "Apache Spark", null, 13 },
                    { 97, "Apache Airflow", null, 13 },
                    { 98, "Hadoop", null, 13 },
                    { 99, "BigQuery", null, 13 },
                    { 100, "Snowflake", null, 13 },
                    { 101, "Redshift", null, 13 },
                    { 102, "Kafka", null, 13 },
                    { 103, "ETL Pipelines", null, 13 },
                    { 104, "Figma", null, 14 },
                    { 105, "Adobe XD", null, 14 },
                    { 106, "Sketch", null, 14 },
                    { 107, "InVision", null, 14 },
                    { 108, "Zeplin", null, 14 },
                    { 109, "FigJam", null, 14 },
                    { 110, "Figma", null, 15 },
                    { 111, "Adobe XD", null, 15 },
                    { 112, "Sketch", null, 15 },
                    { 113, "User Journey Mapping", null, 15 },
                    { 114, "Wireframing", null, 15 },
                    { 115, "Prototyping", null, 15 },
                    { 116, "Photoshop", null, 16 },
                    { 117, "Illustrator", null, 16 },
                    { 118, "InDesign", null, 16 },
                    { 119, "Canva", null, 16 },
                    { 120, "After Effects", null, 17 },
                    { 121, "Premiere Pro", null, 17 },
                    { 122, "Blender", null, 17 },
                    { 123, "Cinema 4D", null, 17 },
                    { 124, "Illustrator", null, 18 },
                    { 125, "Photoshop", null, 18 },
                    { 126, "Brand Identity Systems", null, 18 },
                    { 127, "Logo Design Tools", null, 18 },
                    { 128, "Jira", null, 19 },
                    { 129, "Confluence", null, 19 },
                    { 130, "Notion", null, 19 },
                    { 131, "Trello", null, 19 },
                    { 132, "Miro", null, 19 },
                    { 133, "Product Roadmaps", null, 19 },
                    { 134, "OKRs", null, 19 },
                    { 135, "Excel", null, 20 },
                    { 136, "Power BI", null, 20 },
                    { 137, "Tableau", null, 20 },
                    { 138, "SQL", null, 20 },
                    { 139, "BPMN", null, 20 },
                    { 140, "SWOT Analysis", null, 20 },
                    { 141, "Jira", null, 21 },
                    { 142, "Trello", null, 21 },
                    { 143, "Asana", null, 21 },
                    { 144, "Monday.com", null, 21 },
                    { 145, "ClickUp", null, 21 },
                    { 146, "Resume Review Tools", null, 22 },
                    { 147, "Interview Preparation Frameworks", null, 22 },
                    { 148, "LinkedIn Optimization", null, 22 },
                    { 149, "Career Planning Models", null, 22 },
                    { 150, "Lean Canvas", null, 23 },
                    { 151, "Business Model Canvas", null, 23 },
                    { 152, "Pitch Deck Tools", null, 23 },
                    { 153, "Market Research Tools", null, 23 },
                    { 154, "Financial Modeling", null, 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_application_answers_ApplicationId",
                table: "application_answers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_application_answers_ProgramQuestionId",
                table: "application_answers",
                column: "ProgramQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_MenteeProfileId",
                table: "applications",
                column: "MenteeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_ProgramId",
                table: "applications",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_apps_cancellation_MenteeId",
                table: "apps_cancellation",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_email_verification_tokens_user_id",
                table: "email_verification_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_MenteeProfileId",
                table: "feedbacks",
                column: "MenteeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_MentorProfileId",
                table: "feedbacks",
                column: "MentorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_MentorshipId",
                table: "feedbacks",
                column: "MentorshipId");

            migrationBuilder.CreateIndex(
                name: "IX_follows_follower_id_following_id",
                table: "follows",
                columns: new[] { "follower_id", "following_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_follows_following_id",
                table: "follows",
                column: "following_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentee_interests_technology_id",
                table: "mentee_interests",
                column: "technology_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentee_profile_career_goal_id",
                table: "mentee_profile",
                column: "career_goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentee_profile_country_code",
                table: "mentee_profile",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "IX_mentee_profile_domain_id",
                table: "mentee_profile",
                column: "domain_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentee_profile_learning_style_id",
                table: "mentee_profile",
                column: "learning_style_id");

            migrationBuilder.CreateIndex(
                name: "IX_MenteeSubDomains_SubDomainId",
                table: "MenteeSubDomains",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_mentor_expertise_technology_id",
                table: "mentor_expertise",
                column: "technology_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentor_profile_country_code",
                table: "mentor_profile",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "IX_mentor_profile_domain_id",
                table: "mentor_profile",
                column: "domain_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentorship_requirements_ProgramId",
                table: "mentorship_requirements",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorship_requirements_TechnologyId",
                table: "mentorship_requirements",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorships_MenteeProfileId",
                table: "mentorships",
                column: "MenteeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorships_MentorProfileId",
                table: "mentorships",
                column: "MentorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorships_ProgramId",
                table: "mentorships",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorships_cancellation_MenteeProfileId",
                table: "mentorships_cancellation",
                column: "MenteeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_mentorships_cancellation_MentorProfileId",
                table: "mentorships_cancellation",
                column: "MentorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSubDomains_SubDomainId",
                table: "MentorSubDomains",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_user_id",
                table: "password_reset_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_likes_ProgramId",
                table: "post_likes",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_post_likes_UserId",
                table: "post_likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post-Comment_ProgramId",
                table: "Post-Comment",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Post-Comment_UserId",
                table: "Post-Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_profile_links_user_id",
                table: "profile_links",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_questions_ProgramId",
                table: "program_questions",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_DomainId",
                table: "programs",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_MentorProfileId",
                table: "programs",
                column: "MentorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_RoadmapId",
                table: "programs",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_SubDomainId",
                table: "programs",
                column: "SubDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_question_options_ProgramQuestionId",
                table: "question_options",
                column: "ProgramQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSessions_ExpiresAt_IsCompleted",
                table: "RegistrationSessions",
                columns: new[] { "ExpiresAt", "IsCompleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSessions_SessionToken",
                table: "RegistrationSessions",
                column: "SessionToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationSessions_UserId",
                table: "RegistrationSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reported_items_owner_user_id",
                table: "reported_items",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reported_items_target_type_target_id",
                table: "reported_items",
                columns: new[] { "target_type", "target_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reports_reported_item_id",
                table: "reports",
                column: "reported_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_reporter_id",
                table: "reports",
                column: "reporter_id");

            migrationBuilder.CreateIndex(
                name: "IX_roadmap_steps_RoadmapId",
                table: "roadmap_steps",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_roadmaps_MentorProfileId",
                table: "roadmaps",
                column: "MentorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_saved_posts_ProgramId",
                table: "saved_posts",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_saved_posts_UserId",
                table: "saved_posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_MentorshipId",
                table: "Sessions",
                column: "MentorshipId");

            migrationBuilder.CreateIndex(
                name: "IX_shared_posts_ProgramId",
                table: "shared_posts",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_shared_posts_UserId",
                table: "shared_posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_subdomain_domain_id",
                table: "subdomain",
                column: "domain_id");

            migrationBuilder.CreateIndex(
                name: "IX_technologies_ProgramId",
                table: "technologies",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_technologies_subdomain_id",
                table: "technologies",
                column: "subdomain_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_bans_user_id",
                table: "user_bans",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_education_user_id",
                table: "user_education",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_warnings_user_id",
                table: "user_warnings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_answers");

            migrationBuilder.DropTable(
                name: "apps_cancellation");

            migrationBuilder.DropTable(
                name: "email_verification_tokens");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "follows");

            migrationBuilder.DropTable(
                name: "mentee_interests");

            migrationBuilder.DropTable(
                name: "MenteeSubDomains");

            migrationBuilder.DropTable(
                name: "mentor_expertise");

            migrationBuilder.DropTable(
                name: "mentorship_requirements");

            migrationBuilder.DropTable(
                name: "mentorships_cancellation");

            migrationBuilder.DropTable(
                name: "MentorSubDomains");

            migrationBuilder.DropTable(
                name: "password_reset_tokens");

            migrationBuilder.DropTable(
                name: "post_likes");

            migrationBuilder.DropTable(
                name: "Post-Comment");

            migrationBuilder.DropTable(
                name: "profile_links");

            migrationBuilder.DropTable(
                name: "question_options");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "RegistrationSessions");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "roadmap_steps");

            migrationBuilder.DropTable(
                name: "saved_posts");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "shared_posts");

            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "user_bans");

            migrationBuilder.DropTable(
                name: "user_education");

            migrationBuilder.DropTable(
                name: "user_warnings");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "technologies");

            migrationBuilder.DropTable(
                name: "program_questions");

            migrationBuilder.DropTable(
                name: "reported_items");

            migrationBuilder.DropTable(
                name: "mentorships");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "mentee_profile");

            migrationBuilder.DropTable(
                name: "programs");

            migrationBuilder.DropTable(
                name: "career_goal");

            migrationBuilder.DropTable(
                name: "learning_style");

            migrationBuilder.DropTable(
                name: "roadmaps");

            migrationBuilder.DropTable(
                name: "subdomain");

            migrationBuilder.DropTable(
                name: "mentor_profile");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "domains");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
