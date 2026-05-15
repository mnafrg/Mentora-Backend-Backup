
using Microsoft.EntityFrameworkCore;
using Mentora.Domain.Entities;
using Mentora.Domain.Entities.Profiles;
using Mentora.Domain.Enums;
using Mentora.Persistence.Seeding;
using Mentora.Domain.Entities.Interactions;
using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Entities.Classroom;
using System.Security.Cryptography;

namespace Mentora.Persistence;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<SkillDomain> Domains { get; set; }
    public DbSet<SubDomain> SubDomains { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<CareerGoal> CareerGoals { get; set; }
    public DbSet<LearningStyle> LearningStyles { get; set; }
    public DbSet<MenteeProfile> MenteeProfiles { get; set; }
    public DbSet<MentorProfile> MentorProfiles { get; set; }
    public DbSet<MenteeInterest> MenteeInterests { get; set; }
    public DbSet<MentorExpertise> MentorExpertises { get; set; }
    public DbSet<MenteeSubDomain> MenteeSubDomains { get; set; }
    public DbSet<MentorSubDomain> MentorSubDomains { get; set; } 
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public DbSet<RegistrationSession> RegistrationSessions { get; set; }
    public DbSet<Program> Programs { get; set; }
    public DbSet<ProgramQuestion> ProgramQuestions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<ApplicationAnswer> ApplicationAnswers { get; set; }

    public DbSet<Roadmap> Roadmaps { get; set; }
    public DbSet<RoadmapPhase> RoadmapPhases { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<TopicTask> Tasks { get; set; }
    public DbSet<TopicMaterial> Materials { get; set; }
    public DbSet<RoadmapTechnology> RoadmapTechnologies { get; set; }
    public DbSet<SavedRoadmap> SavedRoadmaps { get; set; }

    public DbSet<Mentorship> Mentorships { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<SavedPost> SavedPosts { get; set; }
    public DbSet<SharedPost> SharedPosts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<MentorshipApplication> MentorshipApplications { get; set; }
    public DbSet<MentorshipRequirement> MentorshipRequirements { get; set; }
    public DbSet<AppCancellation> AppCancellations { get; set; }
    public DbSet<MentorshipCancellation> MentorshipCancellations { get; set; }
    public DbSet<ProfileLink> ProfileLinks { get; set; }


  
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<UserEducation> UserEducations { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<ReportedItem> ReportedItems { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<UserBan> UserBans { get; set; }
    public DbSet<UserWarning> UserWarnings { get; set; }

    // Classroom
    public DbSet<ClassRoom> Classrooms { get; set; }
    public DbSet<ClassroomSession> ClassroomSessions { get; set; }

    public DbSet<ClassroomPost> ClassroomPosts { get; set; }
    public DbSet<ClassroomPostLike> ClassroomPostLikes { get; set; }
    public DbSet<ClassroomComment> ClassroomComments { get; set; }
    public DbSet<ClassroomCommentLike> ClassroomCommentLikes { get; set; }
   
    public DbSet<TaskSubmission>  TaskSubmissions  { get; set; }
    public DbSet<SubmissionLink>  SubmissionLinks  { get; set; }
    public DbSet<SubmissionReview> SubmissionReviews { get; set; }
    public DbSet<MaterialCompletion> MaterialCompletions { get; set; }

    // Community
    public DbSet<Community> Communities { get; set; }
    public DbSet<CommunityMember> CommunityMembers { get; set; }
    public DbSet<CommunityPost> CommunityPosts { get; set; }
    public DbSet<CommunityComment> CommunityComments { get; set; }
    public DbSet<CommunityPostLike> CommunityPostLikes { get; set; }
    public DbSet<CommunityPostShare> CommunityPostShares { get; set; }
    public DbSet<CommunityPostSave> CommunityPostSaves { get; set; }
    public DbSet<CommunityReport> CommunityReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Role).HasColumnName("role").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
        });
        // MenteeProfile Configuration
        modelBuilder.Entity<MenteeProfile>(entity =>
        {
            entity.ToTable("mentee_profile");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.CurrentLevel).HasColumnName("current_level");
            entity.Property(e => e.EducationStatus).HasColumnName("education_status");
            entity.Property(e => e.CareerGoalId).HasColumnName("career_goal_id");
            entity.Property(e => e.LearningStyleId).HasColumnName("learning_style_id");
            entity.Property(e => e.CountryCode).HasColumnName("country_code").HasMaxLength(2);
            entity.Property(e => e.ProfilePictureUrl).HasColumnName("profile_picture_url");
            entity.Property(e => e.Bio).HasColumnName("bio").HasMaxLength(1000);
            entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");

            entity.HasOne(e => e.User)
                .WithOne(u => u.MenteeProfile)
                .HasForeignKey<MenteeProfile>(e => e.UserId);

            entity.HasOne(e => e.Domain)
                .WithMany(d => d.MenteeProfiles)
                .HasForeignKey(e => e.DomainId);

            entity.HasOne(e => e.CareerGoal)
                .WithMany(c => c.MenteeProfiles)
                .HasForeignKey(e => e.CareerGoalId);

            entity.HasOne(e => e.LearningStyle)
                .WithMany(l => l.MenteeProfiles)
                .HasForeignKey(e => e.LearningStyleId);

            entity.HasOne(e => e.Country)
                .WithMany(c => c.MenteeProfiles)
                .HasForeignKey(e => e.CountryCode);
        });

        // MentorProfile Configuration
        modelBuilder.Entity<MentorProfile>(entity =>
        {
            entity.ToTable("mentor_profile");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.YearsOfExperience).HasColumnName("years_of_experience");
            entity.Property(e => e.Bio).HasColumnName("bio").HasMaxLength(2000);
            entity.Property(e => e.LinkedInUrl).HasColumnName("linkedin_url");
            entity.Property(e => e.ProfilePictureUrl).HasColumnName("profile_picture_url");
            entity.Property(e => e.PastExperience).HasColumnName("past_experience");
            entity.Property(e => e.IsVerified).HasColumnName("is_verified");
            entity.Property(e => e.AverageRating).HasColumnName("average_rating").HasColumnType("decimal(3,2)");
            entity.Property(e => e.TotalReviews).HasColumnName("total_reviews");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CountryCode).HasColumnName("country_code").HasMaxLength(2);
            entity.Property(e => e.IsEmailVerified).HasColumnName("is_email_verified");

            entity.HasOne(e => e.User)
                .WithOne(u => u.MentorProfile)
                .HasForeignKey<MentorProfile>(e => e.UserId);

            entity.HasOne(e => e.Domain)
                .WithMany(d => d.MentorProfiles)
                .HasForeignKey(e => e.DomainId);

            entity.HasOne(e => e.Country)
                .WithMany(c => c.MentorProfiles)
                .HasForeignKey(e => e.CountryCode);
        });

        // Country Configuration
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("countries");
            entity.HasKey(e => e.CountryCode);
            entity.Property(e => e.CountryCode).HasColumnName("country_code").HasMaxLength(2);
            entity.Property(e => e.CountryName).HasColumnName("country_name").IsRequired().HasMaxLength(100);
        });

        // CareerGoal Configuration
        modelBuilder.Entity<CareerGoal>(entity =>
        {
            entity.ToTable("career_goal");
            entity.HasKey(e => e.CareerGoalId);
            entity.Property(e => e.CareerGoalId).HasColumnName("career_goal_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        });

        // LearningStyle Configuration
        modelBuilder.Entity<LearningStyle>(entity =>
        {
            entity.ToTable("learning_style");
            entity.HasKey(e => e.LearningStyleId);
            entity.Property(e => e.LearningStyleId).HasColumnName("learning_style_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description");
        });

        // Domain Configuration
        modelBuilder.Entity<SkillDomain>(entity =>
        {
            entity.ToTable("domains");
            entity.HasKey(e => e.DomainId);
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description");
        });

        // SubDomain Configuration
        modelBuilder.Entity<SubDomain>(entity =>
        {
            entity.ToTable("subdomain");
            entity.HasKey(e => e.SubDomainId);
            entity.Property(e => e.SubDomainId).HasColumnName("subdomain_id");
            entity.Property(e => e.DomainId).HasColumnName("domain_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Domain)
                .WithMany(d => d.SubDomains)
                .HasForeignKey(e => e.DomainId);
        });

        // Technology Configuration
        modelBuilder.Entity<Technology>(entity =>
        {
            entity.ToTable("technologies");
            entity.HasKey(e => e.TechnologyId);
            entity.Property(e => e.TechnologyId).HasColumnName("technology_id");
            entity.Property(e => e.SubDomainId).HasColumnName("subdomain_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.SubDomain)
                .WithMany(s => s.Technologies)
                .HasForeignKey(e => e.SubDomainId);
        });

        // MenteeInterest Configuration
        // MenteeInterest
        modelBuilder.Entity<MenteeInterest>(entity =>
        {
            entity.ToTable("mentee_interests");
            entity.HasKey(e => new { e.UserId, e.TechnologyId });

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TechnologyId).HasColumnName("technology_id");
            entity.Property(e => e.ExperienceLevel).HasColumnName("experience_level");

            // Cascade delete when removing MenteeProfile
            entity.HasOne(e => e.MenteeProfile)
                  .WithMany(m => m.MenteeInterests)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // NO cascade when deleting Technology
            entity.HasOne(e => e.Technology)
                  .WithMany(t => t.MenteeInterests)
                  .HasForeignKey(e => e.TechnologyId)
                  .OnDelete(DeleteBehavior.Restrict);  
        });

        // MentorExpertise – same pattern
        modelBuilder.Entity<MentorExpertise>(entity =>
        {
            entity.ToTable("mentor_expertise");
            entity.HasKey(e => new { e.MentorId, e.TechnologyId });

            entity.Property(e => e.MentorId).HasColumnName("mentor_id");
            entity.Property(e => e.TechnologyId).HasColumnName("technology_id");

            entity.HasOne(e => e.MentorProfile)
                  .WithMany(m => m.MentorExpertises)
                  .HasForeignKey(e => e.MentorId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Technology)
                  .WithMany(t => t.MentorExpertises)
                  .HasForeignKey(e => e.TechnologyId)
                  .OnDelete(DeleteBehavior.Restrict); 
        });

        // EmailVerificationToken Configuration
        modelBuilder.Entity<EmailVerificationToken>(entity =>
        {
            entity.ToTable("email_verification_tokens");
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");

            entity.HasOne(e => e.User)
                .WithMany(u => u.EmailVerificationTokens)
                .HasForeignKey(e => e.UserId);
        });

        // RefreshToken Configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            entity.HasKey(e => e.TokenId);
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash").IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // PasswordResetToken Configuration
        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("password_reset_tokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });


        //// Seed Admin User// Fixed admin user (non-deterministic values removed)
        //var fixedAdminGuid = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"); // ← CHANGE THIS to your own fixed GUID
        //var fixedDate = new DateTime(2025, 6, 1, 10, 0, 0, DateTimeKind.Utc);

        //modelBuilder.Entity<User>().HasData(
        //    new User
        //    {
        //        UserId = fixedAdminGuid,
        //        Email = "admin@mentora.com",
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
        //        FirstName = "System",
        //        LastName = "Administrator",
        //        Role = UserRole.Admin,
        //        CreatedAt = fixedDate,
        //        UpdatedAt = fixedDate,
        //        IsActive = true
        //    }
        //);


        // MenteeSubDomain Configuration
        modelBuilder.Entity<MenteeSubDomain>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SubDomainId });

            entity.HasOne(e => e.MenteeProfile)
                .WithMany(m => m.MenteeSubDomains)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SubDomain)
                .WithMany()
                .HasForeignKey(e => e.SubDomainId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MentorSubDomain Configuration
        modelBuilder.Entity<MentorSubDomain>(entity =>
        {
            entity.HasKey(e => new { e.MentorId, e.SubDomainId });

            entity.HasOne(e => e.MentorProfile)
                .WithMany(m => m.MentorSubDomains)
                .HasForeignKey(e => e.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SubDomain)
                .WithMany()
                .HasForeignKey(e => e.SubDomainId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RegistrationSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);

            entity.Property(e => e.SessionToken)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(e => e.SessionToken)
                .IsUnique();

            entity.Property(e => e.CurrentStep)
                .IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookups
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.ExpiresAt, e.IsCompleted });
        });
        // --- Mentorship Programs Configuration ---
        modelBuilder.Entity<Program>(entity =>
        {
            entity.ToTable("programs");
            entity.HasKey(e => e.ProgramId);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.ProgramImageUrl)
              .HasMaxLength(500); 

           
            entity.Property(p => p.Deadline)
                  .IsRequired();

            entity.Property(e => e.Description);

            entity.Property(e => e.EducationLevel)
                .HasConversion<string>();

            entity.Property(e => e.TargetLevel)
                .HasConversion<string>();

            entity.Property(e => e.Availability)
                .HasMaxLength(100);

            entity.Property(e => e.Duration)
                .HasMaxLength(100);

            entity.Property(e => e.ProgramPostStatus)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(e => e.MentorProfile)
                .WithMany(m => m.Programs)
                .HasForeignKey(e => e.MentorProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Domain)
                .WithMany()
                .HasForeignKey(e => e.DomainId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SubDomain)
                .WithMany()
                .HasForeignKey(e => e.SubDomainId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Roadmap)
                .WithMany()
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Questions)
                .WithOne(q => q.Program)
                .HasForeignKey(q => q.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Program)
                .HasForeignKey(c => c.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Likes)
                .WithOne(l => l.Program)
                .HasForeignKey(l => l.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.SavedByUsers)
                .WithOne(s => s.Program)
                .HasForeignKey(s => s.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Shares)
                .WithOne(s => s.Program)
                .HasForeignKey(s => s.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.MentorshipRequirements)
                .WithOne(mr => mr.Program)
                .HasForeignKey(mr => mr.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        //  ProgramQuestion  Configuration
        modelBuilder.Entity<ProgramQuestion>(entity =>
        {
            entity.ToTable("program_questions");
            entity.HasKey(e => e.ProgramQuestionId);

            entity.Property(e => e.QuestionText)
                .HasMaxLength(500);

            entity.Property(e => e.AnswerType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.MaxSelections)
                .IsRequired(false);

            entity.HasOne(e => e.Program)
                .WithMany(p => p.Questions)
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Options)
                .WithOne(o => o.ProgramQuestion)
                .HasForeignKey(o => o.ProgramQuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // QuestionOption Configuration
        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.ToTable("question_options");
            entity.HasKey(e => e.QuestionOptionId);

            entity.Property(e => e.OptionText)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasOne(e => e.ProgramQuestion)
                .WithMany(q => q.Options)
                .HasForeignKey(e => e.ProgramQuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        //ApplicationAnswer Configuration
        modelBuilder.Entity<ApplicationAnswer>(entity =>
        {
            entity.ToTable("application_answers");
            entity.HasKey(e => e.ApplicationAnswerId);

            entity.Property(e => e.AnswerText)
                .IsRequired();

            entity.HasOne(e => e.MentorshipApplication)
                .WithMany(a => a.Answers)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ProgramQuestion)
                .WithMany()
                .HasForeignKey(e => e.ProgramQuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //Roadmap Configuration
        modelBuilder.Entity<Roadmap>(entity =>
        {
            entity.ToTable("roadmaps");
            entity.HasKey(e => e.RoadmapId);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Duration)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


            entity.Property(r => r.Status)
                    .HasConversion<string>();                  

            entity.HasMany(e => e.Phases)
                .WithOne(s => s.Roadmap)
                .HasForeignKey(s => s.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.SkillDomain)
              .WithMany(d => d.Roadmaps)
              .HasForeignKey(r => r.SkillDomainId)
              .OnDelete(DeleteBehavior.Restrict);

           
            entity.HasOne(r => r.SubDomain)
                  .WithMany(s => s.Roadmaps)
                  .HasForeignKey(r => r.SubDomainId)
                  .OnDelete(DeleteBehavior.NoAction); 


            entity.HasOne(e => e.MentorProfile)
                .WithMany()
                .HasForeignKey(e => e.MentorProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        //RoadmapSteps Configuration
        modelBuilder.Entity<RoadmapPhase>(entity =>
        {
            entity.ToTable("roadmap_Phases");
            entity.HasKey(e => e.RoadmapPhaseId);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Summary)
             .HasMaxLength(1000) 
             .IsRequired(false);

          

            entity.HasOne(e => e.Roadmap)
                .WithMany(r => r.Phases)
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.ToTable("topics");
            entity.HasKey(e => e.TopicId);

            entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

            entity.Property(e => e.Summary)
             .HasMaxLength(1000) 
             .IsRequired(false);


      
            entity.HasOne(t => t.RoadmapPhase)
                  .WithMany(p => p.Topics)
                  .HasForeignKey(t => t.RoadmapPhaseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TopicTask>(entity =>
                {
                    entity.ToTable("topic_tasks");
                    entity.HasKey(e => e.TaskId);

                    entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);


                    entity.Property(e => e.Description)
                     .HasMaxLength(1000)
                     .IsRequired(false);

                    entity.Property(t => t.TaskAttachmentUrl)
                         .IsRequired();

                    entity.Property(t => t.DeadLine)
                          .IsRequired(false) 
                          .HasColumnType("datetime2");
                    entity.HasOne(task => task.Topic)
                        .WithMany(topic => topic.Tasks)
                        .HasForeignKey(task => task.TopicId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
        modelBuilder.Entity<TopicMaterial>(entity =>
        {
            entity.ToTable("materials");
            entity.HasKey(e => e.MaterialId);

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.Url)
                  .IsRequired();

          
            entity.HasOne(m => m.Topic)
                  .WithMany(t => t.Materials) 
                  .HasForeignKey(m => m.TopicId)
                  .OnDelete(DeleteBehavior.Cascade); 
        });
        modelBuilder.Entity<RoadmapTechnology>(entity =>
        {
            entity.ToTable("roadmap_technologies");

           
            entity.HasKey(rt => new { rt.RoadmapId, rt.TechnologyId });

         
            entity.HasOne(rt => rt.Roadmap)
                  .WithMany(r => r.RoadmapTechnologies)
                  .HasForeignKey(rt => rt.RoadmapId);

         
            entity.HasOne(rt => rt.Technology)
                  .WithMany(t => t.RoadmapTechnologies)
                  .HasForeignKey(rt => rt.TechnologyId);
        });
        modelBuilder.Entity<SavedRoadmap>(entity =>
        {
            entity.ToTable("saved_roadmaps");
            entity.HasKey(e => e.SavedRoadmapId);

            entity.Property(e => e.SavedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Roadmap)
                .WithMany()
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //modelBuilder.Entity<Session>(entity =>
        //{
        //    entity.ToTable("sessions");
        //    entity.HasKey(e => e.SessionId);

        //    entity.HasOne(e => e.Mentorship)
        //        .WithMany(m => m.Sessions)
        //        .HasForeignKey(e => e.MentorshipId)
        //        .OnDelete(DeleteBehavior.Cascade); 

        //    entity.Property(e => e.ScheduledAt).IsRequired();
        //    entity.Property(e => e.Duration).IsRequired();
        //    entity.Property(e => e.Type).HasMaxLength(20); 
        //    entity.Property(e => e.Status).HasMaxLength(20); 
        //});
        //Mentorship  Configuration
        modelBuilder.Entity<Mentorship>(entity =>
        {
            entity.ToTable("mentorships");
            entity.HasKey(e => e.MentorshipId);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.EndDate)
                .IsRequired(false);

            entity.HasOne(e => e.MentorProfile)
                .WithMany(m => m.Mentorships)
                .HasForeignKey(e => e.MentorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MenteeProfile)
                .WithMany(m => m.Mentorships)
                .HasForeignKey(e => e.MenteeProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Program)
                .WithMany(p => p.Mentorships)
                .HasForeignKey(e => e.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Feedbacks)
                .WithOne(f => f.Mentorship)
                .HasForeignKey(f => f.MentorshipId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // Feedback Configuration
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("feedbacks");
            entity.HasKey(e => e.FeedbackId);

            entity.Property(e => e.Rating)
                .IsRequired();

            entity.Property(e => e.Comment)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Mentorship)
                .WithMany(m => m.Feedbacks)
                .HasForeignKey(e => e.MentorshipId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MentorProfile)
                .WithMany(m => m.FeedbacksReceived)
                .HasForeignKey(e => e.MentorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MenteeProfile)
                .WithMany(m => m.FeedbacksGiven)
                .HasForeignKey(e => e.MenteeProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        //PostLike Configuration
        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.ToTable("post_likes");
            entity.HasKey(e => e.LikeId);

            entity.Property(e => e.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Program)
                  .WithMany(p => p.Likes)
                  .HasForeignKey(e => e.ProgramId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                  .WithMany(m => m.LikedPosts)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        //SavedPost Configuration
        modelBuilder.Entity<SavedPost>(entity =>
        {
            entity.ToTable("saved_posts");
            entity.HasKey(e => e.SaveId);

            entity.Property(e => e.CreatedAt)
              .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Program)
                  .WithMany(p => p.SavedByUsers)
                  .HasForeignKey(e => e.ProgramId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                  .WithMany(m => m.SavedPosts)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        //SharedPost Configuration
        modelBuilder.Entity<SharedPost>(entity =>
        {
            entity.ToTable("shared_posts");
            entity.HasKey(e => e.ShareId);

            entity.Property(e => e.SharedAt)
              .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Program)
                  .WithMany(p => p.Shares)
                  .HasForeignKey(e => e.ProgramId)
                  .OnDelete(DeleteBehavior.Cascade); 

            entity.HasOne(e => e.User)
                  .WithMany(m => m.SharedPosts)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict); 
        });
        //PostComment Configuration
        modelBuilder.Entity<PostComment>(entity =>
        {

            entity.ToTable("Post-Comment");
            entity.HasKey(e => e.CommentId);

            entity.Property(e => e.CommentText)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity.HasOne(e => e.Program)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(e => e.ProgramId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                  .WithMany(m => m.MyComments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict); 
        });
        // Application Configuration
        modelBuilder.Entity<MentorshipApplication>(entity =>
        {
            entity.ToTable("applications");
            entity.HasKey(e => e.ApplicationId);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.MeetRequirements)
                .IsRequired();

            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.DecisionAt)
                .IsRequired(false);

            entity.HasOne(e => e.Program)
                   .WithMany(p => p.Applications)  
                   .HasForeignKey(e => e.ProgramId)
                   .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MenteeProfile)
                .WithMany()
                .HasForeignKey(e => e.MenteeProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Answers)
                .WithOne(a => a.MentorshipApplication)
                .HasForeignKey(a => a.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppCancellation>(entity =>
        {
            entity.ToTable("apps_cancellation");
            entity.HasKey(e => e.CancelledId);

            entity.Property(e => e.CancellationDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.CancellationReason)
                .IsRequired()
                .HasConversion<string>(); 

            entity.HasOne(e => e.MenteeProfile)
                .WithMany()
                .HasForeignKey(e => e.MenteeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MentorshipCancellation>(entity =>
        {
            entity.ToTable("mentorships_cancellation");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CancellationDate)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.CancellationActor)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CancellationReasonValue)
                .IsRequired()
                .HasMaxLength(100);



            entity.HasOne(e => e.MenteeProfile)
                .WithMany()
                .HasForeignKey(e => e.MenteeProfileId)
                .OnDelete(DeleteBehavior.Restrict);

           
            entity.HasOne(e => e.MentorProfile)
                .WithMany()
                .HasForeignKey(e => e.MentorProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<MentorshipRequirement>(entity =>
        {
            entity.ToTable("mentorship_requirements"); 

            entity.HasKey(mr => mr.MentorshipRequirementId); 

            entity.Property(mr => mr.RequiredExperienceLevel)
                  .IsRequired()
                  .HasConversion<string>();

            entity.HasOne(mr => mr.Program)
                  .WithMany(p => p.MentorshipRequirements)
                  .HasForeignKey(mr => mr.ProgramId)
                  .OnDelete(DeleteBehavior.Cascade); 
         
            entity.HasOne(mr => mr.Technology)
                  .WithMany() 
                  .HasForeignKey(mr => mr.TechnologyId)
                  .OnDelete(DeleteBehavior.Restrict); 
        });

        modelBuilder.Entity<ProfileLink>(entity =>
        {
            entity.ToTable("profile_links");
            entity.HasKey(e => e.LinkId);
            entity.Property(e => e.LinkId).HasColumnName("link_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Label).HasColumnName("label").IsRequired().HasMaxLength(50);
            entity.Property(e => e.URL).HasColumnName("url").IsRequired().HasMaxLength(500);
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.ToTable("achievements");
            entity.HasKey(e => e.AchievementId);
            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(1000);
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.ToTable("user_achievements");
            entity.HasKey(e => new { e.UserId, e.AchievementId });

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.DateAchieved).HasColumnName("date_achieved");

            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Achievement)
                  .WithMany(a => a.UserAchievements)
                  .HasForeignKey(e => e.AchievementId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserEducation>(entity =>
        {
            entity.ToTable("user_education");
            entity.HasKey(e => e.EducationId);
            entity.Property(e => e.EducationId).HasColumnName("education_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Institution).HasColumnName("institution").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Faculty).HasColumnName("faculty").HasMaxLength(255);
            entity.Property(e => e.Degree).HasColumnName("degree").HasMaxLength(255);
            entity.Property(e => e.StartYear).HasColumnName("start_year");
            entity.Property(e => e.GraduationYear).HasColumnName("graduation_year");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.ToTable("follows");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FollowerId).HasColumnName("follower_id");
            entity.Property(e => e.FollowingId).HasColumnName("following_id");
            entity.Property(e => e.FollowedAt).HasColumnName("followed_at");

            entity.HasOne(e => e.Follower)
                .WithMany()
                .HasForeignKey(e => e.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Following)
                .WithMany()
                .HasForeignKey(e => e.FollowingId)
                .OnDelete(DeleteBehavior.NoAction);

            // Ensure a user cannot follow the same user more than once
            entity.HasIndex(e => new { e.FollowerId, e.FollowingId }).IsUnique();
        });

        // ── Community Module ────────────────────────────────────────────────
        modelBuilder.Entity<Community>(entity =>
        {
            entity.ToTable("communities");

            entity.HasKey(e => e.CommunityId);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.CoverImageUrl)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");



            entity.HasOne(r => r.Domain)
              .WithMany(d => d.Communities)
              .HasForeignKey(r => r.DomainId)
              .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Members)
                .WithOne(m => m.Community)
                .HasForeignKey(m => m.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Posts)
                .WithOne(p => p.Community)
                .HasForeignKey(p => p.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityMember>(entity =>
        {
            entity.ToTable("community_members");
            entity.HasKey(e => e.CommunityMemberId);

            entity.Property(e => e.Role)
                .IsRequired();

            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsBanned)
                .HasDefaultValue(false);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.CommunityId, e.UserId }).IsUnique();
            entity.HasIndex(e => e.CommunityId);
        });

        modelBuilder.Entity<CommunityPost>(entity =>
        {
            entity.ToTable("community_posts");
            entity.HasKey(e => e.CommunityPostId);

            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(5000);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500);

            entity.Property(e => e.LinkUrl)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

           

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.CommunityPost)
                .HasForeignKey(c => c.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Likes)
                .WithOne(l => l.CommunityPost)
                .HasForeignKey(l => l.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Shares)
                .WithOne(s => s.CommunityPost)
                .HasForeignKey(s => s.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Saves)
                .WithOne(s => s.CommunityPost)
                .HasForeignKey(s => s.CommunityPostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Reports)
                .WithOne(r => r.TargetPost)
                .HasForeignKey(r => r.TargetPostId)
              .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasIndex(e => e.CommunityId);
        });

        modelBuilder.Entity<CommunityComment>(entity =>
        {
            entity.ToTable("community_comments");
            entity.HasKey(e => e.CommunityCommentId);

            entity.Property(e => e.CommentText)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Reports)
                 .WithOne(r => r.TargetComment)
               
                 .HasForeignKey(r => r.TargetCommentId)
               .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.HasIndex(e => e.CommunityPostId);
        });

        modelBuilder.Entity<CommunityPostLike>(entity =>
        {
            entity.ToTable("community_post_likes");
            entity.HasKey(e => e.CommunityPostLikeId);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.CommunityPostId, e.UserId }).IsUnique();
        });

        modelBuilder.Entity<CommunityPostShare>(entity =>
        {
            entity.ToTable("community_post_shares");
            entity.HasKey(e => e.CommunityPostShareId);

            entity.Property(e => e.SharedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.SharedByUser)
                .WithMany()
                .HasForeignKey(e => e.SharedByUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostSave>(entity =>
        {
            entity.ToTable("community_post_saves");
            entity.HasKey(e => e.CommunityPostSaveId);

            entity.Property(e => e.SavedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.CommunityPostId, e.UserId }).IsUnique();
        });

        modelBuilder.Entity<CommunityReport>(entity =>
        {
            entity.ToTable("community_reports");
            entity.HasKey(e => e.CommunityReportId);

            entity.Property(e => e.ReportReason)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Reporter)
                .WithMany()
                .HasForeignKey(e => e.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TargetPost)
                .WithMany()
                .HasForeignKey(e => e.TargetPostId)
               .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            entity.HasOne(e => e.TargetComment)
                .WithMany()
                .HasForeignKey(e => e.TargetCommentId)
              .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            entity.HasOne(e => e.ReviewedByUser)
                .WithMany()
                .HasForeignKey(e => e.ReviewedByUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        modelBuilder.SeedData();
    }


}