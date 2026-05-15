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
    public class SharedPostRepository : ISharedPostRepository
    {
        private readonly ApplicationDbContext _context;

        public SharedPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SharedPost> CreateAsync(SharedPost share)
        {
            await _context.SharedPosts.AddAsync(share);
            return share;
        }

        public async Task<int> CountAsync(Expression<Func<SharedPost, bool>> filter)
        {
            return await _context.SharedPosts.CountAsync(filter);
        }
    }
}
