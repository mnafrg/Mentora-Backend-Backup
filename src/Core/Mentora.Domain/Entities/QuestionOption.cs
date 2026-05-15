using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{ 
    //when question status is MCQ
    public class QuestionOption
    {
        public int QuestionOptionId { get; set; }
        public string OptionText { get; set; } = null!;
        public int ProgramQuestionId { get; set; }
        public ProgramQuestion ProgramQuestion { get; set; } = null!;
    }
}
