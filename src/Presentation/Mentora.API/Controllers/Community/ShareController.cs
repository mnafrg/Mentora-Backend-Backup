using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class ShareController : ControllerBase
{
    private readonly ICommunityShareService _shareService;

    public ShareController(ICommunityShareService shareService)
    {
        _shareService = shareService;
    }

    // POST /api/communities/posts/{postId}/share
    [HttpPost("posts/{postId:guid}/share")]
    public async Task<IActionResult> SharePost(Guid postId)
    {
        var userId = User.GetUserId();
        var result = await _shareService.SharePostAsync(postId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
