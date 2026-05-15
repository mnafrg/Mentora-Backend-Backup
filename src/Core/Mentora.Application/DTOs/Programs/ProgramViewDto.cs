using Mentora.Application.DTOs.Roadmap;
using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class ProgramViewDto
    {
        public int ProgramId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ProgramImageUrl { get; set; }

        public string TargetLevel { get; set; }  = string.Empty;
        public string DomainName { get; set;  } = string.Empty;
        public DateTime Deadline { get; set; }

        public string SubDomainName { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public RoadmapDetailsDto? Roadmap { get; set; }
        public int MenteesCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string ShareUrl { get; set; }


        //  Mentor Data
      
        public Guid MentorProfileId { get; set; }
        public string MentorName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
}

}
