using Microsoft.Extensions.Logging;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Profile;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Mentora.Domain.Entities.Profiles;
namespace Mentora.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(IUnitOfWork unitOfWork, ILogger<ProfileService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<MenteeOwnProfileDto>> GetMenteeOwnProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<MenteeOwnProfileDto>.ErrorResponse("User not found");

        var profile = await _unitOfWork.MenteeProfiles.GetByUserIdAsync(userId);
        if (profile == null)
            return ApiResponse<MenteeOwnProfileDto>.ErrorResponse("Mentee profile not found");

        var links = await _unitOfWork.ProfileLinks.GetByUserIdAsync(userId);
        var educations = await _unitOfWork.UserEducations.GetByUserIdAsync(userId);
        var achievements = await _unitOfWork.Achievements.GetByUserIdAsync(userId);

        var dto = new MenteeOwnProfileDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePictureUrl = profile.ProfilePictureUrl,
            Bio = profile.Bio,
            CountryCode = profile.CountryCode,
            CountryName = profile.Country?.CountryName,
            DomainName = profile.Domain?.Name ?? string.Empty,
            CurrentLevel = profile.CurrentLevel.ToString(),
            EducationStatus = profile.EducationStatus.ToString(),
            CreatedAt = user.CreatedAt,
            
            Links = links
                .Select(MapLinkToDto)
                .OrderBy(l => l.DisplayOrder)
                .ToList(),

            Education = educations
                .Select(MapEducationToDto)
                .OrderBy(e => e.DisplayOrder)
                .ToList()
        };
        return ApiResponse<MenteeOwnProfileDto>.SuccessResponse(dto);
    }
    public async Task<ApiResponse<MentorOwnProfileDto>> GetMentorOwnProfileAsync(Guid userId)
    {
        
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<MentorOwnProfileDto>.ErrorResponse("User not found");

        var profile = await _unitOfWork.MentorProfiles.GetByUserIdAsync(userId);
        if (profile == null)
            return ApiResponse<MentorOwnProfileDto>.ErrorResponse("Mentor profile not found");

        var links = await _unitOfWork.ProfileLinks.GetByUserIdAsync(userId);
        var educations = await _unitOfWork.UserEducations.GetByUserIdAsync(userId);
        var achievements = await _unitOfWork.Achievements.GetByUserIdAsync(userId);

        var dto = new MentorOwnProfileDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePictureUrl = profile.ProfilePictureUrl,
            Bio = profile.Bio,
            CountryCode = profile.CountryCode,
            CountryName = profile.Country?.CountryName,
            DomainName = profile.Domain?.Name ?? string.Empty,
            YearsOfExperience = profile.YearsOfExperience,
            LinkedInUrl = profile.LinkedInUrl,
            PastExperience = profile.PastExperience,

            Links = links
                .Select(MapLinkToDto)
                .OrderBy(l => l.DisplayOrder)
                .ToList(),

            Education = educations
                .Select(MapEducationToDto)
                .OrderBy(e => e.DisplayOrder)
                .ToList()
        };
        return ApiResponse<MentorOwnProfileDto>.SuccessResponse(dto);

    }

    // --- Public Profile ---
    
    public async Task<ApiResponse<PublicProfileDto>> GetPublicProfileAsync(
        Guid targetUserId)
    {
        var targetUser = await _unitOfWork.Users.GetByIdAsync(targetUserId);
        if (targetUser == null)
            return ApiResponse<PublicProfileDto>.ErrorResponse("User not found");
        
        var links = await _unitOfWork.ProfileLinks.GetByUserIdAsync(targetUserId);
        var educations = await _unitOfWork.UserEducations.GetByUserIdAsync(targetUserId);
        
        var dto = new PublicProfileDto
        {
            UserId = targetUser.UserId,
            FirstName = targetUser.FirstName,
            LastName = targetUser.LastName,
            Role = targetUser.Role.ToString(),

            Education = educations.Select(MapEducationToDto).ToList(),
            Links     = links.Select(MapLinkToDto).ToList(),

        };
        if (targetUser.Role == UserRole.Mentor)
        {
            var mentorProfile = await _unitOfWork.MentorProfiles.GetByUserIdAsync(targetUserId);
            if (mentorProfile != null)
            {
                dto.ProfilePictureUrl = mentorProfile.ProfilePictureUrl;
                dto.Bio = mentorProfile.Bio;
                dto.CountryName = mentorProfile.Country?.CountryName;
                dto.DomainName = mentorProfile.Domain?.Name ?? string.Empty;
                dto.YearsOfExperience = mentorProfile.YearsOfExperience;

                // Future fields to consider adding:
                // dto.AverageRating = mentorProfile.AverageRating;
                // dto.TotalReviews = mentorProfile.TotalReviews;
                // dto.FollowerCount = mentorProfile.FollowerCount;
                dto.IsVerified = mentorProfile.IsVerified;
            }
        }
        else if (targetUser.Role == UserRole.Mentee)
        {
            var menteeProfile = await _unitOfWork.MenteeProfiles.GetByUserIdAsync(targetUserId);
            if (menteeProfile != null)
            {
                dto.ProfilePictureUrl = menteeProfile.ProfilePictureUrl;
                dto.Bio = menteeProfile.Bio;
                dto.CountryName = menteeProfile.Country?.CountryName;
                dto.DomainName = menteeProfile.Domain?.Name ?? string.Empty;
            }
        }
        return ApiResponse<PublicProfileDto>.SuccessResponse(dto);
    }

    // --- Profile  Updates ---
    public async Task<ApiResponse<bool>> UpdateMenteeProfileAsync(
        Guid userId, UpdateMenteeProfileRequest request)
    {
        var profile = await _unitOfWork.MenteeProfiles.GetByUserIdAsync(userId);
        if (profile == null)
            return ApiResponse<bool>.ErrorResponse("Mentee profile not found");

        if (request.Bio != null) profile.Bio = request.Bio.Trim();
        if (request.CountryCode!= null) profile.CountryCode = request.CountryCode.ToUpper();
        if (request.ProfilePictureUrl != null) profile.ProfilePictureUrl = request.ProfilePictureUrl;

        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Profile updated successfully");
    }

    public async Task<ApiResponse<bool>> UpdateMentorProfileAsync(
        Guid userId, UpdateMentorProfileRequest request)
    {
        var profile = await _unitOfWork.MentorProfiles.GetByUserIdAsync(userId);
        if (profile == null)
            return ApiResponse<bool>.ErrorResponse("Mentor profile not found");

        if (request.Bio != null) profile.Bio = request.Bio.Trim();
        if (request.LinkedInUrl != null) profile.LinkedInUrl = request.LinkedInUrl.Trim();
        if (request.CountryCode != null) profile.CountryCode = request.CountryCode.ToUpper();
        if (request.PastExperience != null) profile.PastExperience = request.PastExperience.Trim();
        if (request.ProfilePictureUrl != null) profile.ProfilePictureUrl = request.ProfilePictureUrl;
       
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Profile updated successfully");
    }   

    // --- Education CRUD ---
    public async Task<ApiResponse<EducationDto>> AddEducationAsync(
        Guid userId, AddEducationRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse<EducationDto>.ErrorResponse("User not found");

       var (valid, error) = ValidateEducationRequest(
            request.Institution, request.StartYear, request.GraduationYear);
        if (!valid) return ApiResponse<EducationDto>.ErrorResponse(error!);

        var entry = new UserEducation
        {
            EducationId    = Guid.NewGuid(),
            UserId         = userId,
            Institution    = request.Institution.Trim(),
            Faculty        = request.Faculty?.Trim(),
            Degree         = request.Degree?.Trim(),
            StartYear      = request.StartYear,
            GraduationYear = request.GraduationYear,
            DisplayOrder   = request.DisplayOrder,
        };

        await _unitOfWork.UserEducations.CreateAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<EducationDto>.SuccessResponse(
            MapEducationToDto(entry), "Education entry added");

    }

    
    public async Task<ApiResponse<EducationDto>> UpdateEducationAsync(
        Guid userId, Guid educationId, UpdateEducationRequest request)
    {
        var entry = await _unitOfWork.UserEducations.GetByIdAsync(educationId);
        if (entry == null)
            return ApiResponse<EducationDto>.ErrorResponse("Education entry not found");
        if (entry.UserId != userId)
            return ApiResponse<EducationDto>.ErrorResponse("Not authorized");

        var (valid, error) = ValidateEducationRequest(
            request.Institution, request.StartYear, request.GraduationYear);
        if (!valid) return ApiResponse<EducationDto>.ErrorResponse(error!);

        entry.Institution    = request.Institution.Trim();
        entry.Faculty        = request.Faculty?.Trim();
        entry.Degree         = request.Degree?.Trim();
        entry.StartYear      = request.StartYear;
        entry.GraduationYear = request.GraduationYear;
        entry.DisplayOrder   = request.DisplayOrder;

        await _unitOfWork.UserEducations.UpdateAsync(entry);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<EducationDto>.SuccessResponse(
            MapEducationToDto(entry), "Education entry updated");
    }

    public async Task<ApiResponse<bool>> DeleteEducationAsync(Guid userId, Guid educationId)
    {
        var entry = await _unitOfWork.UserEducations.GetByIdAsync(educationId);
        if (entry == null)
            return ApiResponse<bool>.ErrorResponse("Education entry not found");
        if (entry.UserId != userId)
            return ApiResponse<bool>.ErrorResponse("Not authorized");

        await _unitOfWork.UserEducations.DeleteAsync(educationId);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Education entry deleted");
    }
  
    // --- Profile Links CRUD ---
    
    public async Task<ApiResponse<ProfileLinkDTO>> AddLinkAsync(
        Guid userId, AddLinkRequest request)
    {
        if (!IsValidUrl(request.URL))
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Invalid URL. Must start with http:// or https://");

        if (string.IsNullOrWhiteSpace(request.Label))
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Label is required");

        var link = new ProfileLink
        {
            LinkId       = Guid.NewGuid(),
            UserId       = userId,
            Label        = request.Label.Trim(),
            URL          = request.URL.Trim(),
            DisplayOrder = request.DisplayOrder,
            CreatedAt    = DateTime.UtcNow
        };

        await _unitOfWork.ProfileLinks.CreateAsync(link);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<ProfileLinkDTO>.SuccessResponse(
            MapLinkToDto(link), "Link added");
    }

    public async Task<ApiResponse<ProfileLinkDTO>> UpdateLinkAsync(
        Guid userId, Guid linkId, UpdateLinkRequest request)
    {
        var link = await _unitOfWork.ProfileLinks.GetByIdAsync(linkId);
        if (link == null)
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Link not found");
        if (link.UserId != userId)
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Not authorized");

        if (!IsValidUrl(request.URL))
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Invalid URL. Must start with http:// or https://");

        if (string.IsNullOrWhiteSpace(request.Label))
            return ApiResponse<ProfileLinkDTO>.ErrorResponse("Label is required");

        link.Label        = request.Label.Trim();
        link.URL          = request.URL.Trim();
        link.DisplayOrder = request.DisplayOrder;

        await _unitOfWork.ProfileLinks.UpdateAsync(link);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<ProfileLinkDTO>.SuccessResponse(
            MapLinkToDto(link), "Link updated");
    }

    public async Task<ApiResponse<bool>> DeleteLinkAsync(Guid userId, Guid linkId)
    {
        var link = await _unitOfWork.ProfileLinks.GetByIdAsync(linkId);
        if (link == null)
            return ApiResponse<bool>.ErrorResponse("Link not found");
        if (link.UserId != userId)
            return ApiResponse<bool>.ErrorResponse("Not authorized");

        await _unitOfWork.ProfileLinks.DeleteAsync(linkId);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Link deleted");
    }

    //--- Helper mapping methods ---
    private static EducationDto MapEducationToDto(UserEducation ue) => new()
    {
        EducationId = ue.EducationId,
        Institution = ue.Institution,
        Faculty = ue.Faculty,
        Degree = ue.Degree,
        StartYear = ue.StartYear,
        GraduationYear = ue.GraduationYear,
        DisplayOrder = ue.DisplayOrder ?? 0
    };

    private static ProfileLinkDTO MapLinkToDto(ProfileLink link) => new()
    {
       LinkId = link.LinkId,
       Label = link.Label,
       URL = link.URL,
       DisplayOrder = link.DisplayOrder
        
    };
    
    private static bool IsValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private static (bool valid, string? error) ValidateEducationRequest(
        string institution, int? startYear, int? graduationYear)
    {
        if (string.IsNullOrWhiteSpace(institution))
            return (false, "Institution name is required");

        var currentYear = DateTime.UtcNow.Year;

        if (startYear.HasValue && (startYear < 1900 || startYear > currentYear + 1))
            return (false, $"Start year must be between 1900 and {currentYear + 1}");

        if (graduationYear.HasValue && (graduationYear < 1900 || graduationYear > currentYear + 10))
            return (false, $"Graduation year must be between 1900 and {currentYear + 10}");

        if (startYear.HasValue && graduationYear.HasValue && startYear > graduationYear)
            return (false, "Start year cannot be after graduation year");

        return (true, null);
    }
}