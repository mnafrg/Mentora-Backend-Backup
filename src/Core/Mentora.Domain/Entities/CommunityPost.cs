using System;
using System.Collections.Generic;

namespace Mentora.Domain.Entities
{
    public class CommunityPost
    {
        public Guid CommunityPostId { get; set; }
        public Guid CommunityId { get; set; }
        public Community Community { get; set; } = null!;

        public Guid AuthorUserId { get; set; }
        public User Author { get; set; } = null!;

        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
      

        public ICollection<CommunityComment> Comments { get; set; } = new List<CommunityComment>();
        public ICollection<CommunityPostLike> Likes { get; set; } = new List<CommunityPostLike>();
        public ICollection<CommunityPostShare> Shares { get; set; } = new List<CommunityPostShare>();
        public ICollection<CommunityPostSave> Saves { get; set; } = new List<CommunityPostSave>();
        public ICollection<CommunityReport> Reports { get; set; } = new List<CommunityReport>();
    }
}
