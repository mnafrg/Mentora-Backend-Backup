using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
    public class RoadmapPhaseRepository : IRoadmapPhaseRepository
    {
        private readonly ApplicationDbContext _context;

        public RoadmapPhaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoadmapPhase?> GetByIdAsync(int id)
        {
            return await _context.RoadmapPhases
                .FirstOrDefaultAsync(p => p.RoadmapPhaseId == id);
        }
        public async Task<RoadmapPhase?> GetByIdWithDetailsAsync(int id)
        {
            
            return await _context.RoadmapPhases
                .Include(p => p.Topics)
                        .ThenInclude(t => t.Materials)
                .Include (p => p.Topics)
                        .ThenInclude(t => t.Tasks)
           
                .FirstOrDefaultAsync(p => p.RoadmapPhaseId == id);
        }

        public async Task<IEnumerable<RoadmapPhase>> GetByRoadmapIdAsync(int roadmapId)
        {
            return await _context.RoadmapPhases
                .Where(p => p.RoadmapId == roadmapId)
          
                .Include(p => p.Topics)
                    .ThenInclude(t => t.Materials) 
                .Include(p => p.Topics)
                    .ThenInclude(t => t.Tasks)     
                           
                .ToListAsync();
        }
        public void Add(RoadmapPhase phase)
        {
            _context.RoadmapPhases.Add(phase);
        }

        public void Update(RoadmapPhase phase)
        {
            _context.RoadmapPhases.Update(phase);
        }

        public void Delete(RoadmapPhase phase)
        {
            _context.RoadmapPhases.Remove(phase);
        }
    }
}
