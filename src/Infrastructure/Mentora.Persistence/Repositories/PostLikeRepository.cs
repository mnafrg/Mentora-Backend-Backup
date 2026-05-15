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
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public PostLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PostLike?> GetAsync(Expression<Func<PostLike, bool>> filter)
        {
            return await _context.PostLikes.FirstOrDefaultAsync(filter);
        }

        public async Task<PostLike> CreateAsync(PostLike like)
        {
            await _context.PostLikes.AddAsync(like);
            return like;
        }
        public async Task<int> CountAsync(Expression<Func<PostLike, bool>> filter)
        {
            return await _context.PostLikes.CountAsync(filter);
        }

        public async Task DeleteAsync(PostLike like)
        {
            _context.PostLikes.Remove(like);
        }
    }
}
