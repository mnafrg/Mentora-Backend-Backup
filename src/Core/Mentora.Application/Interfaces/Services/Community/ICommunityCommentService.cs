using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityCommentService
{
    Task<ApiResponse<Guid>> CreateCommentAsync(Guid postId, CreateCommentDto dto, Guid userId);
    Task<ApiResponse<IEnumerable<CommentResponseDto>>> GetAllCommentsAsync(Guid postId);
    Task<ApiResponse<CommentResponseDto>> UpdateCommentAsync(Guid commentId, UpdateCommentDto dto, Guid userId);
    Task<ApiResponse<bool>> DeleteCommentAsync(Guid commentId, Guid userId);
}
