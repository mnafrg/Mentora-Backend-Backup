
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;

namespace Mentora.Application.Services.Community;

public class CommunityService : ICommunityService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommunityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<Guid>> CreateCommunityAsync(
        CreateCommunityDto dto,
        Guid userId)
    {
        var allCommunities = await _unitOfWork.Communities
            .GetAllCommunitiesAsync();

        if (allCommunities.Any(c =>
            c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()))
        {
            return ApiResponse<Guid>
                .ErrorResponse("Community name already exists");
        }

        var community = new Domain.Entities.Community
        {
            CommunityId = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            DomainId = dto.DomainId
        };

        await _unitOfWork.Communities.CreateAsync(community);

        // Auto-add creator as Owner
        var ownerMember = new CommunityMember
        {
            CommunityMemberId = Guid.NewGuid(),
            CommunityId = community.CommunityId,
            UserId = userId,
            Role = CommunityRole.Owner,
            JoinedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityMembers
            .CreateAsync(ownerMember);

        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<Guid>
            .SuccessResponse(
                community.CommunityId,
                "Community created successfully");
    }

    public async Task<ApiResponse<CommunityResponseDto>>
        GetCommunityAsync(Guid communityId, Guid currentUserId)
    {
        var community = await _unitOfWork.Communities
            .GetCommunityByIdAsync(communityId);

        if (community == null)
        {
            return ApiResponse<CommunityResponseDto>
                .ErrorResponse("Community not found");
        }

        var member = community.Members?
            .FirstOrDefault(m => m.UserId == currentUserId);

        var response = new CommunityResponseDto
        {
            CommunityId = community.CommunityId,
            Name = community.Name,
            Description = community.Description,
            CoverImageUrl = community.CoverImageUrl,
            DomainId = community.DomainId,
            CreatedAt = community.CreatedAt,

            MembersCount = community.Members?.Count ?? 0,
            PostsCount = community.Posts?.Count ?? 0,

            CreatedByUserName = community.CreatedByUser != null
                ? $"{community.CreatedByUser.FirstName} {community.CreatedByUser.LastName}"
                : "Unknown User",

            CreatedByUserProfilePicture =
                community.CreatedByUser?.MentorProfile?.ProfilePictureUrl
                ?? community.CreatedByUser?.MenteeProfile?.ProfilePictureUrl,

            // Frontend support fields
            IsMember = member != null,

            CurrentUserRole = member?.Role.ToString(),

            CanManage =
                member?.Role == CommunityRole.Owner ||
                member?.Role == CommunityRole.Admin
        };

        return ApiResponse<CommunityResponseDto>
            .SuccessResponse(response);
    }

    public async Task<ApiResponse<IEnumerable<CommunityResponseDto>>>
        GetAllCommunitiesAsync(Guid currentUserId)
    {
        var communities = await _unitOfWork.Communities
            .GetAllCommunitiesAsync();

        var response = communities.Select(c =>
        {
            var member = c.Members?
                .FirstOrDefault(m => m.UserId == currentUserId);

            return new CommunityResponseDto
            {
                CommunityId = c.CommunityId,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                DomainId = c.DomainId,
                CreatedAt = c.CreatedAt,

                MembersCount = c.Members?.Count ?? 0,
                PostsCount = c.Posts?.Count ?? 0,

                CreatedByUserName = c.CreatedByUser != null
                    ? $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}"
                    : "Unknown User",

                CreatedByUserProfilePicture =
                    c.CreatedByUser?.MentorProfile?.ProfilePictureUrl
                    ?? c.CreatedByUser?.MenteeProfile?.ProfilePictureUrl,

                // Frontend support fields
                IsMember = member != null,

                CurrentUserRole = member?.Role.ToString(),

                CanManage =
                    member?.Role == CommunityRole.Owner ||
                    member?.Role == CommunityRole.Admin
            };
        }).ToList();

        return ApiResponse<IEnumerable<CommunityResponseDto>>
            .SuccessResponse(response);
    }

    public async Task<ApiResponse<IEnumerable<CommunityResponseDto>>>
        GetAllCommunitiesByMemberAsync(Guid userId)
    {
        var communities = await _unitOfWork.Communities
            .GetAllCommunitiesByMemberIdAsync(userId);

        var responses = communities.Select(c =>
        {
            var member = c.Members?
                .FirstOrDefault(m => m.UserId == userId);

            return new CommunityResponseDto
            {
                CommunityId = c.CommunityId,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                DomainId = c.DomainId,
                CreatedAt = c.CreatedAt,

                MembersCount = c.Members?.Count ?? 0,
                PostsCount = c.Posts?.Count ?? 0,

                CreatedByUserName = c.CreatedByUser != null
                    ? $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}"
                    : "Admin",

                CreatedByUserProfilePicture =
                    c.CreatedByUser?.MentorProfile?.ProfilePictureUrl
                    ?? c.CreatedByUser?.MenteeProfile?.ProfilePictureUrl,

                // Frontend support fields
                IsMember = true,

                CurrentUserRole = member?.Role.ToString(),

                CanManage =
                    member?.Role == CommunityRole.Owner ||
                    member?.Role == CommunityRole.Admin
            };
        }).ToList();

        return ApiResponse<IEnumerable<CommunityResponseDto>>
            .SuccessResponse(responses);
    }

    public async Task<ApiResponse<bool>> UpdateCommunityAsync(
        Guid communityId,
        UpdateCommunityDto dto,
        Guid userId)
    {
        var community = await _unitOfWork.Communities
            .GetCommunityByIdAsync(communityId);

        if (community == null)
        {
            return ApiResponse<bool>
                .ErrorResponse("Community not found");
        }

        var member = await _unitOfWork.CommunityMembers
            .GetMemberAsync(communityId, userId);

        if (member == null ||
            (member.Role != CommunityRole.Owner &&
             member.Role != CommunityRole.Admin))
        {
            return ApiResponse<bool>
                .ErrorResponse(
                    "Only Owner or Admin can update the community");
        }

        if (dto.Name != null)
            community.Name = dto.Name;

        if (dto.Description != null)
            community.Description = dto.Description;

        if (dto.CoverImageUrl != null)
            community.CoverImageUrl = dto.CoverImageUrl;

        if (dto.DomainId != 0)
            community.DomainId = dto.DomainId.Value;

        community.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Communities.UpdateAsync(community);

        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>
            .SuccessResponse(
                true,
                "Community updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteCommunityAsync(
        Guid communityId,
        Guid userId)
    {
        var community = await _unitOfWork.Communities
            .GetCommunityByIdAsync(communityId);

        if (community == null)
        {
            return ApiResponse<bool>
                .ErrorResponse("Community not found");
        }

        var member = await _unitOfWork.CommunityMembers
            .GetMemberAsync(communityId, userId);

        if (member == null ||
            member.Role != CommunityRole.Owner)
        {
            return ApiResponse<bool>
                .ErrorResponse(
                    "Only the Owner can delete the community");
        }

        await _unitOfWork.Communities.DeleteAsync(community);

        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>
            .SuccessResponse(
                true,
                "Community deleted successfully");
    }
}

