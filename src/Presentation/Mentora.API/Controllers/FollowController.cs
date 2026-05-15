using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    // POST /api/follow/{mentorId}
    // Follow a mentor. Both mentees and mentors can follow mentors.
    [HttpPost("{mentorId:guid}")]
    public async Task<IActionResult> Follow(Guid mentorId)
    {
        var followerId = User.GetUserId();
        var result     = await _followService.FollowAsync(followerId, mentorId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/follow/{mentorId}
    // Unfollow a mentor.
    [HttpDelete("{mentorId:guid}")]
    public async Task<IActionResult> Unfollow(Guid mentorId)
    {
        var followerId = User.GetUserId();
        var result     = await _followService.UnfollowAsync(followerId, mentorId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/follow/me
    // Returns the list of mentors the authenticated user is following.
    [HttpGet("me")]
    public async Task<IActionResult> GetFollowing()
    {
        var userId = User.GetUserId();
        var result = await _followService.GetFollowingAsync(userId);
        return Ok(result);
    }

    // GET /api/follow/{mentorId}/count
    // Returns the follower count for a mentor. Public (no auth required).
    [HttpGet("{mentorId:guid}/count")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFollowerCount(Guid mentorId)
    {
        var result = await _followService.GetFollowerCountAsync(mentorId);
        return Ok(result);
    }
}