using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Application.Interfaces.Repositories.Interactions;
public interface IUserBanRepository
{
    /// Returns the current active (non-expired, non-revoked) ban, or null.
    Task<UserBan?> GetActiveBanAsync(Guid userId);

    /// All ban history for a user (for the admin history panel).
    Task<List<UserBan>> GetHistoryAsync(Guid userId);

    Task CreateAsync(UserBan ban);
    Task UpdateAsync(UserBan ban);
}