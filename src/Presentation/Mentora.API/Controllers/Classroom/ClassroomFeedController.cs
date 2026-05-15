using Mentora.Application.DTOs.Classroom;
using Mentora.Application.Interfaces.Services.Classroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers.Classroom;

[ApiController]
[Route("api/classroom/program/{programId:int}/feed")]
[Authorize]
public class ClassroomFeedController : ControllerBase
{
    private readonly IClassroomFeedService _feedService;

    public ClassroomFeedController(IClassroomFeedService feedService)
    {
        _feedService = feedService;
    }

    // ── Feed ─────────────────────────────────────────────────────────────────

    /// GET /api/classroom/program/{programId}/feed?page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetFeed(
        int programId,
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 10)
    {
        pageSize = Math.Clamp(pageSize, 1, 50);
        var result = await _feedService.GetFeedAsync(programId, GetRequesterId(), page, pageSize);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // ── Posts ─────────────────────────────────────────────────────────────────

    /// POST /api/classroom/program/{programId}/feed/posts
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost(
        int programId, [FromBody] CreatePostRequest dto)
    {
        var result = await _feedService.CreatePostAsync(programId, GetRequesterId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/classroom/program/{programId}/feed/posts/{postId}
    [HttpPatch("posts/{postId:int}")]
    public async Task<IActionResult> UpdatePost(
        int programId, int postId, [FromBody] UpdatePostRequest dto)
    {
        var result = await _feedService.UpdatePostAsync(postId, GetRequesterId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// DELETE /api/classroom/program/{programId}/feed/posts/{postId}
    [HttpDelete("posts/{postId:int}")]
    public async Task<IActionResult> DeletePost(int programId, int postId)
    {
        var result = await _feedService.DeletePostAsync(postId, GetRequesterId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/classroom/program/{programId}/feed/posts/{postId}/pin
    /// Mentor only.
    [HttpPatch("posts/{postId:int}/pin")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> TogglePin(int programId, int postId)
    {
        var result = await _feedService.TogglePinPostAsync(postId, GetMentorProfileId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// POST /api/classroom/program/{programId}/feed/posts/{postId}/like
    [HttpPost("posts/{postId:int}/like")]
    public async Task<IActionResult> ToggleLikePost(int programId, int postId)
    {
        var result = await _feedService.ToggleLikePostAsync(postId, GetRequesterId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Comments ──────────────────────────────────────────────────────────────

    /// GET /api/classroom/program/{programId}/feed/posts/{postId}/comments
    [HttpGet("posts/{postId:int}/comments")]
    public async Task<IActionResult> GetComments(int programId, int postId)
    {
        var result = await _feedService.GetCommentsAsync(postId, GetRequesterId());
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// POST /api/classroom/program/{programId}/feed/posts/{postId}/comments
    [HttpPost("posts/{postId:int}/comments")]
    public async Task<IActionResult> CreateComment(
        int programId, int postId, [FromBody] CreateCommentRequest dto)
    {
        var result = await _feedService.CreateCommentAsync(postId, GetRequesterId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/classroom/program/{programId}/feed/comments/{commentId}
    [HttpPatch("comments/{commentId:int}")]
    public async Task<IActionResult> UpdateComment(
        int programId, int commentId, [FromBody] UpdateCommentRequest dto)
    {
        var result = await _feedService.UpdateCommentAsync(commentId, GetRequesterId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// DELETE /api/classroom/program/{programId}/feed/comments/{commentId}
    [HttpDelete("comments/{commentId:int}")]
    public async Task<IActionResult> DeleteComment(int programId, int commentId)
    {
        var result = await _feedService.DeleteCommentAsync(commentId, GetRequesterId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// POST /api/classroom/program/{programId}/feed/comments/{commentId}/like
    [HttpPost("comments/{commentId:int}/like")]
    public async Task<IActionResult> ToggleLikeComment(int programId, int commentId)
    {
        var result = await _feedService.ToggleLikeCommentAsync(commentId, GetRequesterId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private Guid GetRequesterId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private Guid GetMentorProfileId() =>
        Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
}