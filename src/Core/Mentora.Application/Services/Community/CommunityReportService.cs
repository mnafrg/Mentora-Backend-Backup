using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;

namespace Mentora.Application.Services.Community;

public class CommunityReportService : ICommunityReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> CreateReportAsync(Guid communityId, Guid postId, CreateReportDto dto, Guid userId)
    {
     
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(postId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

     
        if (dto.TargetCommentId.HasValue)
        {
            var comment = await _unitOfWork.CommunityComments
                .GetCommentByIdAsync(dto.TargetCommentId.Value);
            if (comment == null)
                return ApiResponse<bool>.ErrorResponse("Comment not found");
        }

        var report = new CommunityReport
        {
            CommunityReportId = Guid.NewGuid(),
            CommunityId = communityId,
            ReporterUserId = userId,
            TargetPostId = postId,            
            TargetCommentId = dto.TargetCommentId,
            ReportReason = dto.ReportReason,
            Status = CommunityReportStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityReports.CreateAsync(report);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Report submitted successfully");
    }

    public async Task<ApiResponse<IEnumerable<ReportResponseDto>>> GetAllReportsByPostAsync(Guid postId)
    {
        var reports = await _unitOfWork.CommunityReports.GetAllReportsByPostIdAsync(postId);

        var response = reports.Select(r => new ReportResponseDto
        {
            CommunityReportId = r.CommunityReportId,
            ReportReason = r.ReportReason,
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            ReporterUserId = r.ReporterUserId,
            ReporterName = $"{r.Reporter.FirstName} {r.Reporter.LastName}",
            TargetPostId = r.TargetPostId,
            TargetCommentId = r.TargetCommentId,
            ReportsCount = reports.Count()
        });

        return ApiResponse<IEnumerable<ReportResponseDto>>.SuccessResponse(response);
    }

    public async Task<ApiResponse<bool>> UpdateReportStatusAsync(Guid reportId, CommunityReportStatus newStatus, Guid userId)
    {
        var report = await _unitOfWork.CommunityReports.GetReportByIdAsync(reportId);
        if (report == null)
            return ApiResponse<bool>.ErrorResponse("Report not found");

        // Check if user is Admin or Owner of the community
        var member = await _unitOfWork.CommunityMembers.GetMemberAsync(report.CommunityId, userId);
        if (member == null || (member.Role != CommunityRole.Owner && member.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("Only Owner or Admin can update report status");

        report.Status = newStatus;
        report.ReviewedAt = DateTime.UtcNow;
        report.ReviewedByUserId = userId;

        await _unitOfWork.CommunityReports.UpdateAsync(report);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Report status updated successfully");
    }
    public async Task<ApiResponse<bool>> DeleteReportAsync(Guid reportId, Guid userId)
    {
        var report = await _unitOfWork.CommunityReports.GetReportByIdAsync(reportId);
        if (report == null)
            return ApiResponse<bool>.ErrorResponse("Report not found");

      
        var member = await _unitOfWork.CommunityMembers
            .GetMemberAsync(report.CommunityId, userId);
        if (member == null || (member.Role != CommunityRole.Owner && member.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("You do not have permission to delete this report");

        await _unitOfWork.CommunityReports.DeleteAsync(report);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Report deleted successfully");
    }
}
