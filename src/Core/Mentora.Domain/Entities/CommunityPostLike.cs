using System;

namespace Mentora.Domain.Entities
{
    public class CommunityPostLike
    {
        public Guid CommunityPostLikeId { get; set; }
        public Guid CommunityPostId { get; set; }
        public CommunityPost CommunityPost { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
