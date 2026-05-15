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
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TopicMaterial?> GetByIdAsync(int id)
        {
           
            return await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialId == id);
        }

        public async Task<IEnumerable<TopicMaterial>> GetByTopicIdAsync(int topicId)
        {
            return await _context.Materials
                .Where(m => m.TopicId == topicId)
                .ToListAsync();
        }

        public void Add(TopicMaterial material)
        {
            _context.Materials.Add(material);
        }

        public void Update(TopicMaterial material)
        {
            _context.Materials.Update(material);
        }

        public void Delete(TopicMaterial material)
        {
            _context.Materials.Remove(material);
        }
    }
}
