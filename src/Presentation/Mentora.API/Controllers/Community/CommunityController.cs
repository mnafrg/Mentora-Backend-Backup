using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    // POST /api/communities
    [HttpPost("create")]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityDto dto)
    {
        var userId = User.GetUserId();
        var result = await _communityService.CreateCommunityAsync(dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/{communityId}
    [HttpGet("{communityId:guid}")]
    public async Task<IActionResult> GetCommunity(Guid communityId)
    {
        var result = await _communityService.GetCommunityAsync(communityId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCommunities()
    {
        var userId = User.GetUserId();
        var result = await _communityService.GetAllCommunitiesAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/myCommunities
    [HttpGet("my")]
    public async Task<IActionResult> GetAllCommunitiesByMemberAsync()
    {
        var userId = User.GetUserId();
        var result = await _communityService.GetAllCommunitiesByMemberAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }



    // PATCH /api/communities/{communityId}
    [HttpPatch("{communityId:guid}")]
    public async Task<IActionResult> UpdateCommunity(Guid communityId, [FromBody] UpdateCommunityDto dto)
    {
        var userId = User.GetUserId();
        var result = await _communityService.UpdateCommunityAsync(communityId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/communities/{communityId}
    [HttpDelete("{communityId:guid}")]
    public async Task<IActionResult> DeleteCommunity(Guid communityId)
    {
        var userId = User.GetUserId();
        var result = await _communityService.DeleteCommunityAsync(communityId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/{communityId}/link
    [HttpGet("{communityId:guid}/link")]
    public IActionResult GetCommunityLink(Guid communityId)
    {
        var link = $"https://mentora.com/community/{communityId}";
        var result = ApiResponse<string>.SuccessResponse(link, "Community link generated");
        return Ok(result);
    }
}
