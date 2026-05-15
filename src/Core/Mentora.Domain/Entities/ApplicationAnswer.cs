using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class ApplicationAnswer
    {
        public int ApplicationAnswerId { get; set; }
        public string AnswerText { get; set; } = string.Empty;

        public int ApplicationId { get; set; }
        public MentorshipApplication MentorshipApplication { get; set; } = null!;

        public int ProgramQuestionId { get; set; }
        public ProgramQuestion ProgramQuestion { get; set; } = null!;
    }
}
