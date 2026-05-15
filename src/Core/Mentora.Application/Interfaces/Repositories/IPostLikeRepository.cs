using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IPostLikeRepository
    {
        Task<PostLike?> GetAsync(Expression<Func<PostLike, bool>> filter);
        Task<PostLike> CreateAsync(PostLike like);
        Task<int> CountAsync(Expression<Func<PostLike, bool>> filter);

        Task DeleteAsync(PostLike like);
    }
}
