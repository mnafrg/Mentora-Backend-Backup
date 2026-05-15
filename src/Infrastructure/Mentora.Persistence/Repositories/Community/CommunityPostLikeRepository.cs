using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityPostLikeRepository : ICommunityPostLikeRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityPostLikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityPostLike?> GetLikeAsync(Guid postId, Guid userId)
    {
        return await _context.CommunityPostLikes
            .FirstOrDefaultAsync(l => l.CommunityPostId == postId && l.UserId == userId);
    }

    public async Task<IEnumerable<CommunityPostLike>> GetAllLikesByPostIdAsync(Guid postId)
    {
        return await _context.CommunityPostLikes
            .Where(l => l.CommunityPostId == postId)
            .ToListAsync();
    }

    public async Task<CommunityPostLike> CreateAsync(CommunityPostLike like)
    {
        await _context.CommunityPostLikes.AddAsync(like);
        return like;
    }

    public Task DeleteAsync(CommunityPostLike like)
    {
        _context.CommunityPostLikes.Remove(like);
        return Task.CompletedTask;
    }
}
