using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Explore
{
    public class RoadmapExploreDto
    {
        public int RoadmapId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SkillDomainId { get; set; }
        public int SubDomainId { get; set; }
        public int Duration { get; set; }
        public List<int> TechnologyIds { get; set; }
        public ExperienceLevel? TargetLevelFrom { get; set; }
        public ExperienceLevel? TargetLevelTo { get; set; }
        public int PhasesCount { get; set; }
    }
}
