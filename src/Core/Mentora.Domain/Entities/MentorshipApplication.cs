using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class MentorshipApplication
    {
        public int ApplicationId { get; set; }
        public bool MeetRequirements { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? DecisionAt { get; set; }

        public int ProgramId { get; set; }
        public Program Program { get; set; } = null!;

        public Guid MenteeProfileId { get; set; }
        public MenteeProfile MenteeProfile { get; set; } = null!;

        public ICollection<ApplicationAnswer> Answers { get; set; } = new List<ApplicationAnswer>();
    }
}
