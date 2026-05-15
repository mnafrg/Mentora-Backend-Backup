
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;

namespace Mentora.Application.Services.Community;

public class CommunityLikeService : ICommunityLikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityLikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<ToggleInteractionResponseDto>>
        ToggleLikeAsync(Guid postId, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts
            .GetPostByIdAsync(postId);

        if (post == null)
        {
            return ApiResponse<ToggleInteractionResponseDto>
                .ErrorResponse("Post not found");
        }

        var existingLike = await _unitOfWork.CommunityPostLikes
            .GetLikeAsync(postId, userId);

        if (existingLike != null)
        {
            await _unitOfWork.CommunityPostLikes
                .DeleteAsync(existingLike);

            await _unitOfWork.SaveChangesAsync();

            var likesCount =
                (await _unitOfWork.CommunityPostLikes
                    .GetAllLikesByPostIdAsync(postId))
                .Count();

            return ApiResponse<ToggleInteractionResponseDto>
                .SuccessResponse(
                    new ToggleInteractionResponseDto
                    {
                        IsActive = false,
                        Count = likesCount
                    },
                    "Post unliked");
        }

        var like = new CommunityPostLike
        {
            CommunityPostLikeId = Guid.NewGuid(),
            CommunityPostId = postId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityPostLikes
            .CreateAsync(like);

        await _unitOfWork.SaveChangesAsync();

        var updatedLikesCount =
            (await _unitOfWork.CommunityPostLikes
                .GetAllLikesByPostIdAsync(postId))
            .Count();

        return ApiResponse<ToggleInteractionResponseDto>
            .SuccessResponse(
                new ToggleInteractionResponseDto
                {
                    IsActive = true,
                    Count = updatedLikesCount
                },
                "Post liked");
    }
}

