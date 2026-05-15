namespace Mentora.Domain.Entities.Classroom;

public class ClassRoom
{
    public int ClassroomId { get; set; }

    ///One-to-one with Program. Created automatically when a program is created.
    public int ProgramId { get; set; }
    public Program Program { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<ClassroomSession> Sessions { get; set; } = new List<ClassroomSession>();
}