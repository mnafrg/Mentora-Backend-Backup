using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class ProgramQuestionDto
    {
        
        public int ? ProgramQuestionId { get; set; }
        public string QuestionText { get; set; } =null!;

    
        public string? AnswerType { get; set; } = null!;

        public int? MaxSelections { get; set; }

        public List<string>? Options { get; set; }
    }
}
