namespace Mentora.Domain.Entities.Classroom;

public class ClassroomComment
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public Guid AuthorId { get; set; }

    /// Null = top-level comment. Non-null = reply to another comment.
    public int? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ClassroomPost Post { get; set; } = null!;
    public User Author { get; set; } = null!;
    public ClassroomComment? ParentComment { get; set; }
    public ICollection<ClassroomComment> Replies { get; set; } = new List<ClassroomComment>();
    public ICollection<ClassroomCommentLike> Likes { get; set; } = new List<ClassroomCommentLike>();
}