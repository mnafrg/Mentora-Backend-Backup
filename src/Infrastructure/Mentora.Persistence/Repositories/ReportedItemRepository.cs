using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Persistence.Repositories;

public class ReportedItemRepository : IReportedItemRepository
{
    private readonly ApplicationDbContext _ctx;
    public ReportedItemRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<ReportedItem?> GetByTargetAsync(ReportTargetType targetType, Guid targetId) =>
        await _ctx.ReportedItems
            .FirstOrDefaultAsync(i => i.TargetType == targetType && i.TargetId == targetId);

    public async Task<ReportedItem?> GetDetailAsync(Guid reportedItemId) =>
        await _ctx.ReportedItems
            .Include(i => i.OwnerUser)
            .Include(i => i.Reports)
                .ThenInclude(r => r.Reporter)
            .FirstOrDefaultAsync(i => i.ReportedItemId == reportedItemId);

    public async Task<(List<ReportedItem> Items, int Total)> GetQueueAsync(
        ReportedItemStatus? status,
        ReportTargetType?   targetType,
        string              sortBy,
        int                 skip,
        int                 take)
    {
        var query = _ctx.ReportedItems
            .Include(i => i.OwnerUser)
            .Include(i => i.Reports)
            .AsQueryable();

        // Default to showing items that reached the threshold
        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);
        else
            query = query.Where(i =>
                i.Status == ReportedItemStatus.FlaggedForReview ||
                i.Status == ReportedItemStatus.ActionTaken);

        if (targetType.HasValue)
            query = query.Where(i => i.TargetType == targetType.Value);

        var total = await query.CountAsync();

        query = sortBy.Equals("MostRecent", StringComparison.OrdinalIgnoreCase)
            ? query.OrderByDescending(i => i.UpdatedAt)
            : query.OrderByDescending(i => i.ReportScore);

        var items = await query.Skip(skip).Take(take).ToListAsync();
        return (items, total);
    }

    public async Task CreateAsync(ReportedItem item)
    {
        await _ctx.ReportedItems.AddAsync(item);
    }

    public async Task UpdateAsync(ReportedItem item)
    {
        _ctx.ReportedItems.Update(item);
    }
}