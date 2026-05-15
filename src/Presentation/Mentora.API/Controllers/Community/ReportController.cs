using Mentora.API.Extensions;
using Mentora.Application.DTOs.Community;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers.Community;

[ApiController]
[Route("api/communities")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly ICommunityReportService _reportService;

    public ReportController(ICommunityReportService reportService)
    {
        _reportService = reportService;
    }

    // POST /api/communities/{communityId}/posts/{postId}/report
    [HttpPost("{communityId:guid}/posts/{postId:guid}/report")]
    public async Task<IActionResult> CreateReport(Guid communityId, Guid postId, [FromBody] CreateReportDto dto)
    {
        var userId = User.GetUserId();
        var result = await _reportService.CreateReportAsync(communityId, postId, dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET /api/communities/posts/{postId}/reports
    [HttpGet("posts/{postId:guid}/reports")]
    public async Task<IActionResult> GetAllReportsByPost(Guid postId)
    {
        var result = await _reportService.GetAllReportsByPostAsync(postId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PATCH /api/communities/posts/reports/{reportId}/status
    [HttpPatch("posts/reports/{reportId:guid}/status")]
    public async Task<IActionResult> UpdateReportStatus(Guid reportId, [FromBody] CommunityReportStatus newStatus)
    {
        var userId = User.GetUserId();
        var result = await _reportService.UpdateReportStatusAsync(reportId, newStatus, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpDelete("posts/reports/{reportId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReport(Guid reportId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _reportService.DeleteReportAsync(reportId, userId);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }
}
