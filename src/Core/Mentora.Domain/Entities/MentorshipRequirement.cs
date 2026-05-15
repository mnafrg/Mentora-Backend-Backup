using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class MentorshipRequirement
    {
        public int MentorshipRequirementId { get; set; }
        public ExperienceLevel RequiredExperienceLevel { get; set; }

        public int ProgramId { get; set; }
        public Program Program { get; set; } = null!;

        public int TechnologyId { get; set; }
        public Technology Technology { get; set; } = null!;
    }
}
