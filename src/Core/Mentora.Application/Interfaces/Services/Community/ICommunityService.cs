using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityService
{
    Task<ApiResponse<Guid>> CreateCommunityAsync(
        CreateCommunityDto dto,
        Guid userId);

    Task<ApiResponse<CommunityResponseDto>> GetCommunityAsync(
        Guid communityId,
        Guid currentUserId);

    Task<ApiResponse<IEnumerable<CommunityResponseDto>>>
        GetAllCommunitiesAsync(Guid currentUserId);

    Task<ApiResponse<IEnumerable<CommunityResponseDto>>>
        GetAllCommunitiesByMemberAsync(Guid userId);

    Task<ApiResponse<bool>> UpdateCommunityAsync(
        Guid communityId,
        UpdateCommunityDto dto,
        Guid userId);

    Task<ApiResponse<bool>> DeleteCommunityAsync(
        Guid communityId,
        Guid userId);
}
