using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class Topic
    {
       
            public int TopicId { get; set; }
            public string Title { get; set; } = null!;
            public string? Summary { get; set; }
  
            public int RoadmapPhaseId { get; set; }
            public RoadmapPhase RoadmapPhase { get; set; } = null!;
       
            public ICollection<TopicTask> Tasks { get; set; } = new List<TopicTask>();
        
        public ICollection<TopicMaterial> Materials { get; set; } = new List<TopicMaterial>();
    }
}    



