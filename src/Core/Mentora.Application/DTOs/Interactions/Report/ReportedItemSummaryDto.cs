namespace Mentora.Application.DTOs.Interactions.Report;
/// Summary card shown in the admin queue list.
/// Contains score, target info, and a preview of the latest reason.

public class ReportedItemSummaryDto
{
    public Guid  ReportedItemId { get; set; }
    public string  TargetType { get; set; } = null!;
    public Guid TargetId { get; set; }
    public string  OwnerName{ get; set; } = null!;
    public string? OwnerPictureUrl { get; set; }
    public int ReportScore { get; set; }
    public int ReportThreshold { get; set; }
    public string  Status { get; set; } = null!;
    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }
 
    /// Most common reason across all submissions for quick glance.
    public string  TopReason{ get; set; } = null!;
 
    /// Total number of individual report submissions.
    public int TotalReports { get; set; }
}