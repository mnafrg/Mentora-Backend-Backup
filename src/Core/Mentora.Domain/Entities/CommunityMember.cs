using Mentora.Domain.Enums;
using System;

namespace Mentora.Domain.Entities
{
    public class CommunityMember
    {
        public Guid CommunityMemberId { get; set; }
        public Guid CommunityId { get; set; }
        public Community Community { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public CommunityRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}
