using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations;

public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
{
    public void Configure(EntityTypeBuilder<TaskSubmission> e)
    {
        e.ToTable("task_submissions");

        e.HasKey(x => x.SubmissionId);

        e.Property(x => x.SubmissionId).HasColumnName("submission_id");
        e.Property(x => x.Title).HasColumnName("title").IsRequired().HasMaxLength(300);
        e.Property(x => x.NotesForMentor).HasColumnName("notes_for_mentor").HasMaxLength(2000);
        e.Property(x => x.Status).HasColumnName("status").IsRequired().HasConversion<string>();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETUTCDATE()");
        e.Property(x => x.TaskId).HasColumnName("task_id").IsRequired();
        e.Property(x => x.MenteeProfileId).HasColumnName("mentee_profile_id").IsRequired();

        // One submission per mentee per task
        e.HasIndex(x => new { x.TaskId, x.MenteeProfileId }).IsUnique();

        e.HasOne(x => x.Task)
         .WithMany()
         .HasForeignKey(x => x.TaskId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.MenteeProfile)
         .WithMany()
         .HasForeignKey(x => x.MenteeProfileId)
         .OnDelete(DeleteBehavior.Restrict);

        e.HasMany(x => x.Links)
         .WithOne(l => l.Submission)
         .HasForeignKey(l => l.SubmissionId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Review)
         .WithOne(r => r.Submission)
         .HasForeignKey<SubmissionReview>(r => r.SubmissionId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}