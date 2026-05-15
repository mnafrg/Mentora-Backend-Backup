namespace Mentora.Application.DTOs.Classroom;

// Returned after every toggle call and in list responses
public class MaterialCompletionResponseDto
{
    public int MaterialId { get; set; }
    public string MaterialTitle { get; set; } = null!;
    public string MaterialUrl { get; set; } = null!;
    public string MaterialType { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}