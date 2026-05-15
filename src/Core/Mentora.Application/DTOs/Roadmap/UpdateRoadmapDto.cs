using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class UpdateRoadmapDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public ExperienceLevel? TargetLevelFrom { get; set; }
        public ExperienceLevel? TargetLevelTo { get; set; }
        public List<int> TechnologyIds { get; set; }
        public List<UpdatePhaseDto> Phases { get; set; }
    }
}
