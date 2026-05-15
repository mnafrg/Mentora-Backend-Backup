using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
  
        public class SavedPostRepository : ISavedPostRepository
        {
            private readonly ApplicationDbContext _context;

            public SavedPostRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<SavedPost?> GetAsync(Expression<Func<SavedPost, bool>> filter)
            {
                return await _context.SavedPosts.FirstOrDefaultAsync(filter);
            }
            public async Task<IEnumerable<SavedPost>> GetAllAsync(
        Expression<Func<SavedPost, bool>> filter,
        params Expression<Func<SavedPost, object>>[] includes)
            {
                IQueryable<SavedPost> query = _context.SavedPosts;

                if (filter != null)
                    query = query.Where(filter);

                foreach (var include in includes)
                    query = query.Include(include);

                return await query.ToListAsync();
            }

        public async Task<SavedPost> CreateAsync(SavedPost save)
            {
                await _context.SavedPosts.AddAsync(save);
                return save;
            }

        public async Task<int> CountAsync(Expression<Func<SavedPost, bool>> filter)
        {
            return await _context.SavedPosts.CountAsync(filter);
        }
        public async Task DeleteAsync(SavedPost save)
            {
                _context.SavedPosts.Remove(save);
            }
        }
    }
