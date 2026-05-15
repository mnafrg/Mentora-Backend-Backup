using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunitySaveService
{

Task<ApiResponse<ToggleInteractionResponseDto>>
    ToggleSaveAsync(Guid postId, Guid userId);

    Task<ApiResponse<IEnumerable<PostResponseDto>>> GetAllSavedPostsAsync(Guid userId);
}
