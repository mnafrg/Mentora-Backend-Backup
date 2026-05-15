namespace Mentora.Application.DTOs.Profile;

public class MentorOwnProfileDto
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
    public int YearsOfExperience { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? PastExperience { get; set; }

    public List<EducationDto> Education { get; set; } = new();
    public List<ProfileLinkDTO> Links { get; set; } = new();
}