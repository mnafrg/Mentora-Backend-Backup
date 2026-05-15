using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Classroom;

public class CreateSubmissionDto
{
    [Required]
    public string Title { get; set; } = null!;

    public List<SubmissionLinkDto> Links { get; set; } = new();
    public string? NotesForMentor { get; set; }

    /// If true, publishes immediately; otherwise saved as Draft.
    public bool Publish { get; set; } = false;
}

public class SubmissionLinkDto
{
    [Required]
    public string Url { get; set; } = null!;
    public string? Label { get; set; }
}