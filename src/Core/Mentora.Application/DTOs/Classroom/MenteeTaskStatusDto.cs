namespace Mentora.Application.DTOs.Classroom;

public class MenteeTaskStatusDto
{
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = null!;
    public string? TaskDescription { get; set; }
    public DateTime? Deadline { get; set; }

    /// Todo | Draft | Submitted | Reviewed
    public string PersonalStatus { get; set; } = null!;

    public SubmissionResponseDto? Submission { get; set; }
}