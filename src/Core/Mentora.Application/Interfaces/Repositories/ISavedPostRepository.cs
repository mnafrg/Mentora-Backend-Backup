using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ISavedPostRepository
    {
        Task<SavedPost?> GetAsync(Expression<Func<SavedPost, bool>> filter);
        Task<IEnumerable<SavedPost>> GetAllAsync(
    Expression<Func<SavedPost, bool>> filter,
    params Expression<Func<SavedPost, object>>[] includes);
        Task<SavedPost> CreateAsync(SavedPost save);
        Task<int> CountAsync(Expression<Func<SavedPost, bool>> filter);

        Task DeleteAsync(SavedPost save);
    }
}
