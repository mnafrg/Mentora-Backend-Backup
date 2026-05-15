using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class PhaseDto
    {
        public int PhaseId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
   
        public List<TopicDto> Topics { get; set; } = new();
    }
}
