using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Community
{
    public class FeedResponseDto
    {
        public Guid CommunityPostId { get; set; }
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string? AuthorProfilePicture { get; set; }
    }
}
