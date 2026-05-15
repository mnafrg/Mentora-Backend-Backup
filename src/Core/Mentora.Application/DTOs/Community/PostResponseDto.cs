namespace Mentora.Application.DTOs.Community;

public class PostResponseDto
{
    public Guid CommunityPostId { get; set; }

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LikesCount { get; set; }

    public int CommentsCount { get; set; }

    public int SharesCount { get; set; }

    public Guid AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public string? AuthorProfilePicture { get; set; }

    // Frontend support fields
    public bool IsLiked { get; set; }

    public bool IsSaved { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }
}