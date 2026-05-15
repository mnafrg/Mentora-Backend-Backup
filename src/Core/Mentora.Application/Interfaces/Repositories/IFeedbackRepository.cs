using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        IQueryable<Feedback> GetQueryable();

        Task<Feedback> GetByIdAsync(int id);
        Task<IEnumerable<Feedback>> GetAllAsync(Expression<Func<Feedback, bool>> filter = null);
        Task CreateAsync(Feedback feedback);
        void Update(Feedback feedback);
        void Delete(Feedback feedback);

        Task<double> GetMentorAverageRatingAsync(Guid mentorId);
    }
}
