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

    public async Task<ApiResponse<bool>> ToggleSaveAsync(Guid postId, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(postId);
        if (post == null )
            return ApiResponse<bool>.ErrorResponse("Post not found");

        var existingSave = await _unitOfWork.CommunityPostSaves.GetSaveAsync(postId, userId);

        if (existingSave != null)
        {
            await _unitOfWork.CommunityPostSaves.DeleteAsync(existingSave);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Post unsaved");
        }

        var save = new CommunityPostSave
        {
            CommunityPostSaveId = Guid.NewGuid(),
            CommunityPostId = postId,
            UserId = userId,
            SavedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityPostSaves.CreateAsync(save);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Post saved");
    }

    public async Task<ApiResponse<IEnumerable<PostResponseDto>>> GetAllSavedPostsAsync(Guid userId)
    {
        var saves = await _unitOfWork.CommunityPostSaves.GetAllSavedPostsByUserIdAsync(userId);

        var response = saves.Select(s => new PostResponseDto
        {
            CommunityPostId = s.CommunityPost.CommunityPostId,
            Content = s.CommunityPost.Content,
            ImageUrl = s.CommunityPost.ImageUrl,
            LinkUrl = s.CommunityPost.LinkUrl,
            CreatedAt = s.CommunityPost.CreatedAt,
            LikesCount = s.CommunityPost.Likes?.Count ?? 0,
            CommentsCount = s.CommunityPost.Comments?.Count() ?? 0,
          
            AuthorId = s.CommunityPost.Author.UserId,
            AuthorName = $"{s.CommunityPost.Author.FirstName} {s.CommunityPost.Author.LastName}",
            AuthorProfilePicture = s.CommunityPost.Author.MentorProfile?.ProfilePictureUrl
                ?? s.CommunityPost.Author.MenteeProfile?.ProfilePictureUrl
        });

        return ApiResponse<IEnumerable<PostResponseDto>>.SuccessResponse(response);
    }
}
