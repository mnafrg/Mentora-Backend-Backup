using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations;

public class SubmissionReviewConfiguration : IEntityTypeConfiguration<SubmissionReview>
{
    public void Configure(EntityTypeBuilder<SubmissionReview> e)
    {
        e.ToTable("submission_reviews");

        e.HasKey(x => x.ReviewId);

        e.Property(x => x.ReviewId).HasColumnName("review_id");
        e.Property(x => x.SubmissionId).HasColumnName("submission_id").IsRequired();
        e.Property(x => x.MentorProfileId).HasColumnName("mentor_profile_id").IsRequired();
        e.Property(x => x.Feedback).HasColumnName("feedback").HasMaxLength(2000);
        e.Property(x => x.ReviewedAt).HasColumnName("reviewed_at").HasDefaultValueSql("GETUTCDATE()");

        e.HasOne(x => x.Submission)
         .WithOne(s => s.Review)
         .HasForeignKey<SubmissionReview>(x => x.SubmissionId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.MentorProfile)
         .WithMany()
         .HasForeignKey(x => x.MentorProfileId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}