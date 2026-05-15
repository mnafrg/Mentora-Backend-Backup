using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class Feedback
    {
        public Guid FeedbackId { get; set; } 

        public Guid MentorshipId { get; set; }
        public Mentorship Mentorship { get; set; } = null!;

        public Guid MenteeProfileId { get; set; }
        public MenteeProfile MenteeProfile { get; set; } = null!;

        public Guid MentorProfileId { get; set; }
        public MentorProfile MentorProfile { get; set; } = null!;

        public int Rating { get; set; } 
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; } 
    }
}
