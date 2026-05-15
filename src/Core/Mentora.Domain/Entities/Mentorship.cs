using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class Mentorship
    {
        public Guid MentorshipId { get; set; }

       
        public Guid MentorProfileId { get; set; }
        public MentorProfile MentorProfile { get; set; } = null!;

        public Guid MenteeProfileId { get; set; }
        public MenteeProfile MenteeProfile { get; set; } = null!;

        public int ProgramId { get; set; }
         public Program Program { get; set; } = null!;

       
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 

        public MentorshipStatus Status { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}

