using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<PostComment>> GetAllAsync(
          Expression<Func<PostComment, bool>> filter = null,
          params Expression<Func<PostComment, object>>[] includes);
        Task<int> CountAsync(Expression<Func<PostComment, bool>> filter = null);
        Task<PostComment> CreateAsync(PostComment comment);
    }
}
