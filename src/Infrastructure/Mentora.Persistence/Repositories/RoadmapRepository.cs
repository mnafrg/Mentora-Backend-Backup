using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories
{
    public class RoadmapRepository : IRoadmapRepository
    {
        private readonly ApplicationDbContext _context;

        public RoadmapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public async Task<Roadmap?> GetByIdAsync(int id)
        {
            return await _context.Roadmaps
                .FirstOrDefaultAsync(r => r.RoadmapId == id);
        }

        public async Task<IEnumerable<Roadmap>> GetAllBasicInfoAsync()
        {
            return await _context.Roadmaps
                .Where(r => r.Status == RoadmapStatus.Published) 
                .Include(r => r.RoadmapTechnologies)
                .Include(r => r.Phases)
                .ToListAsync();
        }
        public async Task<Roadmap?> GetByIdWithFullHierarchyAsync(int id)
        {
            return await _context.Roadmaps
                 .Include(r => r.SkillDomain)
                .Include(r => r.SubDomain)

                .Include(r => r.RoadmapTechnologies)
                  .ThenInclude(rt => rt.Technology)

                .Include(r => r.Phases)
                    .ThenInclude(p => p.Topics)
                        .ThenInclude(t => t.Materials)
                .Include(r => r.Phases)
                    .ThenInclude(p => p.Topics)
                        .ThenInclude(t => t.Tasks)
                .FirstOrDefaultAsync(r => r.RoadmapId == id);
        }
        public async Task<IEnumerable<Roadmap>> GetAllRoadmapsFullHierarchyAsync(Guid mentorId)
        {
            return await _context.Roadmaps
                .Where(r => r.MentorProfileId == mentorId && r.Status == RoadmapStatus.Published)

                .Include(r => r.SkillDomain)
                .Include(r => r.SubDomain)

                .Include(r => r.RoadmapTechnologies)
                    .ThenInclude(rt => rt.Technology)

                .Include(r => r.Phases)
                    .ThenInclude(p => p.Topics)
                        .ThenInclude(t => t.Materials)

                .Include(r => r.Phases)
                    .ThenInclude(p => p.Topics)
                        .ThenInclude(t => t.Tasks)

                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }


        public async Task<IEnumerable<Roadmap>> GetByMentorIdAsync(Guid mentorId)
        {
            return await _context.Roadmaps
                .Where(r => r.MentorProfileId == mentorId)
                .OrderByDescending(r => r.CreatedAt) 
                .ToListAsync();
        }

     
        public async Task AddAsync(Roadmap roadmap)
        {
            await _context.Roadmaps.AddAsync(roadmap);
        }

     
        public void Update(Roadmap roadmap)
        {
            _context.Roadmaps.Update(roadmap);
        }

       
        public void Delete(Roadmap roadmap)
        {
            _context.Roadmaps.Remove(roadmap);
        }
    }
}