using System;

namespace Mentora.Domain.Entities
{
    public class CommunityPostShare
    {
        public Guid CommunityPostShareId { get; set; }
        public Guid CommunityPostId { get; set; }
        public CommunityPost CommunityPost { get; set; } = null!;

        public Guid SharedByUserId { get; set; }
        public User SharedByUser { get; set; } = null!;

        public DateTime SharedAt { get; set; }
    }
}
