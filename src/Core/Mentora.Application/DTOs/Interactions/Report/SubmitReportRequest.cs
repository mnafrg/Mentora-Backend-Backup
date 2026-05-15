namespace Mentora.Application.DTOs.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;
public class SubmitReportRequest
{
    public ReportTargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid OwnerUserId { get; set; }
    /// Valid values: Spam | Harassment | FakeProfile | InappropriateContent | Scam | Other
    public string Reason { get; set; } = null!;
    public string? Description { get; set; }
}
 