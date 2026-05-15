
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityPostService
{
    Task<ApiResponse<Guid>> CreatePostAsync(
        Guid communityId,
        CreatePostDto dto,
        Guid userId);

    Task<ApiResponse<PostResponseDto>> GetPostAsync(
        Guid postId,
        Guid currentUserId);

    Task<ApiResponse<PagedResult<PostResponseDto>>> GetAllPostsByCommunityAsync(
        Guid communityId,
        PaginationParamsDto pagination,
        Guid currentUserId);

    Task<ApiResponse<PostResponseDto>> UpdatePostAsync(
        Guid postId,
        UpdatePostDto dto,
        Guid userId);

    Task<ApiResponse<bool>> DeletePostAsync(
        Guid postId,
        Guid userId);

    Task<ApiResponse<PagedResult<FeedResponseDto>>> GetFeedAsync(
        PaginationParamsDto pagination,
        Guid currentUserId);
}

