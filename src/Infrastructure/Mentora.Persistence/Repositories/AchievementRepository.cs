using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.Profiles;
namespace Mentora.Persistence.Repositories;

public class AchievementRepository : IAchievementRepository
{
    private readonly ApplicationDbContext _context;

    public AchievementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserAchievement>> GetByUserIdAsync(Guid userId) =>
        await _context.UserAchievements
            .Include(ua => ua.Achievement)
            .Where(ua => ua.UserId == userId)
            .OrderByDescending(ua => ua.DateAchieved)
            .ToListAsync();

    public async Task<Achievement?> GetByIdAsync(int achievementId) =>
        await _context.Achievements.FindAsync(achievementId);

    public async Task CreateAsync(UserAchievement ua)
    {
        await _context.UserAchievements.AddAsync(ua);
    }

    public async Task UpdateAsync(UserAchievement ua)
    {
        _context.UserAchievements.Update(ua);
    }

    public async Task DeleteAsync(Guid userId, int achievementId)
    {
        var userAchievement = await _context.UserAchievements.FindAsync(userId, achievementId);
        if (userAchievement != null) _context.UserAchievements.Remove(userAchievement);
    }
}