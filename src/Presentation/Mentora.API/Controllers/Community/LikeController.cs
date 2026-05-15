using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class LikeController : ControllerBase
{
    private readonly ICommunityLikeService _likeService;

    public LikeController(ICommunityLikeService likeService)
    {
        _likeService = likeService;
    }

    // POST /api/communities/posts/{postId}/like
    [HttpPost("posts/{postId:guid}/like")]
    public async Task<IActionResult> ToggleLike(Guid postId)
    {
        var userId = User.GetUserId();
        var result = await _likeService.ToggleLikeAsync(postId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
