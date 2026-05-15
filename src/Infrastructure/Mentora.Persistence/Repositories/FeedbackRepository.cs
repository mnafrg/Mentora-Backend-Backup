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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Feedback> GetQueryable()
        {
            return _context.Feedbacks.AsQueryable();
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync(Expression<Func<Feedback, bool>> filter = null)
        {
            IQueryable<Feedback> query = _context.Feedbacks;
            if (filter != null) query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task CreateAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
        }

        public void Update(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
        }

        public void Delete(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
        }
        

        public async Task<double> GetMentorAverageRatingAsync(Guid mentorId)
        {
         
            return await _context.Feedbacks
                .Where(f => f.Mentorship.Program.MentorProfileId == mentorId) 
                .AverageAsync(f => (double?)f.Rating) ?? 0.0;
        }
    }
}
