using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Repositories;

public class UserBanRepository : IUserBanRepository
{
    private readonly ApplicationDbContext _ctx;
    public UserBanRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<UserBan?> GetActiveBanAsync(Guid userId) =>
        await _ctx.UserBans
            .Where(b =>
                b.UserId     == userId &&
                !b.IsRevoked &&
                (b.IsPermanent || b.ExpiresAt > DateTime.UtcNow))
            .OrderByDescending(b => b.IssuedAt)
            .FirstOrDefaultAsync();

    public async Task<List<UserBan>> GetHistoryAsync(Guid userId) =>
        await _ctx.UserBans
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.IssuedAt)
            .ToListAsync();

    public async Task CreateAsync(UserBan ban)
    {
        await _ctx.UserBans.AddAsync(ban);
    }

    public async Task UpdateAsync(UserBan ban)
    {
        _ctx.UserBans.Update(ban);
    }
}