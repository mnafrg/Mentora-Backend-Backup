using Mentora.Application.DTOs.Classroom;
using Mentora.Application.Interfaces.Services.Classroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;
    private readonly ISessionService _sessionService;

    public ClassroomController(IClassroomService classroomService, ISessionService sessionService)
    {
        _classroomService = classroomService;
        _sessionService = sessionService;
    }

    // ── Classroom ─────────────────────────────────────────────────────────────


    /// GET /api/classroom/program/{programId}
    /// Returns classroom details. Accessible by the owning mentor and enrolled mentees.
    [HttpGet("program/{programId:int}")]
    public async Task<IActionResult> GetClassroom(int programId)
    {
        var requesterId = GetRequesterId();
        var result      = await _classroomService.GetClassroomByProgramIdAsync(programId, requesterId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // ── Sessions – Mentor ────────────────────────────────────────────────────

    /// POST /api/classroom/program/{programId}/sessions
    /// Mentor creates a new session.
    [HttpPost("program/{programId:int}/sessions")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateSession(int programId, [FromBody] CreateSessionRequest dto)
    {
        var mentorProfileId = GetMentorProfileId();
        var result          = await _sessionService.CreateSessionAsync(programId, mentorProfileId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/classroom/sessions/{sessionId}
    /// Mentor updates a session.
    [HttpPatch("sessions/{sessionId:int}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdateSession(int sessionId, [FromBody] UpdateSessionRequest dto)
    {
        var mentorProfileId = GetMentorProfileId();
        var result          = await _sessionService.UpdateSessionAsync(sessionId, mentorProfileId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/classroom/sessions/{sessionId}/cancel
    /// Mentor cancels a session.
    [HttpPatch("sessions/{sessionId:int}/cancel")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CancelSession(int sessionId)
    {
        var mentorProfileId = GetMentorProfileId();
        var result          = await _sessionService.CancelSessionAsync(sessionId, mentorProfileId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Sessions – Shared ────────────────────────────────────────────────────

    /// GET /api/classroom/program/{programId}/sessions
    /// All future (upcoming/live) sessions. Accessible by mentor and enrolled mentees.
    [HttpGet("program/{programId:int}/sessions")]
    public async Task<IActionResult> GetFutureSessions(int programId)
    {
        var requesterId = GetRequesterId();
        var result      = await _sessionService.GetFutureSessionsAsync(programId, requesterId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// GET /api/classroom/program/{programId}/sessions/upcoming
    /// The single next session ("join now" card). Accessible by mentor and enrolled mentees. 
    [HttpGet("program/{programId:int}/sessions/upcoming")]
    public async Task<IActionResult> GetNextUpcomingSession(int programId)
    {
        var requesterId = GetRequesterId();
        var result      = await _sessionService.GetNextUpcomingSessionAsync(programId, requesterId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private Guid GetRequesterId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private Guid GetMentorProfileId() =>
        Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
}