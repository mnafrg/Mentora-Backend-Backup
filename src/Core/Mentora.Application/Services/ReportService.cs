using Microsoft.Extensions.Logging;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Interactions.Report;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Application.Services;

public class ReportService : IReportService
{
    //  Constants
    private const int ReportThreshold = 3;   // reports before admin queue

    private static readonly HashSet<string> ValidReasons =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Spam", "Harassment", "FakeProfile",
            "InappropriateContent", "Scam", "Other"
        };

    private readonly IUnitOfWork _uow;
    private readonly ILogger<ReportService> _logger;

    public ReportService(IUnitOfWork uow, ILogger<ReportService> logger)
    {
        _uow    = uow;
        _logger = logger;
    }


    public async Task<ApiResponse<bool>> SubmitReportAsync(
        Guid reporterId, SubmitReportRequest request)
    {
        // 1. Self-report guard
        if (reporterId == request.OwnerUserId)
            return ApiResponse<bool>.ErrorResponse("You cannot report yourself");

        // 2. Reason whitelist
        if (!ValidReasons.Contains(request.Reason))
            return ApiResponse<bool>.ErrorResponse(
                $"Invalid reason. Valid values: {string.Join(", ", ValidReasons)}");

        // 3. Verify owner user exists
        var owner = await _uow.Users.GetByIdAsync(request.OwnerUserId);
        if (owner == null)
            return ApiResponse<bool>.ErrorResponse("The reported user does not exist");

        // 4. Duplicate / spam guard — one report per reporter per target
        var alreadyReported = await _uow.Reports
            .ExistsByReporterAndTargetAsync(reporterId, request.TargetType, request.TargetId);
        if (alreadyReported)
            return ApiResponse<bool>.ErrorResponse(
                "You have already reported this content");

        await _uow.BeginTransactionAsync();
        try
        {
            // 5. Get or create the aggregate (ReportedItem)
            var item = await _uow.ReportedItems
                .GetByTargetAsync(request.TargetType, request.TargetId);

            var now = DateTime.UtcNow;

            if (item == null)
            {
                item = new ReportedItem
                {
                    ReportedItemId  = Guid.NewGuid(),
                    TargetType      = request.TargetType,
                    TargetId        = request.TargetId,
                    OwnerUserId     = request.OwnerUserId,
                    ReportScore     = 0,
                    ReportThreshold = ReportThreshold,
                    Status          = ReportedItemStatus.Open,
                    CreatedAt       = now,
                    UpdatedAt       = now
                };
                await _uow.ReportedItems.CreateAsync(item);
                await _uow.SaveChangesAsync();  // get the PK before creating child
            }

            // 6. Create the individual submission
            var report = new Report
            {
                ReportId       = Guid.NewGuid(),
                ReportedItemId = item.ReportedItemId,
                ReporterId     = reporterId,
                Reason         = request.Reason,
                Description    = request.Description?.Trim(),
                Status         = ReportStatus.Pending,
                CreatedAt      = now
            };
            await _uow.Reports.CreateAsync(report);

            // 7. Increment score
            item.ReportScore++;
            item.UpdatedAt = now;

            // 8. Threshold check → flag for admin if reached
            if (item.ReportScore >= item.ReportThreshold &&
                item.Status == ReportedItemStatus.Open)
            {
                item.Status = ReportedItemStatus.FlaggedForReview;
                _logger.LogInformation(
                    "ReportedItem {Id} ({Type}:{Target}) flagged for admin review (score={Score})",
                    item.ReportedItemId, item.TargetType, item.TargetId, item.ReportScore);
            }

            await _uow.ReportedItems.UpdateAsync(item);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();

            return ApiResponse<bool>.SuccessResponse(
                true, "Your report has been submitted. Our team will review it.");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();
            _logger.LogError(ex, "Error in SubmitReportAsync");
            return ApiResponse<bool>.ErrorResponse("Failed to submit report. Please try again.");
        }
    }

    public async Task<ApiResponse<PagedResult<ReportedItemSummaryDto>>> GetQueueAsync(
        ReportQueueFilterDto filter)
    {
        var skip = (filter.Page - 1) * filter.PageSize;

        var (items, total) = await _uow.ReportedItems.GetQueueAsync(
            filter.Status,
            filter.TargetType,
            filter.SortBy,
            skip,
            filter.PageSize);

        var dtos = items.Select(ToSummaryDto).ToList();

        return ApiResponse<PagedResult<ReportedItemSummaryDto>>.SuccessResponse(
            new PagedResult<ReportedItemSummaryDto>
            {
                Items      = dtos,
                TotalCount = total,
                Page       = filter.Page,
                PageSize   = filter.PageSize
            });
    }

    public async Task<ApiResponse<ReportedItemDetailDto>> GetDetailAsync(Guid reportedItemId)
    {
        var item = await _uow.ReportedItems.GetDetailAsync(reportedItemId);
        if (item == null)
            return ApiResponse<ReportedItemDetailDto>.ErrorResponse("Reported item not found");

        // Load owner moderation history in parallel
        var bansTask     = _uow.UserBans.GetHistoryAsync(item.OwnerUserId);
        var warningsTask = _uow.UserWarnings.GetByUserIdAsync(item.OwnerUserId);
        await Task.WhenAll(bansTask, warningsTask);

        var bans     = bansTask.Result;
        var warnings = warningsTask.Result;

        // Build reason breakdown
        var breakdown = item.Reports
            .GroupBy(r => r.Reason)
            .ToDictionary(g => g.Key, g => g.Count());

        // Build owner history from warnings + bans
        var history = new List<UserActionHistoryDto>();

        foreach (var w in warnings)
            history.Add(new UserActionHistoryDto
            {
                ActionType = "Warning",
                Message    = w.Message,
                IssuedAt   = w.IssuedAt,
                IsActive   = true   // warnings don't expire
            });

        foreach (var b in bans)
            history.Add(new UserActionHistoryDto
            {
                ActionType = b.IsPermanent ? "PermanentBan" : "TemporaryBan",
                Message    = b.Reason,
                IssuedAt   = b.IssuedAt,
                ExpiresAt  = b.ExpiresAt,
                IsActive   = !b.IsRevoked &&
                             (b.IsPermanent || b.ExpiresAt > DateTime.UtcNow)
            });

        history = history.OrderByDescending(h => h.IssuedAt).ToList();

        // Build content snapshot
        var snapshot = BuildContentSnapshot(item);

        // Owner profile picture
        var ownerPicture = item.OwnerUser.MentorProfile?.ProfilePictureUrl
                        ?? item.OwnerUser.MenteeProfile?.ProfilePictureUrl;

        var dto = new ReportedItemDetailDto
        {
            ReportedItemId  = item.ReportedItemId,
            TargetType      = item.TargetType.ToString(),
            TargetId        = item.TargetId,
            Content         = snapshot,
            OwnerUserId     = item.OwnerUserId,
            OwnerName       = $"{item.OwnerUser.FirstName} {item.OwnerUser.LastName}",
            OwnerPictureUrl = ownerPicture,
            OwnerRole       = item.OwnerUser.Role.ToString(),
            OwnerJoinedAt   = item.OwnerUser.CreatedAt,
            ReportScore     = item.ReportScore,
            ReportThreshold = item.ReportThreshold,
            Status          = item.Status.ToString(),
            CreatedAt       = item.CreatedAt,
            UpdatedAt       = item.UpdatedAt,
            ContentAction   = item.ContentAction != ContentAction.None
                                  ? item.ContentAction.ToString() : null,
            UserAction      = item.UserAction != UserAction.None
                                  ? item.UserAction.ToString() : null,
            AdminNotes      = item.AdminNotes,
            ResolvedAt      = item.ResolvedAt,
            ReasonBreakdown = breakdown,
            OwnerHistory    = history,

            Reports = item.Reports
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReportSubmissionDto
                {
                    ReportId           = r.ReportId,
                    ReporterId         = r.ReporterId,
                    ReporterName       = $"{r.Reporter.FirstName} {r.Reporter.LastName}",
                    ReporterPictureUrl = r.Reporter.MentorProfile?.ProfilePictureUrl
                                     ?? r.Reporter.MenteeProfile?.ProfilePictureUrl,
                    Reason             = r.Reason,
                    Description        = r.Description,
                    CreatedAt          = r.CreatedAt
                }).ToList()
        };

        return ApiResponse<ReportedItemDetailDto>.SuccessResponse(dto);
    }

    public async Task<ApiResponse<bool>> ApplyActionAsync(
        Guid adminId, Guid reportedItemId, AdminActionRequest request)
    {
        // Validate TemporaryBan requires duration
        if (request.UserAction == UserAction.TemporaryBan &&
            (request.BanDurationHours == null || request.BanDurationHours <= 0))
        {
            return ApiResponse<bool>.ErrorResponse(
                "BanDurationHours is required and must be greater than 0 for a temporary ban");
        }

        var item = await _uow.ReportedItems.GetDetailAsync(reportedItemId);
        if (item == null)
            return ApiResponse<bool>.ErrorResponse("Reported item not found");

        await _uow.BeginTransactionAsync();
        try
        {
            var now = DateTime.UtcNow;

            //  Apply content/User action 
            item.ContentAction = request.ContentAction;

            item.UserAction = request.UserAction;

            switch (request.UserAction)
            {
                case UserAction.Warning:
                    var warning = new UserWarning
                    {
                        WarningId      = Guid.NewGuid(),
                        UserId         = item.OwnerUserId,
                        Message        = request.UserActionMessage
                                         ?? "Your content violated our community guidelines.",
                        ReportedItemId = item.ReportedItemId,
                        IssuedBy       = adminId,
                        IssuedAt       = now
                    };
                    await _uow.UserWarnings.CreateAsync(warning);
                    break;

                case UserAction.TemporaryBan:
                    var tempBan = new UserBan
                    {
                        BanId          = Guid.NewGuid(),
                        UserId         = item.OwnerUserId,
                        IsPermanent    = false,
                        ExpiresAt      = now.AddHours(request.BanDurationHours!.Value),
                        Reason         = request.UserActionMessage
                                         ?? $"Temporary ban ({request.BanDurationHours}h)",
                        ReportedItemId = item.ReportedItemId,
                        IssuedBy       = adminId,
                        IssuedAt       = now
                    };
                    await _uow.UserBans.CreateAsync(tempBan);

                    // Deactivate the user account for the ban duration
                    var tempBanUser = await _uow.Users.GetByIdAsync(item.OwnerUserId);
                    if (tempBanUser != null)
                    {
                        tempBanUser.IsActive  = false;
                        tempBanUser.UpdatedAt = now;
                        await _uow.Users.UpdateAsync(tempBanUser);
                    }
                    break;

                case UserAction.PermanentBan:
                    var permBan = new UserBan
                    {
                        BanId          = Guid.NewGuid(),
                        UserId         = item.OwnerUserId,
                        IsPermanent    = true,
                        ExpiresAt      = null,
                        Reason         = request.UserActionMessage
                                         ?? "Permanent ban due to repeated violations",
                        ReportedItemId = item.ReportedItemId,
                        IssuedBy       = adminId,
                        IssuedAt       = now
                    };
                    await _uow.UserBans.CreateAsync(permBan);

                    // Permanently deactivate user
                    var permBanUser = await _uow.Users.GetByIdAsync(item.OwnerUserId);
                    if (permBanUser != null)
                    {
                        permBanUser.IsActive  = false;
                        permBanUser.UpdatedAt = now;
                        await _uow.Users.UpdateAsync(permBanUser);
                    }
                    break;
            }

            // ── Resolve the reported item ─────────────────────────────────────
            item.Status     = request.ContentAction == ContentAction.Approved &&
                              request.UserAction     == UserAction.None
                                  ? ReportedItemStatus.Cleared
                                  : ReportedItemStatus.ActionTaken;

            item.AdminNotes = request.AdminNotes;
            item.ResolvedAt = now;
            item.ResolvedBy = adminId;
            item.UpdatedAt  = now;

            await _uow.ReportedItems.UpdateAsync(item);
            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Action applied successfully");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();
            _logger.LogError(ex, "Error in ApplyActionAsync for ReportedItem {Id}", reportedItemId);
            return ApiResponse<bool>.ErrorResponse("Failed to apply action. Please try again.");
        }
    }


    // PRIVATE HELPERS

    private static ReportedItemSummaryDto ToSummaryDto(ReportedItem item)
    {
        var topReason = item.Reports
            .GroupBy(r => r.Reason)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "—";

        var ownerPicture = item.OwnerUser?.MentorProfile?.ProfilePictureUrl
                        ?? item.OwnerUser?.MenteeProfile?.ProfilePictureUrl;

        return new ReportedItemSummaryDto
        {
            ReportedItemId  = item.ReportedItemId,
            TargetType      = item.TargetType.ToString(),
            TargetId        = item.TargetId,
            OwnerName       = item.OwnerUser != null
                                  ? $"{item.OwnerUser.FirstName} {item.OwnerUser.LastName}"
                                  : "Unknown",
            OwnerPictureUrl = ownerPicture,
            ReportScore     = item.ReportScore,
            ReportThreshold = item.ReportThreshold,
            Status          = item.Status.ToString(),
            CreatedAt       = item.CreatedAt,
            UpdatedAt       = item.UpdatedAt,
            TopReason       = topReason,
            TotalReports    = item.Reports.Count
        };
    }

    /// Builds a content snapshot based on TargetType.
    /// For User targets, reads basic profile info from the navigation already loaded.
    /// For Post / Event / Comment the body fields are left null — they will be
    /// populated once those tables exist (inject the relevant repository then).

    private static ReportedContentSnapshotDto BuildContentSnapshot(ReportedItem item)
    {
        if (item.TargetType == ReportTargetType.User)
        {
            return new ReportedContentSnapshotDto
            {
                Title     = $"{item.OwnerUser.FirstName} {item.OwnerUser.LastName}",
                Body      = item.OwnerUser.MenteeProfile?.Bio
                         ?? item.OwnerUser.MentorProfile?.Bio,
                CreatedAt = item.OwnerUser.CreatedAt
            };
        }

        // Post / Event / Comment — stubs until those tables are built
        return new ReportedContentSnapshotDto
        {
            Title     = $"{item.TargetType} #{item.TargetId}",
            Body      = "[Content preview available once the Post/Event/Comment module is implemented]",
            CreatedAt = item.CreatedAt
        };
    }
}