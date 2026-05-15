namespace Mentora.Application.DTOs.Classroom;

// ── Requests ────────────────────────────────────────────────────────────────

public class CreatePostRequest
{
    public string Content { get; set; } = null!;
}

public class UpdatePostRequest
{
    public string Content { get; set; } = null!;
}

public class CreateCommentRequest
{
    public string Content { get; set; } = null!;

    /// Null for a top-level comment; set to a comment ID to reply.
    public int? ParentCommentId { get; set; }
}

public class UpdateCommentRequest
{
    public string Content { get; set; } = null!;
}

// ── Responses ────────────────────────────────────────────────────────────────

public class AuthorDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Role { get; set; } = null!;
}

public class CommentDto
{
    public int CommentId { get; set; }
    public AuthorDto Author { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int LikesCount { get; set; }
    public bool LikedByMe { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    /// Populated only for top-level comments.
    public List<CommentDto> Replies { get; set; } = new();
}

public class PostDto
{
    public int PostId { get; set; }
    public AuthorDto Author { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsPinned { get; set; }
    public int LikesCount { get; set; }
    public bool LikedByMe { get; set; }
    public int CommentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    /// Latest 2 comments shown in feed preview.
    public List<CommentDto> LatestComments { get; set; } = new();
}

public class PostFeedResponse
{
    public List<PostDto> Posts { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}