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
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MentorshipApplication?> GetByIdAsync(int applicationId)
        {
            return await _context.MentorshipApplications
                .Include(a => a.Program)
                    .ThenInclude(p => p.MentorProfile)
                        .ThenInclude(m => m.User)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.ProgramQuestion)
                .Include(a => a.MenteeProfile)
                    .ThenInclude(m => m.User)
               
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        }
        public async Task<IEnumerable<MentorshipApplication>> GetAllAsync(
            Expression<Func<MentorshipApplication, bool>> filter = null,
            params Expression<Func<MentorshipApplication, object>>[] includes)
        {
            IQueryable<MentorshipApplication> query = _context.MentorshipApplications;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }
       

       
        
        public async Task<int> CountAsync(Expression<Func<MentorshipApplication, bool>> filter = null)
        {
            IQueryable<MentorshipApplication> query = _context.MentorshipApplications;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<MentorshipApplication> CreateAsync(MentorshipApplication application)
        {
            await _context.MentorshipApplications.AddAsync(application);
            return application;
        }

        public  Task UpdateAsync(MentorshipApplication application)
        {
            _context.MentorshipApplications.Update(application);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(MentorshipApplication application)
        {
            _context.MentorshipApplications.Remove(application);
            return Task.CompletedTask;
        }
    }
}
