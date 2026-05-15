using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations.Classroom;

public class ClassroomPostConfiguration : IEntityTypeConfiguration<ClassroomPost>
{
    public void Configure(EntityTypeBuilder<ClassroomPost> e)
    {
        e.ToTable("classroom_posts");

        e.HasKey(x => x.PostId);
        e.Property(x => x.PostId).HasColumnName("post_id");
        e.Property(x => x.ClassroomId).HasColumnName("classroom_id").IsRequired();
        e.Property(x => x.AuthorId).HasColumnName("author_id").IsRequired();
        e.Property(x => x.Content).HasColumnName("content").IsRequired().HasMaxLength(5000);
        e.Property(x => x.IsPinned).HasColumnName("is_pinned").IsRequired();
        e.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired().HasDefaultValue(false);
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        // Efficient feed queries
        e.HasIndex(x => new { x.ClassroomId, x.IsPinned, x.CreatedAt });

        e.HasOne(x => x.ClassRoom)
         .WithMany()
         .HasForeignKey(x => x.ClassroomId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Author)
         .WithMany()
         .HasForeignKey(x => x.AuthorId)
         .OnDelete(DeleteBehavior.Restrict);

        e.HasMany(x => x.Likes)
         .WithOne(l => l.Post)
         .HasForeignKey(l => l.PostId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasMany(x => x.Comments)
         .WithOne(c => c.Post)
         .HasForeignKey(c => c.PostId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomPostLikeConfiguration : IEntityTypeConfiguration<ClassroomPostLike>
{
    public void Configure(EntityTypeBuilder<ClassroomPostLike> e)
    {
        e.ToTable("classroom_post_likes");
        e.HasKey(x => x.LikeId);
        e.Property(x => x.LikeId).HasColumnName("like_id");
        e.Property(x => x.PostId).HasColumnName("post_id").IsRequired();
        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        // One like per user per post
        e.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();

        e.HasOne(x => x.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(x => x.PostId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.User)
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}