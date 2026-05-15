using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IMentorshipRepository
    {
        Task<Mentorship?> GetByIdAsync(Guid id);
        Task<IEnumerable<Mentorship>> GetAllAsync(
            Expression<Func<Mentorship, bool>> filter = null,
            params Expression<Func<Mentorship, object>>[] includes);
        Task<int> CountAsync(Expression<Func<Mentorship, bool>> filter = null);
        Task<int> GetTotalUniqueMenteesByMentorIdAsync(Guid mentorId);
        Task CreateAsync(Mentorship mentorship);
        Task UpdateAsync(Mentorship mentorship);
        Task DeleteAsync(Mentorship mentorship);
    }
}
