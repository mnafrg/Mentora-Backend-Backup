using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface IClassroomFeedService
{
    // ── Posts ────────────────────────────────────────────────────────────────
    Task<ApiResponse<PostFeedResponse>> GetFeedAsync(
        int programId, Guid requesterId, int page, int pageSize);

    Task<ApiResponse<PostDto>> CreatePostAsync(
        int programId, Guid authorId, CreatePostRequest dto);

    Task<ApiResponse<PostDto>> UpdatePostAsync(
        int postId, Guid requesterId, UpdatePostRequest dto);

    Task<ApiResponse<bool>> DeletePostAsync(int postId, Guid requesterId);

    /// Mentor-only: toggle pinned state.
    Task<ApiResponse<bool>> TogglePinPostAsync(int postId, Guid mentorProfileId);

    Task<ApiResponse<bool>> ToggleLikePostAsync(int postId, Guid userId);

    // ── Comments ─────────────────────────────────────────────────────────────
    Task<ApiResponse<List<CommentDto>>> GetCommentsAsync(
        int postId, Guid requesterId);

    Task<ApiResponse<CommentDto>> CreateCommentAsync(
        int postId, Guid authorId, CreateCommentRequest dto);

    Task<ApiResponse<CommentDto>> UpdateCommentAsync(
        int commentId, Guid requesterId, UpdateCommentRequest dto);

    Task<ApiResponse<bool>> DeleteCommentAsync(int commentId, Guid requesterId);

    Task<ApiResponse<bool>> ToggleLikeCommentAsync(int commentId, Guid userId);
}