using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly ICommunityPostService _postService;

    public PostController(ICommunityPostService postService)
    {
        _postService = postService;
    }

    // POST /api/communities/{communityId}/posts
    [HttpPost("{communityId:guid}/posts")]
    public async Task<IActionResult> CreatePost(Guid communityId, [FromBody] CreatePostDto dto)
    {
        var userId = User.GetUserId();
        var result = await _postService.CreatePostAsync(communityId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/{communityId}/posts
    [HttpGet("{communityId:guid}/posts")]
    public async Task<IActionResult> GetAllPostsByCommunity(Guid communityId)
    {
        var result = await _postService.GetAllPostsByCommunityAsync(communityId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/posts/feed
    [HttpGet("posts/feed")]
    public async Task<IActionResult> GetFeed()
    {
        var result = await _postService.GetFeedAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/posts/{postId}
    [HttpGet("posts/{postId:guid}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var result = await _postService.GetPostAsync(postId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PATCH /api/communities/posts/{postId}
    [HttpPatch("posts/{postId:guid}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromBody] UpdatePostDto dto)
    {
        var userId = User.GetUserId();
        var result = await _postService.UpdatePostAsync(postId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/communities/posts/{postId}
    [HttpDelete("posts/{postId:guid}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        var userId = User.GetUserId();
        var result = await _postService.DeletePostAsync(postId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
