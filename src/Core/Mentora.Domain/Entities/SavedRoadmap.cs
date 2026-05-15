using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class SavedRoadmap
    {

        public int SavedRoadmapId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int RoadmapId { get; set; }
        public Roadmap Roadmap { get; set; } = null!;
        public DateTime SavedAt { get; set; }
    }
}
