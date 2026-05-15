using Mentora.Domain.Enums.Classroom;

namespace Mentora.Domain.Entities.Classroom;

public class ClassroomSession
{
    public int SessionId { get; set; }

    public int ClassroomId { get; set; }
    public string Title { get; set; } = null!;

    /// Zoom / Google Meet / Teams link provided by the mentor.
    public string? MeetingLink { get; set; }

    public DateTime ScheduledAt { get; set; }

    public ClassroomSessionStatus Status { get; set; } = ClassroomSessionStatus.Upcoming;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ClassRoom ClassRoom { get; set; } = null!;

}
