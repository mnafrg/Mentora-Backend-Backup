
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;

namespace Mentora.Application.Services.Community;

public class CommunitySaveService : ICommunitySaveService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunitySaveService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<ToggleInteractionResponseDto>>
        ToggleSaveAsync(Guid postId, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts
            .GetPostByIdAsync(postId);

        if (post == null)
        {
            return ApiResponse<ToggleInteractionResponseDto>
                .ErrorResponse("Post not found");
        }

        var existingSave = await _unitOfWork.CommunityPostSaves
            .GetSaveAsync(postId, userId);

        if (existingSave != null)
        {
            await _unitOfWork.CommunityPostSaves
                .DeleteAsync(existingSave);

            await _unitOfWork.SaveChangesAsync();

            var savesCount =
                (await _unitOfWork.CommunityPostSaves
                    .GetAllSavedPostsByUserIdAsync(userId))
                .Count();

            return ApiResponse<ToggleInteractionResponseDto>
                .SuccessResponse(
                    new ToggleInteractionResponseDto
                    {
                        IsActive = false,
                        Count = savesCount
                    },
                    "Post unsaved");
        }

        var save = new CommunityPostSave
        {
            CommunityPostSaveId = Guid.NewGuid(),
            CommunityPostId = postId,
            UserId = userId,
            SavedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityPostSaves
            .CreateAsync(save);

        await _unitOfWork.SaveChangesAsync();

        var updatedSavesCount =
            (await _unitOfWork.CommunityPostSaves
                .GetAllSavedPostsByUserIdAsync(userId))
            .Count();

        return ApiResponse<ToggleInteractionResponseDto>
            .SuccessResponse(
                new ToggleInteractionResponseDto
                {
                    IsActive = true,
                    Count = updatedSavesCount
                },
                "Post saved");
    }

    public async Task<ApiResponse<IEnumerable<PostResponseDto>>>
        GetAllSavedPostsAsync(Guid userId)
    {
        var saves = await _unitOfWork.CommunityPostSaves
            .GetAllSavedPostsByUserIdAsync(userId);

        var response = saves.Select(s => new PostResponseDto
        {
            CommunityPostId = s.CommunityPost.CommunityPostId,
            Content = s.CommunityPost.Content,
            ImageUrl = s.CommunityPost.ImageUrl,
            LinkUrl = s.CommunityPost.LinkUrl,
            CreatedAt = s.CommunityPost.CreatedAt,

            LikesCount = s.CommunityPost.Likes?.Count ?? 0,
            CommentsCount = s.CommunityPost.Comments?.Count() ?? 0,

            AuthorId = s.CommunityPost.Author?.UserId ?? Guid.Empty,

            AuthorName = s.CommunityPost.Author != null
                ? $"{s.CommunityPost.Author.FirstName} {s.CommunityPost.Author.LastName}"
                : "Unknown User",

            AuthorProfilePicture =
                s.CommunityPost.Author?.MentorProfile?.ProfilePictureUrl
                ?? s.CommunityPost.Author?.MenteeProfile?.ProfilePictureUrl,

            IsSaved = true,

            CanEdit = s.CommunityPost.Author?.UserId == userId,
            CanDelete = s.CommunityPost.Author?.UserId == userId
        });

        return ApiResponse<IEnumerable<PostResponseDto>>
            .SuccessResponse(response);
    }
}

