using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityMemberRepository : ICommunityMemberRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityMember?> GetMemberByIdAsync(Guid memberId)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(m => m.CommunityMemberId == memberId);
    }

    public async Task<CommunityMember?> GetMemberAsync(Guid communityId, Guid userId)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(m => m.CommunityId == communityId && m.UserId == userId);
    }

    public async Task<IEnumerable<CommunityMember>> GetAllMembersByCommunityIdAsync(Guid communityId)
    {
        return await _context.CommunityMembers
            .Where(m => m.CommunityId == communityId)
            .Include(m => m.User)
            .ToListAsync();
    }

    public async Task<CommunityMember> CreateAsync(CommunityMember member)
    {
        await _context.CommunityMembers.AddAsync(member);
        return member;
    }

    public Task UpdateAsync(CommunityMember member)
    {
        _context.CommunityMembers.Update(member);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CommunityMember member)
    {
        _context.CommunityMembers.Remove(member);
        return Task.CompletedTask;
    }
}
