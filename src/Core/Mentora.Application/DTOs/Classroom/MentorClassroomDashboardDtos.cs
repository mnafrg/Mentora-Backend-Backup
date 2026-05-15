namespace Mentora.Application.DTOs.Classroom;

//  -- Top-level dashboard response ------------------------------
public class MentorClassroomDashboardDto
{
    /// Count of students who have at least one Submitted (not-yet-reviewed) task.
    public int StudentsWaitingForReview { get; set; }

    /// Count of students who have missed deadlines on 2+ tasks without any submission.
    public int StudentsAtRisk { get; set; }

    /// Average overall completion % across all enrolled students (0–100).
    public double AverageRoadmapCompletion { get; set; }

    /// Per-student detailed rows for the analytics table.
    public List<StudentProgressDto> Students { get; set; } = new();
}

// ── Per-student row ───────────────────────────────────────────────────────────

public class StudentProgressDto
{
    public Guid StudentId { get; set; }
    public string FullName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }

    // Last activity
    public string? LastCompletedItemTitle { get; set; }
    public DateTime? LastCompletedAt { get; set; }

    // Progress breakdown
    public int CompletedMaterials { get; set; }
    public int TotalMaterials { get; set; }
    public double MaterialsCompletionPercent { get; set; }

    public int CompletedTasks { get; set; }
    public int TotalTasks { get; set; }
    public double TasksCompletionPercent { get; set; }


    /// Overall = (completedMaterials + completedTasks) / (totalMaterials + totalTasks) * 100

    public double OverallCompletionPercent { get; set; }

    /// True when the student has 2+ missed task deadlines with no submission.
    public bool IsAtRisk { get; set; }

    /// Count of Submitted tasks awaiting mentor review.
    public int TasksWaitingForReview { get; set; }
}

// --- Remove mentee response -----------------

public class RemoveMenteeResponseDto
{
    public Guid MenteeId { get; set; }
    public string FullName { get; set; } = null!;
    public string Message { get; set; } = null!;
}