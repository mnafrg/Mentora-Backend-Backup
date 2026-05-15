using Mentora.Application.DTOs.Classroom;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Enums.Classroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskSubmissionController : ControllerBase
{
    private readonly ITaskSubmissionService _service;

    public TaskSubmissionController(ITaskSubmissionService service)
    {
        _service = service;
    }

    // ── Mentee ────────────────────────────────────────────────────────────────

    /// POST /api/tasksubmission/tasks/{taskId}
    [HttpPost("tasks/{taskId:int}")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> Create(int taskId, [FromBody] CreateSubmissionDto dto)
    {
        var result = await _service.CreateSubmissionAsync(taskId, GetMenteeId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// PATCH /api/tasksubmission/{submissionId}
    [HttpPatch("{submissionId:int}")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> Update(int submissionId, [FromBody] UpdateSubmissionDto dto)
    {
        var result = await _service.UpdateSubmissionAsync(submissionId, GetMenteeId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// DELETE /api/tasksubmission/{submissionId}
    [HttpDelete("{submissionId:int}")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> Delete(int submissionId)
    {
        var result = await _service.DeleteSubmissionAsync(submissionId, GetMenteeId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// GET /api/tasksubmission/tasks/{taskId}/my
    [HttpGet("tasks/{taskId:int}/my")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> GetMine(int taskId)
    {
        var result = await _service.GetMySubmissionAsync(taskId, GetMenteeId());
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// GET /api/tasksubmission/phases/{phaseId}/tasks?status=Todo
    [HttpGet("phases/{phaseId:int}/tasks")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> GetTasksForPhase(int phaseId, [FromQuery] string? status)
    {
        var result = await _service.GetMenteeTasksForPhaseAsync(phaseId, GetMenteeId(), status);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Mentor ────────────────────────────────────────────────────────────────

    /// GET /api/tasksubmission/programs/{programId}/submissions?status=Submitted
    [HttpGet("programs/{programId:int}/submissions")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> GetByProgram(
        int programId, [FromQuery] string? status)
    {
        SubmissionStatus? parsed = status is not null &&
            Enum.TryParse<SubmissionStatus>(status, true, out var s) ? s : null;

        var result = await _service.GetSubmissionsForProgramAsync(
            programId, GetMentorId(), parsed);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// GET /api/tasksubmission/roadmaps/{roadmapId}/submissions?status=Submitted
    [HttpGet("roadmaps/{roadmapId:int}/submissions")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> GetByRoadmap(
        int roadmapId, [FromQuery] string? status)
    {
        SubmissionStatus? parsed = status is not null &&
            Enum.TryParse<SubmissionStatus>(status, true, out var s) ? s : null;

        var result = await _service.GetSubmissionsForRoadmapAsync(
            roadmapId, GetMentorId(), parsed);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// POST /api/tasksubmission/{submissionId}/review
    [HttpPost("{submissionId:int}/review")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> Review(int submissionId, [FromBody] ReviewSubmissionDto dto)
    {
        var result = await _service.ReviewSubmissionAsync(submissionId, GetMentorId(), dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// GET /api/tasksubmission/programs/{programId}/task-registry
    [HttpGet("programs/{programId:int}/task-registry")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> GetTaskRegistry(int programId)
    {
        var result = await _service.GetTaskRegistryAsync(programId, GetMentorId());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private Guid GetMenteeId() =>
        Guid.Parse(User.FindFirst("MenteeProfileId")!.Value);

    private Guid GetMentorId() =>
        Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
}