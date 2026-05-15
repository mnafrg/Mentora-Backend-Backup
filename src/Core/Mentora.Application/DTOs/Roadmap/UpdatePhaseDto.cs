using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class UpdatePhaseDto
    {
        public int? PhaseId { get; set; }  
        public string Title { get; set; }
        public string? Summary { get; set; }
      
        public List<TopicDto> Topics { get; set; }
    }
}
