using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories;


public class MentorProfileRepository : IMentorProfileRepository
{
    private readonly ApplicationDbContext _context;

    public MentorProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MentorProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.MentorProfiles
            .Include(p => p.Domain)
            .Include(p => p.MentorExpertises)
                .ThenInclude(e => e.Technology)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
    public async Task<IEnumerable<MentorProfile>> GetAllWithUserAsync()
    {
        return await _context.MentorProfiles
            .Include(p => p.User)   
            .Include(p => p.Domain)  
            .ToListAsync();
    }
    public async Task<MentorProfile> CreateAsync(MentorProfile profile)
    {
        await _context.MentorProfiles.AddAsync(profile);
        return profile;
    }

    public async Task<int> CountAsync(Expression<Func<MentorProfile, bool>> filter = null)
    {
        if (filter != null)
        {
            return await _context.MentorProfiles.CountAsync(filter);
        }
        return await _context.MentorProfiles.CountAsync();
    }
    public Task UpdateAsync(MentorProfile profile)
    {
        _context.MentorProfiles.Update(profile);
        return Task.CompletedTask;
    }
}