using Mentora.Application.DTOs.Common;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;

namespace Mentora.Application.Services.Community;

public class CommunityShareService : ICommunityShareService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityShareService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<string>> SharePostAsync(Guid postId, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(postId);
        if (post == null )
            return ApiResponse<string>.ErrorResponse("Post not found");

        var share = new CommunityPostShare
        {
            CommunityPostShareId = Guid.NewGuid(),
            CommunityPostId = postId,
            SharedByUserId = userId,
            SharedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityPostShares.CreateAsync(share);
        await _unitOfWork.SaveChangesAsync();

        var link = $"https://mentora.com/community/posts/{postId}";
        return ApiResponse<string>.SuccessResponse(link, "Post shared successfully");
    }
}
