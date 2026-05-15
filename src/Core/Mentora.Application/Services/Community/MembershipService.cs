using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;

namespace Mentora.Application.Services.Community;

public class MembershipService : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork;

    public MembershipService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> JoinCommunityAsync(Guid communityId, Guid userId)
    {
        var community = await _unitOfWork.Communities.GetCommunityByIdAsync(communityId);
        if (community == null)
            return ApiResponse<bool>.ErrorResponse("Community not found");

        var existingMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (existingMember != null)
        {
            if (existingMember.IsBanned)
                return ApiResponse<bool>.ErrorResponse("You are banned from this community");
            return ApiResponse<bool>.ErrorResponse("You are already a member of this community");
        }

        var member = new CommunityMember
        {
            CommunityMemberId = Guid.NewGuid(),
            CommunityId = communityId,
            UserId = userId,
            Role = CommunityRole.Member,
            JoinedAt = DateTime.UtcNow
        };

        await _unitOfWork.CommunityMembers.CreateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Joined community successfully");
    }

    public async Task<ApiResponse<bool>> LeaveCommunityAsync(Guid communityId, Guid userId)
    {
        var member = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (member == null)
            return ApiResponse<bool>.ErrorResponse("You are not a member of this community");

        if (member.Role == CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Owner cannot leave the community");

        await _unitOfWork.CommunityMembers.DeleteAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Left community successfully");
    }

    public async Task<ApiResponse<bool>> RemoveMemberAsync(Guid communityId, Guid targetUserId, Guid userId)
    {
        var actingMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (actingMember == null || (actingMember.Role != CommunityRole.Owner && actingMember.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("Only Owner or Admin can remove members");

        var targetMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, targetUserId);
        if (targetMember == null)
            return ApiResponse<bool>.ErrorResponse("Target user is not a member");

        if (targetMember.Role == CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Owner cannot be removed");

        await _unitOfWork.CommunityMembers.DeleteAsync(targetMember);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Member removed successfully");
    }

    public async Task<ApiResponse<bool>> UpdateMemberRoleAsync(Guid communityId, Guid targetUserId, CommunityRole newRole, Guid userId)
    {
       
        var actingMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (actingMember == null || actingMember.Role != CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Only Owner can change roles");

        if (newRole == CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Cannot assign Owner role");

        var targetMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, targetUserId);
        if (targetMember == null)
            return ApiResponse<bool>.ErrorResponse("Target user is not a member");

        if (targetMember.Role == CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Cannot change Owner's role");

        targetMember.Role = newRole;
        await _unitOfWork.CommunityMembers.UpdateAsync(targetMember);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Role updated successfully");
    }

    public async Task<ApiResponse<bool>> BanMemberAsync(Guid communityId, Guid targetUserId, Guid userId)
    {
        var actingMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, userId);
        if (actingMember == null || (actingMember.Role != CommunityRole.Owner && actingMember.Role != CommunityRole.Admin))
            return ApiResponse<bool>.ErrorResponse("Only Owner or Admin can ban members");

        var targetMember = await _unitOfWork.CommunityMembers.GetMemberAsync(communityId, targetUserId);
        if (targetMember == null)
            return ApiResponse<bool>.ErrorResponse("Target user is not a member");

        if (targetMember.Role == CommunityRole.Owner)
            return ApiResponse<bool>.ErrorResponse("Owner cannot be banned");

        targetMember.IsBanned = true;
        await _unitOfWork.CommunityMembers.UpdateAsync(targetMember);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Member banned successfully");
    }

    public async Task<ApiResponse<IEnumerable<MemberResponseDto>>> GetAllAdminsAsync(Guid communityId)
    {
        var members = await _unitOfWork.CommunityMembers
            .GetAllMembersByCommunityIdAsync(communityId);

        var admins = members
            .Where(m => m.Role == CommunityRole.Admin)
            .Select(m => new MemberResponseDto
            {
                UserId = m.UserId,
                UserName = $"{m.User.FirstName} {m.User.LastName}",
                ProfilePictureUrl = m.User.MentorProfile?.ProfilePictureUrl
                    ?? m.User.MenteeProfile?.ProfilePictureUrl,
                Role = m.Role,
                JoinedAt = m.JoinedAt
            });

        return ApiResponse<IEnumerable<MemberResponseDto>>.SuccessResponse(admins);
    }
    public async Task<ApiResponse<IEnumerable<MemberResponseDto>>> GetAllMembersAsync(Guid communityId)
    {
        var members = await _unitOfWork.CommunityMembers
            .GetAllMembersByCommunityIdAsync(communityId);

        var response = members
            .Where(m => m.Role == CommunityRole.Member)
            .Select(m => new MemberResponseDto
            {
                UserId = m.UserId,
                UserName = $"{m.User.FirstName} {m.User.LastName}",
                ProfilePictureUrl = m.User.MentorProfile?.ProfilePictureUrl
                    ?? m.User.MenteeProfile?.ProfilePictureUrl,
                Role = m.Role,
                JoinedAt = m.JoinedAt
            });

        return ApiResponse<IEnumerable<MemberResponseDto>>.SuccessResponse(response);
    }
}