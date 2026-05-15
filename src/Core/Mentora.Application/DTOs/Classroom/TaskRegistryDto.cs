namespace Mentora.Application.DTOs.Classroom;

public class TaskRegistryDto
{
    public int TaskId { get; set; }
    public string TaskName { get; set; } = null!;
    public string PhaseName { get; set; } = null!;

    public int TotalStudents { get; set; }
    public int TotalSubmissions { get; set; }
    public int ReviewedSubmissions { get; set; }
    public double? AverageScore { get; set; }

    public double SubmissionRate { get; set; }
    public double ReviewRate { get; set; }
    public double CompletionRate { get; set; }

    /// "Done" when all students have a Reviewed submission, otherwise "StillRunning"
    public string Status { get; set; } = null!;
}