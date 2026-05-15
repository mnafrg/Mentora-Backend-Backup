using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.Interactions;

namespace Mentora.Persistence.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDbContext _ctx;
    public FollowRepository(ApplicationDbContext ctx) => _ctx = ctx;
 
    public async Task<bool> ExistsAsync(Guid followerId, Guid followingId) =>
        await _ctx.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
 
    public async Task CreateAsync(Follow follow)
    {
        await _ctx.Follows.AddAsync(follow);
    }
 
    public async Task DeleteAsync(Guid followerId, Guid followingId)
    {
        var follow = await _ctx.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
        if (follow != null) _ctx.Follows.Remove(follow);
    }
 
    public async Task<List<Follow>> GetFollowingByUserAsync(Guid followerId) =>
        await _ctx.Follows
            .Include(f => f.Following)
                .ThenInclude(u => u.MentorProfile)
                    .ThenInclude(mp => mp!.Domain)
            .Where(f => f.FollowerId == followerId)
            .OrderByDescending(f => f.FollowedAt)
            .ToListAsync();
 
    public async Task<int> GetFollowerCountAsync(Guid mentorId) =>
        await _ctx.Follows.CountAsync(f => f.FollowingId == mentorId);
 
    public async Task<List<Follow>> GetFollowersByMentorAsync(Guid mentorId) =>
        await _ctx.Follows
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == mentorId)
            .OrderByDescending(f => f.FollowedAt)
            .ToListAsync();
}