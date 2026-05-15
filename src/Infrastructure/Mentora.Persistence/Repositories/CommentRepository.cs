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
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostComment>> GetAllAsync(
        Expression<Func<PostComment, bool>> filter = null,
        params Expression<Func<PostComment, object>>[] includes) 
        {
            IQueryable<PostComment> query = _context.PostComments;

            if (filter != null)
                query = query.Where(filter);

           
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<PostComment, bool>> filter = null)
        {
            IQueryable<PostComment> query = _context.PostComments;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<PostComment> CreateAsync(PostComment comment)
        {
            await _context.PostComments.AddAsync(comment);
            return comment;
        }
    }
}
