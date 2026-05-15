using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ISharedPostRepository
    {
        Task<SharedPost> CreateAsync(SharedPost share);
        Task<int> CountAsync(Expression<Func<SharedPost, bool>> filter);
    }
}
