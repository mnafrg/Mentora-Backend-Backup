using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IApplicationRepository
    {
        Task<MentorshipApplication?> GetByIdAsync(int applicationId);
        Task<IEnumerable<MentorshipApplication>> GetAllAsync(
            Expression<Func<MentorshipApplication, bool>> filter = null,
            params Expression<Func<MentorshipApplication, object>>[] includes);
      
        Task<int> CountAsync(Expression<Func<MentorshipApplication, bool>> filter = null);
        Task<MentorshipApplication> CreateAsync(MentorshipApplication application);
        Task UpdateAsync(MentorshipApplication application);
         Task DeleteAsync(MentorshipApplication application);
    }
}
