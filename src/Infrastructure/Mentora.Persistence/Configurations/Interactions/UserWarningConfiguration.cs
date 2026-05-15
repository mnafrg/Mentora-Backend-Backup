using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Configurations.Interactions;

public class UserWarningConfiguration : IEntityTypeConfiguration<UserWarning>
{
    public void Configure(EntityTypeBuilder<UserWarning> e)
    {
        e.ToTable("user_warnings");

        e.HasKey(x => x.WarningId);

        e.Property(x => x.WarningId).HasColumnName("warning_id");
        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.Message).HasColumnName("message").IsRequired().HasMaxLength(1000);
        e.Property(x => x.ReportedItemId).HasColumnName("reported_item_id");
        e.Property(x => x.IssuedBy).HasColumnName("issued_by").IsRequired();
        e.Property(x => x.IssuedAt).HasColumnName("issued_at").IsRequired();

        e.HasOne(x => x.User)
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}