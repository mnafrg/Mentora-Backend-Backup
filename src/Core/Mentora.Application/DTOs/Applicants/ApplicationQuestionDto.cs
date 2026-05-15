using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Application
{
    public class ApplicationQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText{ get; set; } = string.Empty;
        public string QuestionAnswer { get; set; } = string.Empty;
    }
}
