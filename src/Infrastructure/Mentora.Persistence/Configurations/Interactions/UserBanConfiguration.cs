using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Configurations.Interactions;

public class UserBanConfiguration : IEntityTypeConfiguration<UserBan>
{
    public void Configure(EntityTypeBuilder<UserBan> e)
    {
        e.ToTable("user_bans");

        e.HasKey(x => x.BanId);

        e.Property(x => x.BanId).HasColumnName("ban_id");
        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.ExpiresAt).HasColumnName("expires_at");
        e.Property(x => x.IsPermanent).HasColumnName("is_permanent").IsRequired();
        e.Property(x => x.IsRevoked).HasColumnName("is_revoked").IsRequired();
        e.Property(x => x.Reason).HasColumnName("reason").HasMaxLength(500);
        e.Property(x => x.ReportedItemId).HasColumnName("reported_item_id");
        e.Property(x => x.IssuedBy).HasColumnName("issued_by").IsRequired();
        e.Property(x => x.IssuedAt).HasColumnName("issued_at").IsRequired();
        e.Property(x => x.RevokedAt).HasColumnName("revoked_at");
        e.Property(x => x.RevokedBy).HasColumnName("revoked_by");

        e.HasOne(x => x.User)
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}