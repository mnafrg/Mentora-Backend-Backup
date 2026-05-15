using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityRepository : ICommunityRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Community?> GetCommunityByIdAsync(Guid communityId)
    {
        return await _context.Communities
            .Include(c => c.CreatedByUser)
                .ThenInclude(u => u.MentorProfile) 
            .Include(c => c.CreatedByUser)
                .ThenInclude(u => u.MenteeProfile) 
            .Include(c => c.Members)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.CommunityId == communityId);
    }
    public async Task<IEnumerable<Domain.Entities.Community>> GetAllCommunitiesAsync()
    {
        return await _context.Communities
          
            .Include(c => c.CreatedByUser)
                .ThenInclude(u => u.MentorProfile)
            .Include(c => c.CreatedByUser)
                .ThenInclude(u => u.MenteeProfile)
            .Include(c => c.Members) 
            .Include(c => c.Posts)   
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Community>> GetAllCommunitiesByMemberIdAsync(Guid userId)
    {
        return await _context.CommunityMembers
         
            .Where(m => m.UserId == userId)
            .Include(m => m.Community)
                .ThenInclude(c => c.CreatedByUser)
                    .ThenInclude(u => u.MentorProfile)
            .Include(m => m.Community)
                .ThenInclude(c => c.CreatedByUser)
                    .ThenInclude(u => u.MenteeProfile)
            .Include(m => m.Community)
                .ThenInclude(c => c.Members) 
            .Include(m => m.Community)
                .ThenInclude(c => c.Posts)   
            .Select(m => m.Community)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Community> CreateAsync(Domain.Entities.Community community)
    {
        await _context.Communities.AddAsync(community);
        return community;
    }

    public Task UpdateAsync(Domain.Entities.Community community)
    {
        _context.Communities.Update(community);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Domain.Entities.Community community)
    {
        _context.Communities.Remove(community);
        return Task.CompletedTask;
    }
}
