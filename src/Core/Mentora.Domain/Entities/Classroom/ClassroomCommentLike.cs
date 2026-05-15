namespace Mentora.Domain.Entities.Classroom;

public class ClassroomCommentLike
{
    public int LikeId { get; set; }
    public int CommentId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ClassroomComment Comment { get; set; } = null!;
    public User User { get; set; } = null!;
}