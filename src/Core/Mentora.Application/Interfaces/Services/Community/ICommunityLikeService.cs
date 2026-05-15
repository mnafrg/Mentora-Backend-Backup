using Mentora.Application.DTOs.Common;

using Mentora.Application.DTOs.Community;


namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityLikeService
{

Task<ApiResponse<ToggleInteractionResponseDto>>
    ToggleLikeAsync(Guid postId, Guid userId);

}
