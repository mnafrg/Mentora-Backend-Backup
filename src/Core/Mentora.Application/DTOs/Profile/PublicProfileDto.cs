namespace Mentora.Application.DTOs.Profile;

public class PublicProfileDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string Role { get; set; } = null!;
    public string DomainName { get; set; } = null!;

    // Country is shown publicly (just the name, not the code)
    public string? CountryName { get; set; }

    // Education is shown publicly
    public List<EducationDto> Education { get; set; } = new();

    // Social links are shown publicly
    public List<ProfileLinkDTO> Links { get; set; } = new();

    // Mentor-only fields (null for mentees)
    public int? YearsOfExperience { get; set; }
    public decimal? AverageRating { get; set; }
    public int? TotalReviews { get; set; }
    public int? FollowerCount { get; set; }
    public bool? IsVerified { get; set; }
}