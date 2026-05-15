namespace Mentora.Application.DTOs.Interactions.Follow;

public class FollowerDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Role { get; set; } = null!; // "Mentor" or "Mentee"
    public string DomainName { get; set; } = null!;
    public DateTime FollowedAt { get; set; }

}