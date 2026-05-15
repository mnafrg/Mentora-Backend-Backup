using Mentora.Domain.Enums;

namespace Mentora.Application.DTOs.Community;

public class CreateReportDto
{
    public CommunityReportReason ReportReason { get; set; }
    public string? AdditionalComment { get; set; }
    public Guid? TargetCommentId { get; set; }
}
