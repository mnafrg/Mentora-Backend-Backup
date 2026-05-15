using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations;

public class ClassroomSessionConfiguration : IEntityTypeConfiguration<ClassroomSession>
{
    public void Configure(EntityTypeBuilder<ClassroomSession> e)
    {
        e.ToTable("classroom_sessions");

        e.HasKey(x => x.SessionId);

        e.Property(x => x.SessionId).HasColumnName("session_id");
        e.Property(x => x.ClassroomId).HasColumnName("classroom_id").IsRequired();
        e.Property(x => x.Title).HasColumnName("title").IsRequired().HasMaxLength(200);
        e.Property(x => x.MeetingLink).HasColumnName("meeting_link").HasMaxLength(500);
        e.Property(x => x.ScheduledAt).HasColumnName("scheduled_at").IsRequired();
        e.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        // Efficient queries for "upcoming sessions" lookups
        e.HasIndex(x => new { x.ClassroomId, x.ScheduledAt });

        e.HasOne(x => x.ClassRoom)
         .WithMany(c => c.Sessions)
         .HasForeignKey(x => x.ClassroomId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}