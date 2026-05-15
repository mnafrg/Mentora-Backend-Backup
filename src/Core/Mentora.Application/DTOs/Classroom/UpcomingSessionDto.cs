namespace Mentora.Application.DTOs.Classroom;

public class UpcomingSessionDto
{
    public int SessionId { get; set; }
    public string Title { get; set; } = null!;
    public string? MeetingLink { get; set; }
    public DateTime ScheduledAt { get; set; }
 
    public string DateDisplay => ScheduledAt.Date == DateTime.Today.AddDays(1)
        ? "Tomorrow"
        : ScheduledAt.Date == DateTime.Today
            ? "Today"
            : ScheduledAt.ToString("MMM dd, yyyy");
 
    public string TimeDisplay => ScheduledAt.ToString("h:mm tt");
 
    /// True when the session starts within the next 15 minutes or is already Live.
    public bool IsJoinable { get; set; }
}
 