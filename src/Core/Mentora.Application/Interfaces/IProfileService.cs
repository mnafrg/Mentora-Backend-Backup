using Mentora.Application.DTOs.Profile;
using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces;

public interface IProfileService
{
    // Own profile 
    Task<ApiResponse<MenteeOwnProfileDto>> GetMenteeOwnProfileAsync(Guid userId);
    Task<ApiResponse<MentorOwnProfileDto>> GetMentorOwnProfileAsync(Guid userId);

    // Public profile (for viewing by others)
    Task<ApiResponse<PublicProfileDto>> GetPublicProfileAsync(Guid targetUserId);

    // Basic profile updates
    Task<ApiResponse<bool>> UpdateMenteeProfileAsync(Guid userId, UpdateMenteeProfileRequest request);
    Task<ApiResponse<bool>> UpdateMentorProfileAsync(Guid userId, UpdateMentorProfileRequest request);

    // Education CRUD
    Task<ApiResponse<EducationDto>> AddEducationAsync(Guid userId, AddEducationRequest request);
    Task<ApiResponse<EducationDto>> UpdateEducationAsync(Guid userId, Guid educationId, UpdateEducationRequest request);
    Task<ApiResponse<bool>> DeleteEducationAsync(Guid userId, Guid educationId);

    // Profile links CRUD
    Task<ApiResponse<ProfileLinkDTO>> AddLinkAsync(Guid userId, AddLinkRequest request);
    Task<ApiResponse<ProfileLinkDTO>> UpdateLinkAsync(Guid userId, Guid linkId, UpdateLinkRequest request);
    Task<ApiResponse<bool>> DeleteLinkAsync(Guid userId, Guid linkId);


}