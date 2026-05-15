using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityPostSaveRepository : ICommunityPostSaveRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityPostSaveRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityPostSave?> GetSaveAsync(Guid postId, Guid userId)
    {
        return await _context.CommunityPostSaves
            .FirstOrDefaultAsync(s => s.CommunityPostId == postId && s.UserId == userId);
    }

    public async Task<IEnumerable<CommunityPostSave>> GetAllSavedPostsByUserIdAsync(Guid userId)
    {
        return await _context.CommunityPostSaves
            .Where(s => s.UserId == userId)
            .Include(s => s.CommunityPost)
                .ThenInclude(p => p.Author)
            .OrderByDescending(s => s.SavedAt)
            .ToListAsync();
    }

    public async Task<CommunityPostSave> CreateAsync(CommunityPostSave save)
    {
        await _context.CommunityPostSaves.AddAsync(save);
        return save;
    }

    public Task DeleteAsync(CommunityPostSave save)
    {
        _context.CommunityPostSaves.Remove(save);
        return Task.CompletedTask;
    }
}
