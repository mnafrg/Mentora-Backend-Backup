using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class ProgramQuestion
    {
        public int ProgramQuestionId { get; set; } 
        public string? QuestionText { get; set; } 
        public AnswerType AnswerType { get; set; } 

        public int ProgramId { get; set; }
        public Program Program { get; set; } = null!;
        public int? MaxSelections { get; set; }
        public   ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }
}
