using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations;

public class SubmissionLinkConfiguration : IEntityTypeConfiguration<SubmissionLink>
{
    public void Configure(EntityTypeBuilder<SubmissionLink> e)
    {
        e.ToTable("submission_links");

        e.HasKey(x => x.LinkId);

        e.Property(x => x.LinkId).HasColumnName("link_id");
        e.Property(x => x.SubmissionId).HasColumnName("submission_id").IsRequired();
        e.Property(x => x.Url).HasColumnName("url").IsRequired().HasMaxLength(1000);
        e.Property(x => x.Label).HasColumnName("label").HasMaxLength(100);

        e.HasOne(x => x.Submission)
         .WithMany(s => s.Links)
         .HasForeignKey(x => x.SubmissionId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}