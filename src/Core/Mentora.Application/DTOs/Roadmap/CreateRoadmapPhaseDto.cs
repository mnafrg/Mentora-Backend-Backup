using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class CreateRoadmapPhaseDto
    {
        public int RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
         

    }
}
