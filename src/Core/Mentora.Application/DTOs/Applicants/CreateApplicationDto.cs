using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Application
{
    public class CreateApplicationDto
    {
        public List<ApplicationQuestionDto>? Answers { get; set; }
        public string? AdditionalComment { get; set; }
    }

  
}
