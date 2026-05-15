namespace Mentora.Application.DTOs.Classroom;

public class SubmissionResponseDto
{
    public int SubmissionId { get; set; }
    public int TaskId { get; set; }
    public string TaskTitle { get; set; } = null!;

    public Guid MenteeProfileId { get; set; }
    public string MenteeName { get; set; } = null!;
    public string? MenteeProfilePicture { get; set; }

    public string Title { get; set; } = null!;
    public string? NotesForMentor { get; set; }
    public string Status { get; set; } = null!;

    public List<SubmissionLinkDto> Links { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Populated when status is Reviewed
    public ReviewDto? Review { get; set; }
}

public class ReviewDto
{
    public int ReviewId { get; set; }
    public int? Grade { get; set; }
    public string? Feedback { get; set; }
    public bool IsRevisionRequest { get; set; }
    public DateTime ReviewedAt { get; set; }
}