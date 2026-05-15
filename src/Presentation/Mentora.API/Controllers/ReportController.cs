using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Interactions.Report;
using Mentora.Application.Interfaces;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }


    /// POST /api/report
    /// Submit a report against a User, Post, Event, or Comment.
    /// Any authenticated user can call this.
    /// Duplicate reports (same reporter, same target) are rejected.
    /// When the aggregate score reaches the threshold (3) the item is
    /// automatically flagged for admin review.
    [HttpPost]
    public async Task<IActionResult> SubmitReport([FromBody] SubmitReportRequest request)
    {
        var reporterId = User.GetUserId();
        var result     = await _reportService.SubmitReportAsync(reporterId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// GET /api/report/admin/queue
    /// Query params:
    ///   status     – Open | FlaggedForReview | ActionTaken | Cleared (optional)
    ///   targetType – User | Post | Event | Comment (optional)
    ///   sortBy     – HighestScore (default) | MostRecent
    ///   page       – default 1
    ///   pageSize   – default 20, max 100
    [HttpGet("admin/queue")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetQueue([FromQuery] ReportQueueFilterDto filter)
    {
        if (filter.Page < 1)         filter.Page     = 1;
        if (filter.PageSize < 1)     filter.PageSize = 20;
        if (filter.PageSize > 100)   filter.PageSize = 100;

        if (!new[] { "HighestScore", "MostRecent" }
                .Contains(filter.SortBy, StringComparer.OrdinalIgnoreCase))
            filter.SortBy = "HighestScore";

        var result = await _reportService.GetQueueAsync(filter);
        return Ok(result);
    }


    /// GET /api/report/admin/{reportedItemId}
    /// Full context view for a single reported item:
    ///   - Content snapshot
    ///   - All individual report submissions with reporter info
    ///   - Reason breakdown (histogram)
    ///   - Owner moderation history (past warnings + bans)
    ///   - Current resolution status

    [HttpGet("admin/{reportedItemId:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetDetail(Guid reportedItemId)
    {
        var result = await _reportService.GetDetailAsync(reportedItemId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// POST /api/report/admin/{reportedItemId}/action
    /// Apply content-level and/or user-level actions on a flagged item.
    ///
    /// Content actions:  None | Approved | ContentDeleted
    /// User actions:     None | Warning  | TemporaryBan | PermanentBan
    ///
    /// Rules:
    ///   - TemporaryBan requires BanDurationHours > 0
    ///   - Approved + None  → item status becomes Cleared
    ///   - Any other combo  → item status becomes ActionTaken
    ///   - TemporaryBan / PermanentBan sets user.IsActive = false
    [HttpPost("admin/{reportedItemId:guid}/action")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ApplyAction(
        Guid reportedItemId,
        [FromBody] AdminActionRequest request)
    {
        var adminId = User.GetUserId();
        var result  = await _reportService.ApplyActionAsync(adminId, reportedItemId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}