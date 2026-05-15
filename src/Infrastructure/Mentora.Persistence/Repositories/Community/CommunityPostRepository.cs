using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityPostRepository : ICommunityPostRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityPostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityPost?> GetPostByIdAsync(Guid postId)
    {
        return await _context.CommunityPosts
            .FirstOrDefaultAsync(p => p.CommunityPostId == postId);
    }

    public async Task<CommunityPost?> GetPostWithDetailsAsync(Guid postId)
    {
        return await _context.CommunityPosts
         
            .Include(p => p.Author)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .Include(p => p.Likes)
            .Include(p => p.Saves)
            .Include(p => p.Shares)
            .FirstOrDefaultAsync(p => p.CommunityPostId == postId);
    }

    public async Task<IEnumerable<CommunityPost>> GetAllPostsByCommunityIdAsync(Guid communityId)
    {
        return await _context.CommunityPosts
            .Where(p => p.CommunityId == communityId)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .Include(p => p.Saves)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<CommunityPost>> GetFeedPostsAsync()
    {
        return await _context.CommunityPosts
           
            .Include(p => p.Author)
            .Include(p => p.Community)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<CommunityPost> CreateAsync(CommunityPost post)
    {
        await _context.CommunityPosts.AddAsync(post);
        return post;
    }

    public Task UpdateAsync(CommunityPost post)
    {
        _context.CommunityPosts.Update(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CommunityPost post)
    {
        _context.CommunityPosts.Remove(post);
        return Task.CompletedTask;
    }
}
