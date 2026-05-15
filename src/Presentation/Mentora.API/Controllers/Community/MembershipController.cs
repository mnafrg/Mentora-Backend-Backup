using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Enums;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class MembershipController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    // POST /api/communities/{communityId}/join
    [HttpPost("{communityId:guid}/join")]
    public async Task<IActionResult> JoinCommunity(Guid communityId)
    {
        var userId = User.GetUserId();
        var result = await _membershipService.JoinCommunityAsync(communityId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/communities/{communityId}/leave
    [HttpDelete("{communityId:guid}/leave")]
    public async Task<IActionResult> LeaveCommunity(Guid communityId)
    {
        var userId = User.GetUserId();
        var result = await _membershipService.LeaveCommunityAsync(communityId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/communities/{communityId}/members/{targetUserId}
    [HttpDelete("{communityId:guid}/members/{targetUserId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid communityId, Guid targetUserId)
    {
        var userId = User.GetUserId();
        var result = await _membershipService.RemoveMemberAsync(communityId, targetUserId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PATCH /api/communities/{communityId}/members/{targetUserId}/role
    [HttpPatch("{communityId:guid}/members/{targetUserId:guid}/role")]
    public async Task<IActionResult> UpdateMemberRole(Guid communityId, Guid targetUserId, [FromBody] CommunityRole newRole)
    {
        var userId = User.GetUserId();
        var result = await _membershipService.UpdateMemberRoleAsync(communityId, targetUserId, newRole, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // POST /api/communities/{communityId}/members/{targetUserId}/ban
    [HttpPost("{communityId:guid}/members/{targetUserId:guid}/ban")]
    public async Task<IActionResult> BanMember(Guid communityId, Guid targetUserId)
    {
        var userId = User.GetUserId();
        var result = await _membershipService.BanMemberAsync(communityId, targetUserId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    // GET /api/communities/{communityId}/Admins
    [HttpGet("{communityId:guid}/admins")]
    public async Task<IActionResult> GetAllAdminss(Guid communityId)
    {
        var result = await _membershipService.GetAllAdminsAsync(communityId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    // GET /api/communities/{communityId}/members
    [HttpGet("{communityId:guid}/members")]
    public async Task<IActionResult> GetAllMembers(Guid communityId)
    {
        var result = await _membershipService.GetAllMembersAsync(communityId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
