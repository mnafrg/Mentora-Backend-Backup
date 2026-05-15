using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityPostShareRepository : ICommunityPostShareRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityPostShareRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetSharesCountByPostIdAsync(Guid postId)
    {
        return await _context.CommunityPostShares
            .CountAsync(s => s.CommunityPostId == postId);
    }

    public async Task<CommunityPostShare> CreateAsync(CommunityPostShare share)
    {
        await _context.CommunityPostShares.AddAsync(share);
        return share;
    }
}
