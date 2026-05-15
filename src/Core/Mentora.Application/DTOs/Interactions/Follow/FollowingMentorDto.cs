namespace Mentora.Application.DTOs.Interactions.Follow;

public class FollowingMentorDto
{
    public Guid MentorId { get; set; }
    public string FullName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string DomainName { get; set; } = null!;
    public decimal? AverageRating { get; set; }
    public int? FollowerCount { get; set; }
    public DateTime FollowedAt { get; set; }
}