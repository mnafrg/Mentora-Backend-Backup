namespace Mentora.Domain.Entities.Classroom;

public class ClassroomPostLike
{
    public int LikeId { get; set; }
    public int PostId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ClassroomPost Post { get; set; } = null!;
    public User User { get; set; } = null!;
}