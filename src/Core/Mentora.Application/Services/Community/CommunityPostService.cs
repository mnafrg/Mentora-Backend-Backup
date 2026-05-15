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

    public async Task<ApiResponse<Guid>> CreatePostAsync(Guid communityId, CreatePostDto dto, Guid userId)
    {
        var member = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (member == null)
            return ApiResponse<Guid>.ErrorResponse("Only members can create posts");

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

       

        return ApiResponse<Guid>.SuccessResponse(post.CommunityPostId, "Post created successfully");
    }

    public async Task<ApiResponse<PostResponseDto>> GetPostAsync(Guid postId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostWithDetailsAsync(postId);

        if (post == null)
            return ApiResponse<PostResponseDto>.ErrorResponse("Post not found");

        var response = new PostResponseDto
        {
            CommunityPostId = post.CommunityPostId,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt,
            AuthorName = $"{post.Author.FirstName} {post.Author.LastName}",
            LikesCount = post.Likes?.Count ?? 0,
            CommentsCount = post.Comments?.Count() ?? 0,


            Comments = post.Comments.Select(c => new CommentResponseDto
            {
                CommunityCommentId = c.CommunityCommentId,
                Content = c.CommentText,
                CreatedAt = c.CreatedAt,
                AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                AuthorProfilePicture = c.User.MentorProfile?.ProfilePictureUrl ?? c.User.MenteeProfile?.ProfilePictureUrl
            }).ToList()
        };

        return ApiResponse<PostResponseDto>.SuccessResponse(response);
    }
    public async Task<ApiResponse<IEnumerable<PostResponseDto>>> GetAllPostsByCommunityAsync(Guid communityId)
    {
        var posts = await _unitOfWork.CommunityPosts.GetAllPostsByCommunityIdAsync(communityId);

     
        if (posts == null || !posts.Any())
            return ApiResponse<IEnumerable<PostResponseDto>>.SuccessResponse(new List<PostResponseDto>(), "No posts found for this user");

        var response = posts.Select(p => new PostResponseDto
        {
            CommunityPostId = p.CommunityPostId,
            Content = p.Content,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            AuthorId=p.AuthorUserId,
            AuthorName = $"{p.Author.FirstName} {p.Author.LastName}",
            AuthorProfilePicture = p.Author.MentorProfile?.ProfilePictureUrl ?? p.Author.MenteeProfile?.ProfilePictureUrl,

            Comments = p.Comments.Select(c => new CommentResponseDto
            {
                CommunityCommentId = c.CommunityCommentId,
                Content = c.CommentText,
                CreatedAt = c.CreatedAt,
                AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                AuthorProfilePicture = c.User.MentorProfile?.ProfilePictureUrl ?? c.User.MenteeProfile?.ProfilePictureUrl
            }).ToList(),

            LikesCount = p.Likes?.Count ?? 0,
            CommentsCount = p.Comments?.Count() ?? 0
        }).ToList();

        return ApiResponse<IEnumerable<PostResponseDto>>.SuccessResponse(response);
    }


    public async Task<ApiResponse<PostResponseDto>> UpdatePostAsync(Guid postId, UpdatePostDto dto, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostWithDetailsAsync(postId);

        if (post == null)
            return ApiResponse<PostResponseDto>.ErrorResponse("Post not found");

      
        if (post.AuthorUserId != userId)
            return ApiResponse<PostResponseDto>.ErrorResponse("Only the post author can update");


        if (dto.Content != null) 
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return ApiResponse<PostResponseDto>.ErrorResponse("Content cannot be empty");
            }
            post.Content = dto.Content;
        }
        if (dto.ImageUrl != null) post.ImageUrl = dto.ImageUrl;
        if (dto.LinkUrl != null) post.LinkUrl = dto.LinkUrl;
        post.UpdatedAt = DateTime.UtcNow;


        await _unitOfWork.CommunityPosts.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

       
        var response = new PostResponseDto
        {
            CommunityPostId = post.CommunityPostId,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            LinkUrl = post.LinkUrl,
            CreatedAt = post.CreatedAt,
            AuthorId=post.AuthorUserId,
            AuthorName = $"{post.Author.FirstName} {post.Author.LastName}",
            AuthorProfilePicture = post.Author.MentorProfile?.ProfilePictureUrl ?? post.Author.MenteeProfile?.ProfilePictureUrl,

          
            Comments = post.Comments.Select(c => new CommentResponseDto
            {
                CommunityCommentId = c.CommunityCommentId,
                Content = c.CommentText,
                CreatedAt = c.CreatedAt,
                AuthorName = $"{c.User.FirstName} {c.User.LastName}",
                AuthorProfilePicture = c.User.MentorProfile?.ProfilePictureUrl ?? c.User.MenteeProfile?.ProfilePictureUrl
            }).ToList(),

            LikesCount = post.Likes?.Count ?? 0,
            CommentsCount = post.Comments?.Count() ?? 0
        };

        return ApiResponse<PostResponseDto>.SuccessResponse(response, "Post updated successfully");
    }
    public async Task<ApiResponse<bool>> DeletePostAsync(Guid postId, Guid userId)
    {
        var post = await _unitOfWork.CommunityPosts.GetPostByIdAsync(postId);
        if (post == null)
            return ApiResponse<bool>.ErrorResponse("Post not found");

        if (post.AuthorUserId == userId)
        {
          
            await _unitOfWork.CommunityPosts.DeleteAsync(post);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Post deleted successfully");
        }

        // Check if user is Admin or Owner of the community
        var member = await _unitOfWork.CommunityMembers.GetMemberAsync(post.CommunityId, userId);
        if (member == null || (member.Role != CommunityRole.Owner && member.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("You do not have permission to delete this post");

      
        await _unitOfWork.CommunityPosts.DeleteAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Post deleted successfully");
    }

    public async Task<ApiResponse<IEnumerable<FeedResponseDto>>> GetFeedAsync()
    {
        var posts = await _unitOfWork.CommunityPosts.GetFeedPostsAsync();

        var response = posts.Select(p => new FeedResponseDto
        {
            CommunityPostId = p.CommunityPostId,
            Content = p.Content,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            AuthorName = $"{p.Author.FirstName} {p.Author.LastName}",
            AuthorProfilePicture = p.Author.MentorProfile?.ProfilePictureUrl ?? p.Author.MenteeProfile?.ProfilePictureUrl,
            LikesCount = p.Likes?.Count ?? 0,
            CommentsCount = p.Comments?.Count() ?? 0
        }).ToList();

        return ApiResponse<IEnumerable<FeedResponseDto>>.SuccessResponse(response);
    }
 
}
