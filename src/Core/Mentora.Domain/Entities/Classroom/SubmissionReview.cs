namespace Mentora.Domain.Entities.Classroom;

public class SubmissionReview
{
    public int ReviewId { get; set; }
    public int SubmissionId { get; set; }
    public Guid MentorProfileId { get; set; }

    public int? Grade { get; set; }           // null when requesting revision
    public string? Feedback { get; set; }
    public bool IsRevisionRequest { get; set; }

    public DateTime ReviewedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public TaskSubmission Submission { get; set; } = null!;
    public MentorProfile MentorProfile { get; set; } = null!;
}