namespace Mentora.Domain.Entities.Classroom;

public class SubmissionLink
{
    public int LinkId { get; set; }
    public int SubmissionId { get; set; }
    public string Url { get; set; } = null!;
    public string? Label { get; set; }

    public TaskSubmission Submission { get; set; } = null!;
}