using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class RoadmapTechnology
    {
       
        public int RoadmapId { get; set; }
        public Roadmap Roadmap { get; set; } = null!;
        public int TechnologyId { get; set; }
        public Technology Technology { get; set; } = null!;
    }
}
