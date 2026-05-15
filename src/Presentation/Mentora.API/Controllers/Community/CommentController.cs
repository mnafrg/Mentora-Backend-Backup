using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.API.Extensions;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommunityCommentService _commentService;

    public CommentController(ICommunityCommentService commentService)
    {
        _commentService = commentService;
    }

    // POST /api/communities/posts/{postId}/comments
    [HttpPost("posts/{postId:guid}/comments")]
    public async Task<IActionResult> CreateComment(Guid postId, [FromBody] CreateCommentDto dto)
    {
        var userId = User.GetUserId();
        var result = await _commentService.CreateCommentAsync(postId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/posts/{postId}/comments
    [HttpGet("posts/{postId:guid}/comments")]
    public async Task<IActionResult> GetAllComments(Guid postId)
    {
        var result = await _commentService.GetAllCommentsAsync(postId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PATCH /api/communities/posts/comments/{commentId}
    [HttpPatch("posts/comments/{commentId:guid}")]
    public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentDto dto)
    {
        var userId = User.GetUserId();
        var result = await _commentService.UpdateCommentAsync(commentId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/communities/posts/comments/{commentId}
    [HttpDelete("posts/comments/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = User.GetUserId();
        var result = await _commentService.DeleteCommentAsync(commentId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
