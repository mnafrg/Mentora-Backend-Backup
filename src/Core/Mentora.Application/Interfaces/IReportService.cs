using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Interactions.Report;

namespace Mentora.Application.Interfaces;

public interface IReportService
{
    /// Submit a report against a user, post, event, or comment.
    /// Handles duplicate-submission guard, score increment, and threshold check.
    Task<ApiResponse<bool>> SubmitReportAsync(Guid reporterId, SubmitReportRequest request);

    /// Paged admin queue of flagged items.
    /// Supports sorting by HighestScore | MostRecent and filtering by status / target type.
    Task<ApiResponse<PagedResult<ReportedItemSummaryDto>>> GetQueueAsync(ReportQueueFilterDto filter);

    /// Full detail view for one reported item:
    /// content snapshot, all individual reports, reason breakdown, owner history.
    Task<ApiResponse<ReportedItemDetailDto>> GetDetailAsync(Guid reportedItemId);

    /// Apply content-level and/or user-level actions on a flagged item.
    /// Handles: Approve | DeleteContent + Warning | TemporaryBan | PermanentBan.
    Task<ApiResponse<bool>> ApplyActionAsync(Guid adminId, Guid reportedItemId, AdminActionRequest request);
}