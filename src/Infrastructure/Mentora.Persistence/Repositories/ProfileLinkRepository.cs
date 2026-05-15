
using System;
using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.Profiles;

namespace Mentora.Persistence.Repositories;

public class ProfileLinkRepository : IProfileLinkRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileLinkRepository(ApplicationDbContext context)
    {
        _context = context;
    }
  public async Task<List<ProfileLink>> GetByUserIdAsync(Guid userId) =>
        await _context.ProfileLinks
            .Where(l => l.UserId == userId)
            .OrderBy(l => l.DisplayOrder)
            .ToListAsync();

    public async Task<ProfileLink?> GetByIdAsync(Guid linkId) =>
        await _context.ProfileLinks.FindAsync(linkId);

    public async Task CreateAsync(ProfileLink link)
    {
        await _context.ProfileLinks.AddAsync(link);
    }

    public async Task UpdateAsync(ProfileLink link)
    {
        _context.ProfileLinks.Update(link);
    }

    public async Task DeleteAsync(Guid linkId)
    {
        var link = await _context.ProfileLinks.FindAsync(linkId);
        if (link != null) _context.ProfileLinks.Remove(link);
    }
}



   

  
  



