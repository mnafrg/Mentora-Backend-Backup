using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;

namespace Mentora.Application.Services.Community;

public class CommunityPostService : ICommunityPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityPostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

public async Task<ApiResponse<Guid>> CreatePostAsync(
    Guid communityId,
    CreatePostDto dto,
    Guid userId)
{
    Console.WriteLine($"USER ID = {userId}");

    var member = await _unitOfWork.CommunityMembers
        .GetMemberAsync(communityId, userId);

    Console.WriteLine($"MEMBER FOUND = {member != null}");

    if (member != null)
    {
        Console.WriteLine($"MEMBER ROLE = {member.Role}");
    }

    if (member == null)
        return ApiResponse<Guid>
            .ErrorResponse("Only members can create posts");

    var post = new CommunityPost
    {
        CommunityPostId = Guid.NewGuid(),
        CommunityId = communityId,
        AuthorUserId = userId,
        Content = dto.Content,
        ImageUrl = dto.ImageUrl,
        LinkUrl = dto.LinkUrl,
        CreatedAt = DateTime.UtcNow
    };

    await _unitOfWork.CommunityPosts.CreateAsync(post);
    await _unitOfWork.SaveChangesAsync();

    return ApiResponse<Guid>
        .SuccessResponse(post.CommunityPostId, "Post created successfully");
}



    public async Task<ApiResponse<PostResponseDto>> GetPostAsync(
        Guid postId,
        Guid currentUserId)
    {
        var post = await _unitOfWork.CommunityPosts
            .GetPostWithDetailsAsync(postId);

        if (post == null)
            return ApiResponse<PostResponseDto>
                .ErrorResponse("Post not found");

        var existingLike = await _unitOfWork.CommunityPostLikes
            .GetLikeAsync(postId, currentUserId);

        var existingSave = await _unitOfWork.CommunityPostSaves
            .GetSaveAsync(postId, currentUserId);

        var response = new PostResponseDto
        {
            CommunityPostId = post.CommunityPostId,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            LinkUrl = post.LinkUrl,
            CreatedAt = post.CreatedAt,

            LikesCount = post.Likes?.Count ?? 0,
            CommentsCount = post.Comments?.Count() ?? 0,
            SharesCount = 0,

            AuthorId = post.AuthorUserId,

            AuthorName = post.Author != null
                ? $"{post.Author.FirstName} {post.Author.LastName}"
                : "Unknown User",

            AuthorProfilePicture =
                post.Author?.MentorProfile?.ProfilePictureUrl
                ?? post.Author?.MenteeProfile?.ProfilePictureUrl,

            IsLiked = existingLike != null,
            IsSaved = existingSave != null,

            CanEdit = post.AuthorUserId == currentUserId,
            CanDelete = post.AuthorUserId == currentUserId
        };

        return ApiResponse<PostResponseDto>
            .SuccessResponse(response);
    }

    public async Task<ApiResponse<PagedResult<PostResponseDto>>>
        GetAllPostsByCommunityAsync(
            Guid communityId,
            PaginationParamsDto pagination,
            Guid currentUserId)
    {
        var posts = await _unitOfWork.CommunityPosts
            .GetAllPostsByCommunityIdAsync(communityId);

        if (posts == null)
        {
            return ApiResponse<PagedResult<PostResponseDto>>
                .SuccessResponse(new PagedResult<PostResponseDto>());
        }

        var totalCount = posts.Count();

        var paginatedPosts = posts
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var postDtos = new List<PostResponseDto>();

        foreach (var p in paginatedPosts)
        {
            var existingLike = await _unitOfWork.CommunityPostLikes
                .GetLikeAsync(p.CommunityPostId, currentUserId);

            var existingSave = await _unitOfWork.CommunityPostSaves
                .GetSaveAsync(p.CommunityPostId, currentUserId);

            postDtos.Add(new PostResponseDto
            {
                CommunityPostId = p.CommunityPostId,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                LinkUrl = p.LinkUrl,
                CreatedAt = p.CreatedAt,

                AuthorId = p.AuthorUserId,

                AuthorName = p.Author != null
                    ? $"{p.Author.FirstName} {p.Author.LastName}"
                    : "Unknown User",

                AuthorProfilePicture =
                    p.Author?.MentorProfile?.ProfilePictureUrl
                    ?? p.Author?.MenteeProfile?.ProfilePictureUrl,

                LikesCount = p.Likes?.Count ?? 0,
                CommentsCount = p.Comments?.Count() ?? 0,
                SharesCount = 0,

                IsLiked = existingLike != null,
                IsSaved = existingSave != null,

                CanEdit = p.AuthorUserId == currentUserId,
                CanDelete = p.AuthorUserId == currentUserId
            });
        }

        var result = new PagedResult<PostResponseDto>
        {
            Items = postDtos,
            TotalCount = totalCount,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };

        return ApiResponse<PagedResult<PostResponseDto>>
            .SuccessResponse(result);
    }

    public async Task<ApiResponse<PostResponseDto>> UpdatePostAsync(
        Guid postId,
        UpdatePostDto dto,
        Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts
            .GetPostWithDetailsAsync(postId);

        if (post == null)
        {
            return ApiResponse<PostResponseDto>
                .ErrorResponse("Post not found");
        }

        if (post.AuthorUserId != userId)
        {
            return ApiResponse<PostResponseDto>
                .ErrorResponse("Only the post author can update");
        }

        if (dto.Content != null)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return ApiResponse<PostResponseDto>
                    .ErrorResponse("Content cannot be empty");
            }

            post.Content = dto.Content;
        }

        if (dto.ImageUrl != null)
            post.ImageUrl = dto.ImageUrl;

        if (dto.LinkUrl != null)
            post.LinkUrl = dto.LinkUrl;

        post.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CommunityPosts.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        var existingLike = await _unitOfWork.CommunityPostLikes
            .GetLikeAsync(post.CommunityPostId, userId);

        var existingSave = await _unitOfWork.CommunityPostSaves
            .GetSaveAsync(post.CommunityPostId, userId);

        var response = new PostResponseDto
        {
            CommunityPostId = post.CommunityPostId,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            LinkUrl = post.LinkUrl,
            CreatedAt = post.CreatedAt,

            LikesCount = post.Likes?.Count ?? 0,
            CommentsCount = post.Comments?.Count() ?? 0,
            SharesCount = 0,

            AuthorId = post.AuthorUserId,

            AuthorName = post.Author != null
                ? $"{post.Author.FirstName} {post.Author.LastName}"
                : "Unknown User",

            AuthorProfilePicture =
                post.Author?.MentorProfile?.ProfilePictureUrl
                ?? post.Author?.MenteeProfile?.ProfilePictureUrl,

            IsLiked = existingLike != null,
            IsSaved = existingSave != null,

            CanEdit = true,
            CanDelete = true
        };

        return ApiResponse<PostResponseDto>
            .SuccessResponse(response, "Post updated successfully");
    }

    public async Task<ApiResponse<bool>> DeletePostAsync(
        Guid postId,
        Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts
            .GetPostByIdAsync(postId);

        if (post == null)
        {
            return ApiResponse<bool>
                .ErrorResponse("Post not found");
        }

        if (post.AuthorUserId == userId)
        {
            await _unitOfWork.CommunityPosts.DeleteAsync(post);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>
                .SuccessResponse(true, "Post deleted successfully");
        }

        var member = await _unitOfWork.CommunityMembers
            .GetMemberAsync(post.CommunityId, userId);

        if (member == null ||
            (member.Role != CommunityRole.Owner &&
             member.Role != CommunityRole.Admin))
        {
            return ApiResponse<bool>
                .ErrorResponse("You do not have permission to delete this post");
        }

        await _unitOfWork.CommunityPosts.DeleteAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>
            .SuccessResponse(true, "Post deleted successfully");
    }

    public async Task<ApiResponse<PagedResult<FeedResponseDto>>> GetFeedAsync(
        PaginationParamsDto pagination,
        Guid currentUserId)
    {
        var posts = await _unitOfWork.CommunityPosts
            .GetFeedPostsAsync();

        var totalCount = posts.Count();

        var paginatedPosts = posts
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var feedDtos = paginatedPosts.Select(p => new FeedResponseDto
        {
            CommunityPostId = p.CommunityPostId,
            Content = p.Content,
            ImageUrl = p.ImageUrl,
            LinkUrl = p.LinkUrl,
            CreatedAt = p.CreatedAt,

            AuthorId = p.AuthorUserId,

            AuthorName = p.Author != null
                ? $"{p.Author.FirstName} {p.Author.LastName}"
                : "Unknown User",

            AuthorProfilePicture =
                p.Author?.MentorProfile?.ProfilePictureUrl
                ?? p.Author?.MenteeProfile?.ProfilePictureUrl,

            LikesCount = p.Likes?.Count ?? 0,
            CommentsCount = p.Comments?.Count() ?? 0
        }).ToList();

        var result = new PagedResult<FeedResponseDto>
        {
            Items = feedDtos,
            TotalCount = totalCount,
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };

        return ApiResponse<PagedResult<FeedResponseDto>> .SuccessResponse(result);
    }
}

