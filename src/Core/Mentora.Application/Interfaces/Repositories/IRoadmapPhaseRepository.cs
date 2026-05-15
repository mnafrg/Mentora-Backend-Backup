using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IRoadmapPhaseRepository
    {
        Task<RoadmapPhase?> GetByIdAsync(int id);
        Task<RoadmapPhase?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<RoadmapPhase>> GetByRoadmapIdAsync(int roadmapId);

        void Add(RoadmapPhase phase);
        void Update(RoadmapPhase phase);
        void Delete(RoadmapPhase phase);
    }
}
