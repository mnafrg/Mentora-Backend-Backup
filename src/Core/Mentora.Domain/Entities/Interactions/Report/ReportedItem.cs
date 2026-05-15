using Mentora.Domain.Enums.Interactions.Report;
namespace Mentora.Domain.Entities.Interactions.Report;
// One row exists per unique (TargetType, TargetId) pair.
public class ReportedItem
{
    public Guid ReportedItemId { get; set; }
    public ReportTargetType TargetType { get; set; }
    // Stored as a plain Guid — no FK constraint so we can support
    //  multiple target types without a polymorphic FK hack.
    public Guid TargetId { get; set; }

    public Guid OwnerUserId { get; set; }
    public int ReportScore { get; set; }

    public int ReportThreshold { get; set; } = 3;

    public ReportedItemStatus Status { get; set; } = ReportedItemStatus.Open;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ContentAction ContentAction { get; set; } = ContentAction.None;
    public UserAction    UserAction    { get; set; } = UserAction.None;

    public DateTime? BanExpiresAt { get; set; }

    public string? AdminNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedBy { get; set; }

    // Navigate
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public User OwnerUser { get; set; } = null!;
}