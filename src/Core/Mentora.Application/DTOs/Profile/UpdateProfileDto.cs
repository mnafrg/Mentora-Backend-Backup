namespace Mentora.Application.DTOs.Profile;
public class UpdateMenteeProfileRequest
{
    public string? Bio { get; set; }
    public string? CountryCode { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

public class UpdateMentorProfileRequest
{
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? CountryCode { get; set; }
    public string? PastExperience { get; set; }
    public string? ProfilePictureUrl { get; set; }
}