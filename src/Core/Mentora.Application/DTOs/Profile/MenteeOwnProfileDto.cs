namespace Mentora.Application.DTOs.Profile;


public class MenteeOwnProfileDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    // Email is included only in the own-profile response, never public
    public string Email { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }

    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }

    public string DomainName { get; set; } = null!;
    public string CurrentLevel { get; set; } = null!;

    // Legacy single-value kept for backward compat; new field below.
    public string EducationStatus { get; set; } = null!;

    /// Rich education entries (university, faculty, graduation year)
    public List<EducationDto> Education { get; set; } = new();

    public List<ProfileLinkDTO> Links { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
