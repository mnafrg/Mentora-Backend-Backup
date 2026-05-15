using Mentora.Domain.Enums;

namespace Mentora.Application.DTOs.Community;

public class ReportResponseDto
{
    public Guid CommunityReportId { get; set; }
    public CommunityReportReason ReportReason { get; set; }
    public string? AdditionalComment { get; set; }
    public CommunityReportStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ReporterUserId { get; set; }
    public string ReporterName { get; set; } = null!;
    public Guid? TargetPostId { get; set; }
    public Guid? TargetCommentId { get; set; }
    public int ReportsCount { get; set; }
}
