using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations.Classroom;

public class MaterialCompletionConfiguration : IEntityTypeConfiguration<MaterialCompletion>
{
    public void Configure(EntityTypeBuilder<MaterialCompletion> e)
    {
        e.ToTable("material_completions");

        e.HasKey(x => x.Id);

        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.MenteeId).HasColumnName("mentee_id").IsRequired();
        e.Property(x => x.MaterialId).HasColumnName("material_id").IsRequired();
        e.Property(x => x.IsCompleted).HasColumnName("is_completed").IsRequired().HasDefaultValue(false);
        e.Property(x => x.CompletedAt).HasColumnName("completed_at");

        // One row per (mentee, material) — enforced at the DB level
        e.HasIndex(x => new { x.MenteeId, x.MaterialId }).IsUnique();

        e.HasOne(x => x.MenteeProfile)
         .WithMany()
         .HasForeignKey(x => x.MenteeId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Material)
         .WithMany()
         .HasForeignKey(x => x.MaterialId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}