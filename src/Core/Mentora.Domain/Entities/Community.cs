using System;
using System.Collections.Generic;

namespace Mentora.Domain.Entities
{
    public class Community
    {
        public Guid CommunityId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int DomainId { get; set; }   
        public SkillDomain Domain { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public ICollection<CommunityMember> Members { get; set; } = new List<CommunityMember>();
        public ICollection<CommunityPost> Posts { get; set; } = new List<CommunityPost>();
      
    }
}
