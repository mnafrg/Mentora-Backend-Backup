using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class CreateRoadmapBasicInfoDto
    {
        public string Title { get; set; } = string.Empty;
        public int SkillDomainId { get; set; }
        public int SubDomainId { get; set; }
         public int Duration { get; set; }
        public string Description { get; set; } = string.Empty;
        public RoadmapStatus? Status { get; set; }
        public ExperienceLevel TargetLevelFrom { get; set; }
        public ExperienceLevel TargetLevelTo { get; set; }

        public List<int> TechnologyIds { get; set; } = new();
    }

}

