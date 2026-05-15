using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class Roadmap
    {
        
        public int RoadmapId { get; set; }
        public string Title { get; set; } = null!; 
        public string? Description { get; set; } 
         public int Duration { get; set; }

        public int SkillDomainId { get; set; }
        public SkillDomain SkillDomain { get; set; } = null!;


        public int SubDomainId { get; set; }
        public SubDomain SubDomain { get; set; } = null!;

        public ExperienceLevel? TargetLevelFrom { get; set; }
        public ExperienceLevel? TargetLevelTo { get; set; }

        public DateTime CreatedAt { get; set; }
        public RoadmapStatus? Status { get; set; }
        public Guid MentorProfileId { get; set; }
        public MentorProfile MentorProfile { get; set; } = null!;

        public ICollection<RoadmapPhase> Phases { get; set; } = new List<RoadmapPhase>();

        public ICollection<RoadmapTechnology> RoadmapTechnologies{ get; set; } = new List<RoadmapTechnology>();

    }
}

