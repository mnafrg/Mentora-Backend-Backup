using Mentora.Application.Interfaces.Services.Classroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mentora.API.Controllers.Classroom;

[ApiController]
[Route("api/classroom/program/{programId:int}/dashboard")]
[Authorize(Roles = "Mentor")]
public class MentorClassroomDashboardController : ControllerBase
{
    private readonly IMentorClassroomDashboardService _dashboardService;

    public MentorClassroomDashboardController(IMentorClassroomDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // --- Dashboard -------------------------------------

    /// GET /api/classroom/program/{programId}/dashboard
    /// Returns the full student analytics dashboard for the classroom:
    /// - Students waiting for review
    /// - Students at risk (2+ missed deadlines)
    /// - Average roadmap completion %
    /// - Per-student progress table

    [HttpGet]
    public async Task<IActionResult> GetDashboard(int programId)
    {
        var mentorProfileId = GetMentorId();
        var result = await _dashboardService.GetDashboardAsync(programId, mentorProfileId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // --- Remove Mentee -------------------------------------

    /// DELETE /api/classroom/program/{programId}/dashboard/students/{menteeId}
    /// Removes a mentee from the program:
    /// - Sets their application status to Rejected
    /// - Cancels any active Mentorship record
    [HttpDelete("students/{menteeId:guid}")]
    public async Task<IActionResult> RemoveMentee(int programId, Guid menteeId)
    {
        var mentorProfileId = GetMentorId();
        var result = await _dashboardService.RemoveMenteeFromProgramAsync(
            programId, menteeId, mentorProfileId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // --- Helper -------------------------------------

    private Guid GetMentorId() =>
        Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
}