using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class SaveController : ControllerBase
{
    private readonly ICommunitySaveService _saveService;

    public SaveController(ICommunitySaveService saveService)
    {
        _saveService = saveService;
    }

    // POST /api/communities/posts/{postId}/save
    [HttpPost("posts/{postId:guid}/save")]
    public async Task<IActionResult> ToggleSave(Guid postId)
    {
        var userId = User.GetUserId();
        var result = await _saveService.ToggleSaveAsync(postId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/posts/saved
    [HttpGet("posts/saved")]
    public async Task<IActionResult> GetAllSavedPosts()
    {
        var userId = User.GetUserId();
        var result = await _saveService.GetAllSavedPostsAsync(userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
