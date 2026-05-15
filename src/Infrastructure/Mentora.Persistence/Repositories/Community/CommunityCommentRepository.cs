using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityCommentRepository : ICommunityCommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityComment?> GetCommentByIdAsync(Guid commentId)
    {
        return await _context.CommunityComments
            .FirstOrDefaultAsync(c => c.CommunityCommentId == commentId);
    }

    public async Task<IEnumerable<CommunityComment>> GetAllCommentsByPostIdAsync(Guid postId)
    {
        return await _context.CommunityComments
            .Where(c => c.CommunityPostId == postId )
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<CommunityComment> CreateAsync(CommunityComment comment)
    {
        await _context.CommunityComments.AddAsync(comment);
        return comment;
    }

    public Task UpdateAsync(CommunityComment comment)
    {
        _context.CommunityComments.Update(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CommunityComment comment)
    {
        _context.CommunityComments.Remove(comment);
        return Task.CompletedTask;
    }
}
