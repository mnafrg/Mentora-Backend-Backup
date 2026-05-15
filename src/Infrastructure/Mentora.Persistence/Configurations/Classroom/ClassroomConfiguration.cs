using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> e)
    {
        e.ToTable("classrooms");

        e.HasKey(x => x.ClassroomId);

        e.Property(x => x.ClassroomId).HasColumnName("classroom_id");
        e.Property(x => x.ProgramId).HasColumnName("program_id").IsRequired();
        e.Property(x => x.Title).HasColumnName("title").IsRequired().HasMaxLength(200);
        e.Property(x => x.Description).HasColumnName("description").HasMaxLength(1000);
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        e.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

        // One classroom per program
        e.HasIndex(x => x.ProgramId).IsUnique();

        e.HasOne(x => x.Program)
         .WithOne()                       // Program does not navigate back yet
         .HasForeignKey<ClassRoom>(x => x.ProgramId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasMany(x => x.Sessions)
         .WithOne(s => s.ClassRoom)
         .HasForeignKey(s => s.ClassroomId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

