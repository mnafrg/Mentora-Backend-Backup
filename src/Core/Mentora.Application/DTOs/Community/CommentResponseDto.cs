namespace Mentora.Application.DTOs.Community;

public class CommentResponseDto
{
    public Guid CommunityCommentId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = null!;
    public string? AuthorProfilePicture { get; set; }
}
