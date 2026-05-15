using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Configurations.Interactions;
public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> e)
    {
        e.ToTable("reports");

        e.HasKey(x => x.ReportId);

        e.Property(x => x.ReportId).HasColumnName("report_id");
        e.Property(x => x.ReportedItemId).HasColumnName("reported_item_id").IsRequired();
        e.Property(x => x.ReporterId).HasColumnName("reporter_id").IsRequired();
        e.Property(x => x.Reason).HasColumnName("reason").IsRequired().HasMaxLength(100);
        e.Property(x => x.Description).HasColumnName("description").HasMaxLength(2000);
        e.Property(x => x.Status).HasColumnName("status").IsRequired();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        e.HasOne(x => x.ReportedItem)
         .WithMany(i => i.Reports)
         .HasForeignKey(x => x.ReportedItemId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Reporter)
         .WithMany()
         .HasForeignKey(x => x.ReporterId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}