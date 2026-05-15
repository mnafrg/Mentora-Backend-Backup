using Mentora.Domain.Enums.Interactions.Report;
namespace Mentora.Domain.Entities.Interactions.Report;

// Many Report rows roll up to one ReportedItem row.
public class Report
{
    public Guid ReportId { get; set; }
    public Guid ReportedItemId { get; set; }
    public Guid ReporterId { get; set; }
    public string Reason { get; set; } = null!;
    public string? Description { get; set; }
    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    public DateTime CreatedAt { get; set; }

    // Navigation
    public ReportedItem ReportedItem { get; set; } = null!;
    public User Reporter { get; set; } = null!;
}