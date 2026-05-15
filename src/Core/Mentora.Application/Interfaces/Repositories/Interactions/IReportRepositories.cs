using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Application.Interfaces.Repositories.Interactions;

public interface IReportedItemRepository
{
    /// Find existing aggregate for this target, or null if first report.
    Task<ReportedItem?> GetByTargetAsync(ReportTargetType targetType, Guid targetId);
    
    /// Load full detail including all Reports and Reporter navigations.
    Task<ReportedItem?> GetDetailAsync(Guid reportedItemId);

    /// Admin queue: items that have reached the threshold (FlaggedForReview or ActionTaken).
    /// Supports sorting by ReportScore desc or UpdatedAt desc.
    /// Returns (items, totalCount) for pagination.
    Task<(List<ReportedItem> Items, int Total)> GetQueueAsync(
        ReportedItemStatus? status,
        ReportTargetType?   targetType,
        string              sortBy,
        int                 skip,
        int                 take);

    Task CreateAsync(ReportedItem item);
    Task UpdateAsync(ReportedItem item);
}

