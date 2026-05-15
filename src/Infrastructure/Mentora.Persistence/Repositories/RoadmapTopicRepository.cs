using Mentora.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories
{
    public class RoadmapTopicRepository : IRoadmapTopicRepository
    {
        private readonly ApplicationDbContext _context;

        public RoadmapTopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<Topic?> GetByIdAsync(int id)
        {
            return await _context.Topics
                .Include(t => t.Materials)
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.TopicId == id);
        }


        public async Task<IEnumerable<Topic>> GetByPhaseIdAsync(int phaseId)
        {
            return await _context.Topics
                .Where(t => t.RoadmapPhaseId == phaseId)
                .Include(t => t.Materials) 
                .Include(t => t.Tasks)    
              
                .ToListAsync();
        }

        public void Add(Topic topic)
        {
            _context.Topics.Add(topic);
        }

        public void Update(Topic topic)
        {
            _context.Topics.Update(topic);
        }

        public void Delete(Topic topic)
        {
            _context.Topics.Remove(topic);
        }
    }
}
