using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class RoadmapPhase
    {
            public int RoadmapPhaseId { get; set; }
            public string Title { get; set; } = null!; 
            public string? Summary { get; set; }
        

            public int RoadmapId { get; set; }
            public Roadmap Roadmap { get; set; } = null!;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
    

