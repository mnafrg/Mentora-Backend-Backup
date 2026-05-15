namespace Mentora.Domain.Entities.Classroom;

public class ClassroomPost
{
    public int PostId { get; set; }
    public int ClassroomId { get; set; }
    public Guid AuthorId { get; set; }

    public string Content { get; set; } = null!;
    public bool IsPinned { get; set; } = false;
    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ClassRoom ClassRoom { get; set; } = null!;
    public User Author { get; set; } = null!;
    public ICollection<ClassroomPostLike> Likes { get; set; } = new List<ClassroomPostLike>();
    public ICollection<ClassroomComment> Comments { get; set; } = new List<ClassroomComment>();
}