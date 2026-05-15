using Mentora.Domain.Entities.Profiles;
namespace Mentora.Application.Interfaces.Repositories;

public interface IAchievementRepository
{
    Task<List<UserAchievement>> GetByUserIdAsync(Guid userId);
    Task<Achievement?> GetByIdAsync(int achievementId);
    Task CreateAsync(UserAchievement ua);
    Task UpdateAsync(UserAchievement ua);
    Task DeleteAsync(Guid userId, int achievementId);
}