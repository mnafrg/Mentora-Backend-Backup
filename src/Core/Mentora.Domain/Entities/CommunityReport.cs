using Mentora.Domain.Enums;
using System;

namespace Mentora.Domain.Entities
{
    public class CommunityReport
    {
        public Guid CommunityReportId { get; set; }
        public Guid CommunityId { get; set; }
        public Community Community { get; set; } = null!;

        public Guid ReporterUserId { get; set; }
        public User Reporter { get; set; } = null!;

        public Guid? TargetPostId { get; set; }
        public CommunityPost? TargetPost { get; set; }

        public Guid? TargetCommentId { get; set; }
        public CommunityComment? TargetComment { get; set; }

        public CommunityReportReason ReportReason { get; set; }
        public CommunityReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedByUserId { get; set; }
        public User? ReviewedByUser { get; set; }
    }
}
