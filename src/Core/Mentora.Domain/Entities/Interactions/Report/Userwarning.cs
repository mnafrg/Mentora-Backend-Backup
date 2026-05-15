namespace Mentora.Domain.Entities.Interactions.Report;

public class UserWarning
{
    public Guid WarningId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = null!;
    public Guid? ReportedItemId { get; set; }
    public Guid IssuedBy { get; set; }
    public DateTime IssuedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}