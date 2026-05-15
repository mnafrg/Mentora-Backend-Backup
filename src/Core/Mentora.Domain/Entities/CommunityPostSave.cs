using System;

namespace Mentora.Domain.Entities
{
    public class CommunityPostSave
    {
        public Guid CommunityPostSaveId { get; set; }
        public Guid CommunityPostId { get; set; }
        public CommunityPost CommunityPost { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime SavedAt { get; set; }
    }
}
