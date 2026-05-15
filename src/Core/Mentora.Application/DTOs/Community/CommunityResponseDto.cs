namespace Mentora.Application.DTOs.Community;


public class CommunityResponseDto
{
    public Guid CommunityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int DomainId { get; set; }
    public int MembersCount { get; set; }
    public int PostsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedByUserName { get; set; } = null!;
    public string? CreatedByUserProfilePicture { get; set; }
}
