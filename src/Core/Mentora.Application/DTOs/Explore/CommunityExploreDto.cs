using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Explore
{
    public class CommunityExploreDto
    {
        public Guid CommunityId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? CreatorName { get; set; }
        public int DomainId { get; set; }
        public int MembersCount { get; set; }
        public int PostsCount { get; set; }
    }
}
