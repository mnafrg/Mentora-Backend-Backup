namespace Mentora.Application.DTOs.Classroom;

public class UpdateSubmissionDto
{
    public string? Title { get; set; }
    public string? NotesForMentor { get; set; }
    public List<SubmissionLinkDto>? Links { get; set; }
    public bool? Publish { get; set; }
}