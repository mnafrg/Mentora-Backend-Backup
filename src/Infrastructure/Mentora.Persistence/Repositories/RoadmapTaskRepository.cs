using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mentora.Domain.Entities;
using Mentora.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Mentora.Persistence.Repositories
{
    public class RoadmapTaskRepository : IRoadmapTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public RoadmapTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TopicTask?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<IEnumerable<TopicTask>> GetByTopicIdAsync(int topicId)
        {
            return await _context.Tasks
                .Where(t => t.TopicId == topicId)
                .ToListAsync();
        }

        public void Add(TopicTask task)
        {
            _context.Tasks.Add(task);
        }

        public void Update(TopicTask task)
        {
            _context.Tasks.Update(task);
        }

        public void Delete(TopicTask task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
