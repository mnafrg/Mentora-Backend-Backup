using Mentora.Domain.Enums;

namespace Mentora.Application.DTOs.Community;

public class MemberResponseDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public CommunityRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}
