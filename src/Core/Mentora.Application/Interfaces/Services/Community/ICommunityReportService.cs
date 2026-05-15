using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Domain.Enums;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityReportService
{
    Task<ApiResponse<bool>> CreateReportAsync(Guid communityId, Guid postId, CreateReportDto dto, Guid userId);
    Task<ApiResponse<IEnumerable<ReportResponseDto>>> GetAllReportsByPostAsync(Guid postId);
    Task<ApiResponse<bool>> UpdateReportStatusAsync(Guid reportId, CommunityReportStatus newStatus, Guid userId);
    Task<ApiResponse<bool>> DeleteReportAsync(Guid reportId, Guid userId);
}
