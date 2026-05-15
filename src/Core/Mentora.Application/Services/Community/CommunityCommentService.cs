using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;

namespace Mentora.Application.Services.Community;

public class CommunityCommentService : ICommunityCommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityCommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<Guid>> CreateCommentAsync(Guid postId, CreateCommentDto dto, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(postId);
        if (post == null)
            return ApiResponse<Guid>.ErrorResponse("Post not found");

        var comment = new CommunityComment
        {
            CommunityPostId = postId,
            UserId = userId,
            CommentText = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityComments.CreateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<Guid>.SuccessResponse(comment.CommunityCommentId, "Comment added successfully");
    }
    public async Task<ApiResponse<IEnumerable<CommentResponseDto>>> GetAllCommentsAsync(Guid postId)
    {
        var comments = await _unitOfWork.CommunityComments.GetAllCommentsByPostIdAsync(postId);

        var response = comments.Select(c => MapToResponse(c, c.User));
        return ApiResponse<IEnumerable<CommentResponseDto>>.SuccessResponse(response);
    }

    public async Task<ApiResponse<CommentResponseDto>> UpdateCommentAsync(Guid commentId, UpdateCommentDto dto, Guid userId)
    {
        var comment = await _unitOfWork.CommunityComments.GetCommentByIdAsync(commentId);
        if (comment == null )
            return ApiResponse<CommentResponseDto>.ErrorResponse("Comment not found");

        if (comment.UserId != userId)
            return ApiResponse<CommentResponseDto>.ErrorResponse("Only the comment author can update");

        comment.CommentText = dto.Content;
        await _unitOfWork.CommunityComments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        var response = MapToResponse(comment, user!);

        return ApiResponse<CommentResponseDto>.SuccessResponse(response, "Comment updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _unitOfWork.CommunityComments.GetCommentByIdAsync(commentId);
        if (comment == null)
            return ApiResponse<bool>.ErrorResponse("Comment not found");
        if (comment.UserId == userId)
        {

            await _unitOfWork.CommunityComments.DeleteAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Comment deleted successfully");
        }
        // Check if user is Admin or Owner
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(comment.CommunityPostId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");
        var member = await _unitOfWork.CommunityMembers.GetMemberAsync(post.CommunityId, userId);
        if (member == null || (member.Role != CommunityRole.Owner && member.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("You do not have permission to delete this comment");

        await _unitOfWork.CommunityComments.DeleteAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Comment deleted successfully");
    }
  
    private static CommentResponseDto MapToResponse(CommunityComment comment, User user)
    {
        return new CommentResponseDto
        {
            CommunityCommentId = comment.CommunityCommentId,
            Content = comment.CommentText,
            CreatedAt = comment.CreatedAt,
            AuthorId = user.UserId,
            AuthorName = $"{user.FirstName} {user.LastName}",
            AuthorProfilePicture = user.MentorProfile?.ProfilePictureUrl
                ?? user.MenteeProfile?.ProfilePictureUrl
        };
    }
}
