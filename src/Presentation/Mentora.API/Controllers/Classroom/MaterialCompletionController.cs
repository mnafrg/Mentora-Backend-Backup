// src/Presentation/Mentora.API/Controllers/Classroom/MaterialCompletionController.cs

using Mentora.Application.Interfaces.Services.Classroom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers.Classroom;

[ApiController]
[Route("api/classroom")]
[Authorize(Roles = "Mentee")]
public class MaterialCompletionController : ControllerBase
{
    private readonly IMaterialCompletionService _service;

    public MaterialCompletionController(IMaterialCompletionService service)
    {
        _service = service;
    }


    /// POST /api/classroom/materials/{materialId}/complete
    /// Toggle completion of a material. Returns the new state.

    [HttpPost("materials/{materialId:int}/complete")]
    public async Task<IActionResult> ToggleCompletion(int materialId)
    {
        var menteeProfileId = GetMenteeId();
        var result = await _service.ToggleMaterialCompletionAsync(materialId, menteeProfileId);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    /// GET /api/classroom/roadmaps/{roadmapId}/material-completions
    /// Returns the completion status of every material in the roadmap for the calling mentee.

    [HttpGet("roadmaps/{roadmapId:int}/material-completions")]
    public async Task<IActionResult> GetRoadmapCompletions(int roadmapId)
    {
        var menteeProfileId = GetMenteeId();
        var result = await _service.GetMaterialCompletionsForRoadmapAsync(roadmapId, menteeProfileId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // --- Helper -----------------------
    private Guid GetMenteeId() =>
        Guid.Parse(User.FindFirst("MenteeProfileId")!.Value);
}