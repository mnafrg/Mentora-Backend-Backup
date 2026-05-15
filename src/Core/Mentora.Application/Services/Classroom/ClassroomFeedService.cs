using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Entities.Classroom;
using Mentora.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services.Classroom;

public class ClassroomFeedService : IClassroomFeedService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClassroomFeedService> _logger;

    public ClassroomFeedService(IUnitOfWork unitOfWork, ILogger<ClassroomFeedService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // ── Posts ────────────────────────────────────────────────────────────────

    public async Task<ApiResponse<PostFeedResponse>> GetFeedAsync(
        int programId, Guid requesterId, int page, int pageSize)
    {
        if (!await RequesterHasAccessAsync(programId, requesterId))
            return ApiResponse<PostFeedResponse>.ErrorResponse("Access denied");

        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<PostFeedResponse>.ErrorResponse("Classroom not found");

        var skip = (page - 1) * pageSize;
        var (posts, total) = await _unitOfWork.ClassroomPosts.GetFeedAsync(
            classroom.ClassroomId, skip, pageSize);

        var postDtos = posts.Select(p => MapPostToDto(p, requesterId, previewComments: true)).ToList();

        return ApiResponse<PostFeedResponse>.SuccessResponse(new PostFeedResponse
        {
            Posts     = postDtos,
            TotalCount = total,
            Page      = page,
            PageSize  = pageSize
        });
    }

    public async Task<ApiResponse<PostDto>> CreatePostAsync(
        int programId, Guid authorId, CreatePostRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return ApiResponse<PostDto>.ErrorResponse("Post content cannot be empty");

        if (!await RequesterHasAccessAsync(programId, authorId))
            return ApiResponse<PostDto>.ErrorResponse("Access denied");

        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<PostDto>.ErrorResponse("Classroom not found");

        var post = new ClassroomPost
        {
            ClassroomId = classroom.ClassroomId,
            AuthorId    = authorId,
            Content     = dto.Content.Trim(),
            CreatedAt   = DateTime.UtcNow
        };

        await _unitOfWork.ClassroomPosts.CreateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        // Reload with full navigation for response
        var created = await _unitOfWork.ClassroomPosts.GetByIdAsync(post.PostId);
        return ApiResponse<PostDto>.SuccessResponse(
            MapPostToDto(created!, authorId), "Post created successfully");
    }

    public async Task<ApiResponse<PostDto>> UpdatePostAsync(
        int postId, Guid requesterId, UpdatePostRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return ApiResponse<PostDto>.ErrorResponse("Post content cannot be empty");

        var post = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (post == null)
            return ApiResponse<PostDto>.ErrorResponse("Post not found");

        if (post.AuthorId != requesterId)
            return ApiResponse<PostDto>.ErrorResponse("You can only edit your own posts");

        post.Content   = dto.Content.Trim();
        post.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClassroomPosts.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        return ApiResponse<PostDto>.SuccessResponse(
            MapPostToDto(updated!, requesterId), "Post updated successfully");
    }

    public async Task<ApiResponse<bool>> DeletePostAsync(int postId, Guid requesterId)
    {
        var post = await _unitOfWork.ClassroomPosts.GetOwnershipAsync(postId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        // Author OR the mentor who owns the classroom can delete
        var program = await GetProgramByClassroomIdAsync(post.Value.ClassroomId);
        bool isMentorOwner = program?.MentorProfileId == requesterId;

        if (post.Value.AuthorId != requesterId && !isMentorOwner)
            return ApiResponse<bool>.ErrorResponse("Access denied");

        var fullPost = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (fullPost == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        fullPost.IsDeleted = true;
        fullPost.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClassroomPosts.UpdateAsync(fullPost);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Post deleted successfully");
    }

    public async Task<ApiResponse<bool>> TogglePinPostAsync(int postId, Guid mentorProfileId)
    {
        var post = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        var program = await GetProgramByClassroomIdAsync(post.ClassroomId);
        if (program?.MentorProfileId != mentorProfileId)
            return ApiResponse<bool>.ErrorResponse("Only the mentor can pin posts");

        post.IsPinned  = !post.IsPinned;
        post.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClassroomPosts.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        var msg = post.IsPinned ? "Post pinned successfully" : "Post unpinned successfully";
        return ApiResponse<bool>.SuccessResponse(post.IsPinned, msg);
    }

    public async Task<ApiResponse<bool>> ToggleLikePostAsync(int postId, Guid userId)
    {
        var post = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        var existing = await _unitOfWork.ClassroomPosts.GetLikeAsync(postId, userId);
        if (existing != null)
        {
            await _unitOfWork.ClassroomPosts.DeleteLikeAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Post unliked");
        }

        await _unitOfWork.ClassroomPosts.CreateLikeAsync(new ClassroomPostLike
        {
            PostId    = postId,
            UserId    = userId,
            CreatedAt = DateTime.UtcNow
        });
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Post liked");
    }

    // ── Comments ─────────────────────────────────────────────────────────────

    public async Task<ApiResponse<List<CommentDto>>> GetCommentsAsync(
        int postId, Guid requesterId)
    {
        var post = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (post == null)
            return ApiResponse<List<CommentDto>>.ErrorResponse("Post not found");

        var comments = await _unitOfWork.ClassroomComments.GetByPostIdAsync(postId);
        var dtos     = comments.Select(c => MapCommentToDto(c, requesterId)).ToList();

        return ApiResponse<List<CommentDto>>.SuccessResponse(dtos);
    }

    public async Task<ApiResponse<CommentDto>> CreateCommentAsync(
        int postId, Guid authorId, CreateCommentRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return ApiResponse<CommentDto>.ErrorResponse("Comment content cannot be empty");

        var post = await _unitOfWork.ClassroomPosts.GetByIdAsync(postId);
        if (post == null)
            return ApiResponse<CommentDto>.ErrorResponse("Post not found");

        // Validate parent comment belongs to the same post
        if (dto.ParentCommentId.HasValue)
        {
            var parent = await _unitOfWork.ClassroomComments.GetByIdAsync(dto.ParentCommentId.Value);
            if (parent == null || parent.PostId != postId)
                return ApiResponse<CommentDto>.ErrorResponse("Parent comment not found");

            // Enforce one level of nesting: replies cannot be made on replies
            if (parent.ParentCommentId.HasValue)
                return ApiResponse<CommentDto>.ErrorResponse(
                    "Replies to replies are not supported. Reply to the top-level comment instead.");
        }

        var comment = new ClassroomComment
        {
            PostId          = postId,
            AuthorId        = authorId,
            ParentCommentId = dto.ParentCommentId,
            Content         = dto.Content.Trim(),
            CreatedAt       = DateTime.UtcNow
        };

        await _unitOfWork.ClassroomComments.CreateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.ClassroomComments.GetByIdAsync(comment.CommentId);
        return ApiResponse<CommentDto>.SuccessResponse(
            MapCommentToDto(created!, authorId), "Comment added successfully");
    }

    public async Task<ApiResponse<CommentDto>> UpdateCommentAsync(
        int commentId, Guid requesterId, UpdateCommentRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return ApiResponse<CommentDto>.ErrorResponse("Comment content cannot be empty");

        var comment = await _unitOfWork.ClassroomComments.GetByIdAsync(commentId);
        if (comment == null)
            return ApiResponse<CommentDto>.ErrorResponse("Comment not found");

        if (comment.AuthorId != requesterId)
            return ApiResponse<CommentDto>.ErrorResponse("You can only edit your own comments");

        comment.Content   = dto.Content.Trim();
        comment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClassroomComments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.ClassroomComments.GetByIdAsync(commentId);
        return ApiResponse<CommentDto>.SuccessResponse(
            MapCommentToDto(updated!, requesterId), "Comment updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteCommentAsync(int commentId, Guid requesterId)
    {
        var comment = await _unitOfWork.ClassroomComments.GetByIdAsync(commentId);
        if (comment == null)
            return ApiResponse<bool>.ErrorResponse("Comment not found");

        // Author OR mentor who owns the classroom
        var post = await _unitOfWork.ClassroomPosts.GetOwnershipAsync(comment.PostId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        var program = await GetProgramByClassroomIdAsync(post.Value.ClassroomId);
        bool isMentorOwner = program?.MentorProfileId == requesterId;

        if (comment.AuthorId != requesterId && !isMentorOwner)
            return ApiResponse<bool>.ErrorResponse("Access denied");

        comment.IsDeleted = true;
        comment.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClassroomComments.UpdateAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Comment deleted successfully");
    }

    public async Task<ApiResponse<bool>> ToggleLikeCommentAsync(int commentId, Guid userId)
    {
        var comment = await _unitOfWork.ClassroomComments.GetByIdAsync(commentId);
        if (comment == null)
            return ApiResponse<bool>.ErrorResponse("Comment not found");

        var existing = await _unitOfWork.ClassroomComments.GetLikeAsync(commentId, userId);
        if (existing != null)
        {
            await _unitOfWork.ClassroomComments.DeleteLikeAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Comment unliked");
        }

        await _unitOfWork.ClassroomComments.CreateLikeAsync(new ClassroomCommentLike
        {
            CommentId = commentId,
            UserId    = userId,
            CreatedAt = DateTime.UtcNow
        });
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Comment liked");
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<bool> RequesterHasAccessAsync(int programId, Guid requesterId)
    {
        var program = await _unitOfWork.Programs.GetByIdAsync(programId);
        if (program == null) return false;

        if (program.MentorProfileId == requesterId) return true;

        var applications = await _unitOfWork.MentorshipApplications.GetAllAsync(
            a => a.ProgramId       == programId
              && a.MenteeProfileId == requesterId
              && a.Status          == ApplicationStatus.Accepted);

        return applications.Any();
    }

    private async Task<Domain.Entities.Program?> GetProgramByClassroomIdAsync(int classroomId)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(classroomId);
        if (classroom == null) return null;
        return await _unitOfWork.Programs.GetByIdAsync(classroom.ProgramId);
    }

    // ── Mapping helpers ───────────────────────────────────────────────────────

    private static AuthorDto MapAuthor(Domain.Entities.User user) => new()
    {
        UserId            = user.UserId,
        FullName          = $"{user.FirstName} {user.LastName}",
        ProfilePictureUrl = user.MentorProfile?.ProfilePictureUrl
                         ?? user.MenteeProfile?.ProfilePictureUrl,
        Role              = user.Role?.ToString() ?? "Unknown"
    };

    private static CommentDto MapCommentToDto(
        ClassroomComment comment, Guid requesterId) => new()
    {
        CommentId  = comment.CommentId,
        Author     = MapAuthor(comment.Author),
        Content    = comment.Content,
        LikesCount = comment.Likes.Count,
        LikedByMe  = comment.Likes.Any(l => l.UserId == requesterId),
        CreatedAt  = comment.CreatedAt,
        UpdatedAt  = comment.UpdatedAt,
        Replies    = comment.Replies
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.CreatedAt)
            .Select(r => MapCommentToDto(r, requesterId))
            .ToList()
    };

    private static PostDto MapPostToDto(
        ClassroomPost post, Guid requesterId, bool previewComments = false)
    {
        var allTopLevelComments = post.Comments
            .Where(c => !c.IsDeleted && c.ParentCommentId == null)
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        var commentsForDto = previewComments
            ? allTopLevelComments.Take(2).ToList()
            : allTopLevelComments;

        return new PostDto
        {
            PostId        = post.PostId,
            Author        = MapAuthor(post.Author),
            Content       = post.Content,
            IsPinned      = post.IsPinned,
            LikesCount    = post.Likes.Count,
            LikedByMe     = post.Likes.Any(l => l.UserId == requesterId),
            CommentsCount = post.Comments.Count(c => !c.IsDeleted),
            CreatedAt     = post.CreatedAt,
            UpdatedAt     = post.UpdatedAt,
            LatestComments = commentsForDto
                .Select(c => MapCommentToDto(c, requesterId))
                .ToList()
        };
    }
}