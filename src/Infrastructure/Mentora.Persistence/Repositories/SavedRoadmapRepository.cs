using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
    public class SavedRoadmapRepository : ISavedRoadmapRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedRoadmapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SavedRoadmap?> GetAsync(Expression<Func<SavedRoadmap, bool>> filter)
        {
            return await _context.SavedRoadmaps.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<SavedRoadmap>> GetAllAsync(
            Expression<Func<SavedRoadmap, bool>> filter,
            params Expression<Func<SavedRoadmap, object>>[] includes)
        {
            IQueryable<SavedRoadmap> query = _context.SavedRoadmaps;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<SavedRoadmap> CreateAsync(SavedRoadmap save)
        {
            await _context.SavedRoadmaps.AddAsync(save);
            return save;
        }

        public Task DeleteAsync(SavedRoadmap save)
        {
            _context.SavedRoadmaps.Remove(save);
            return Task.CompletedTask;
        }
    }
}