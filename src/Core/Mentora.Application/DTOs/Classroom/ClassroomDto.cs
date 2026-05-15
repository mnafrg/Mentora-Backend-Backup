namespace Mentora.Application.DTOs.Classroom;

public class ClassroomDto
{
    public int ClassroomId { get; set; }
    public int ProgramId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}