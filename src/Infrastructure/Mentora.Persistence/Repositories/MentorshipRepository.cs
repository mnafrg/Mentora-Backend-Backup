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
    public class MentorshipRepository : IMentorshipRepository
    {
        private readonly ApplicationDbContext _context;

        public MentorshipRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Mentorship?> GetByIdAsync(Guid id)
        {
            return await _context.Mentorships.FindAsync(id);
        }

        public async Task<IEnumerable<Mentorship>> GetAllAsync(
            Expression<Func<Mentorship, bool>> filter = null,
            params Expression<Func<Mentorship, object>>[] includes)
        {
            IQueryable<Mentorship> query = _context.Mentorships;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Mentorship, bool>> filter = null)
        {
            IQueryable<Mentorship> query = _context.Mentorships;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<int> GetTotalUniqueMenteesByMentorIdAsync(Guid mentorId)
        {
            return await _context.Mentorships
                .Where(m => m.Program.MentorProfileId == mentorId)
                .Select(m => m.MenteeProfileId)
                .Distinct()
                .CountAsync();
        }

        public async Task CreateAsync(Mentorship mentorship)
        {
            await _context.Mentorships.AddAsync(mentorship);
        }

        public async Task UpdateAsync(Mentorship mentorship)
        {
            _context.Mentorships.Update(mentorship);
        }

        public async Task DeleteAsync(Mentorship mentorship)
        {
            _context.Mentorships.Remove(mentorship);
        }
    }
}