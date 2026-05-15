using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class RoadmapContentDto
    {
        public int RoadmapId { get; set; }
        public List<PhaseDto> Phases { get; set; }
    }
}
