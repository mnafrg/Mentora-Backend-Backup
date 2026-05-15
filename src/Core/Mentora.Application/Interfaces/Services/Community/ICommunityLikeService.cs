using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityLikeService
{
    Task<ApiResponse<bool>> ToggleLikeAsync(Guid postId, Guid userId);
}
