namespace Mentora.Domain.Entities.Classroom;

public class MaterialCompletion
{
    public int Id { get; set; }
    public Guid MenteeId { get; set; }
    public MenteeProfile MenteeProfile { get; set; } = null!;
    public int MaterialId { get; set; }
    public TopicMaterial Material { get; set; } = null!;
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
}