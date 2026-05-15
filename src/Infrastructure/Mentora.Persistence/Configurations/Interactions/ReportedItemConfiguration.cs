using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Configurations.Interactions;
public class ReportedItemConfiguration : IEntityTypeConfiguration<ReportedItem>
{
    public void Configure(EntityTypeBuilder<ReportedItem> e)
    {
        e.ToTable("reported_items");

        e.HasKey(x => x.ReportedItemId);

        e.Property(x => x.ReportedItemId).HasColumnName("reported_item_id");
        e.Property(x => x.TargetType).HasColumnName("target_type").IsRequired();
        e.Property(x => x.TargetId).HasColumnName("target_id").IsRequired();
        e.Property(x => x.OwnerUserId).HasColumnName("owner_user_id").IsRequired();
        e.Property(x => x.ReportScore).HasColumnName("report_score").IsRequired();
        e.Property(x => x.ReportThreshold).HasColumnName("report_threshold").IsRequired();
        e.Property(x => x.Status).HasColumnName("status").IsRequired();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        e.Property(x => x.ContentAction).HasColumnName("content_action");
        e.Property(x => x.UserAction).HasColumnName("user_action");
        e.Property(x => x.BanExpiresAt).HasColumnName("ban_expires_at");
        e.Property(x => x.AdminNotes).HasColumnName("admin_notes").HasMaxLength(2000);
        e.Property(x => x.ResolvedAt).HasColumnName("resolved_at");
        e.Property(x => x.ResolvedBy).HasColumnName("resolved_by");

        e.HasIndex(x => new { x.TargetType, x.TargetId }).IsUnique();

        e.HasOne(x => x.OwnerUser)
         .WithMany()
         .HasForeignKey(x => x.OwnerUserId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}