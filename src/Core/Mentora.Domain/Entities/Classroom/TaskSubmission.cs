using Mentora.Domain.Enums.Classroom;

namespace Mentora.Domain.Entities.Classroom;

public class TaskSubmission
{
    public int SubmissionId { get; set; }
    public int TaskId { get; set; }
    public Guid MenteeProfileId { get; set; }

    public string Title { get; set; } = null!;
    public string? NotesForMentor { get; set; }

    public SubmissionStatus Status { get; set; } = SubmissionStatus.Draft;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }

    // Navigation
    public TopicTask Task { get; set; } = null!;
    public MenteeProfile MenteeProfile { get; set; } = null!;
    public ICollection<SubmissionLink> Links { get; set; } = new List<SubmissionLink>();
    public SubmissionReview? Review { get; set; }
}