using System;

namespace Mentora.Domain.Entities
{
    public class CommunityComment
    {
        public Guid CommunityCommentId { get; set; }
        public Guid CommunityPostId { get; set; }
        public CommunityPost CommunityPost { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string CommentText { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public ICollection<CommunityReport> Reports { get; set; } = new List<CommunityReport>();
    }
}
