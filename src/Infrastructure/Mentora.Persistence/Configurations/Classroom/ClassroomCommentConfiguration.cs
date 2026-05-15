using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mentora.Persistence.Configurations.Classroom;

public class ClassroomCommentConfiguration : IEntityTypeConfiguration<ClassroomComment>
{
    public void Configure(EntityTypeBuilder<ClassroomComment> e)
    {
        e.ToTable("classroom_comments");

        e.HasKey(x => x.CommentId);
        e.Property(x => x.CommentId).HasColumnName("comment_id");
        e.Property(x => x.PostId).HasColumnName("post_id").IsRequired();
        e.Property(x => x.AuthorId).HasColumnName("author_id").IsRequired();
        e.Property(x => x.ParentCommentId).HasColumnName("parent_comment_id");
        e.Property(x => x.Content).HasColumnName("content").IsRequired().HasMaxLength(2000);
        e.Property(x => x.IsDeleted).HasColumnName("is_deleted").IsRequired().HasDefaultValue(false);
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        e.HasOne(x => x.Post)
         .WithMany(p => p.Comments)
         .HasForeignKey(x => x.PostId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Author)
         .WithMany()
         .HasForeignKey(x => x.AuthorId)
         .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing for replies
        e.HasOne(x => x.ParentComment)
         .WithMany(c => c.Replies)
         .HasForeignKey(x => x.ParentCommentId)
         .OnDelete(DeleteBehavior.NoAction);

        e.HasMany(x => x.Likes)
         .WithOne(l => l.Comment)
         .HasForeignKey(l => l.CommentId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomCommentLikeConfiguration : IEntityTypeConfiguration<ClassroomCommentLike>
{
    public void Configure(EntityTypeBuilder<ClassroomCommentLike> e)
    {
        e.ToTable("classroom_comment_likes");
        e.HasKey(x => x.LikeId);
        e.Property(x => x.LikeId).HasColumnName("like_id");
        e.Property(x => x.CommentId).HasColumnName("comment_id").IsRequired();
        e.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        e.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        // One like per user per comment
        e.HasIndex(x => new { x.CommentId, x.UserId }).IsUnique();

        e.HasOne(x => x.Comment)
         .WithMany(c => c.Likes)
         .HasForeignKey(x => x.CommentId)
         .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.User)
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}