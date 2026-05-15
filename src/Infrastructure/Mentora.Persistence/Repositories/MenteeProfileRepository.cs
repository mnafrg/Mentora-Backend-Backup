using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories;

public class MenteeProfileRepository : IMenteeProfileRepository
{
    private readonly ApplicationDbContext _context;

    public MenteeProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MenteeProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.MenteeProfiles
            .Include(p => p.Domain)
            .Include(p => p.CareerGoal)
            .Include(p => p.LearningStyle)
            .Include(p => p.MenteeInterests)
                .ThenInclude(i => i.Technology)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<MenteeProfile> CreateAsync(MenteeProfile profile)
    {
        await _context.MenteeProfiles.AddAsync(profile);
        return profile;
    }

    public async Task<int> CountAsync(Expression<Func<MenteeProfile, bool>> filter = null)
    {
        if (filter != null)
        {
            return await _context.MenteeProfiles.CountAsync(filter);
        }
        return await _context.MenteeProfiles.CountAsync();
    }
    public Task UpdateAsync(MenteeProfile profile)
    {
        _context.MenteeProfiles.Update(profile);
        return Task.CompletedTask;
    }
}