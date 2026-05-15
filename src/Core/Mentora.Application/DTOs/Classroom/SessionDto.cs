namespace Mentora.Application.DTOs.Classroom;

public class SessionDto
{
    public int SessionId { get; set; }
    public int ClassroomId { get; set; }
    public string Title { get; set; } = null!;
    public string? MeetingLink { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
 
    // Derived helpers consumed by the frontend
    public string DateDisplay => ScheduledAt.Date == DateTime.Today.AddDays(1)
        ? "Tomorrow"
        : ScheduledAt.Date == DateTime.Today
            ? "Today"
            : ScheduledAt.ToString("MMM dd, yyyy");
 
    public string TimeDisplay => ScheduledAt.ToString("h:mm tt");
}