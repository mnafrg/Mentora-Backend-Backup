using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Domain.Entities.Interactions.Report;

namespace Mentora.Persistence.Repositories;

public class UserWarningRepository : IUserWarningRepository
{
    private readonly ApplicationDbContext _ctx;
    public UserWarningRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<List<UserWarning>> GetByUserIdAsync(Guid userId) =>
        await _ctx.UserWarnings
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.IssuedAt)
            .ToListAsync();

    public async Task CreateAsync(UserWarning warning)
    {
        await _ctx.UserWarnings.AddAsync(warning);
    }
}