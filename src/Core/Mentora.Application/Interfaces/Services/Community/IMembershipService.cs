using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Domain.Enums;

namespace Mentora.Application.Interfaces.Services.Community;

public interface IMembershipService
{
    Task<ApiResponse<bool>> JoinCommunityAsync(Guid communityId, Guid userId);
    Task<ApiResponse<bool>> LeaveCommunityAsync(Guid communityId, Guid userId);
    Task<ApiResponse<bool>> RemoveMemberAsync(Guid communityId, Guid targetUserId, Guid userId);
    Task<ApiResponse<bool>> UpdateMemberRoleAsync(Guid communityId, Guid targetUserId, CommunityRole newRole, Guid userId);
    Task<ApiResponse<bool>> BanMemberAsync(Guid communityId, Guid targetUserId, Guid userId);
    Task<ApiResponse<IEnumerable<MemberResponseDto>>> GetAllAdminsAsync(Guid communityId);
    Task<ApiResponse<IEnumerable<MemberResponseDto>>> GetAllMembersAsync(Guid communityId);
}
