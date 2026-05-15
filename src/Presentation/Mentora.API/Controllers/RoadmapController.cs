using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Roadmap;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;




[ApiController]
[Route("api/[controller]")]

public class RoadmapController : ControllerBase
{
    private readonly IRoadmapService _roadmapService;

    public RoadmapController(IRoadmapService roadmapService)
    {
        _roadmapService = roadmapService;
    }

    // ── ROADMAP ──

    [HttpPost("Create-basic-info")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateBasicInfo([FromBody] CreateRoadmapBasicInfoDto dto)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.CreateBasicInfoAsync(dto, mentorId);
        return Ok(result);
    }

    [HttpGet("View-basic-info")]
    [Authorize]
    public async Task<IActionResult> GetAllRoadmapsBasicInfo()
    {
        var result = await _roadmapService.GetAllRoadmapsBasicInfoAsync();
        return Ok(result);
    }
    [HttpPost("{roadmapId}/save")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> ToggleSaveRoadmap(int roadmapId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _roadmapService.ToggleSaveRoadmapAsync(roadmapId, userId);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("saved")]
    [Authorize(Roles = "Mentee")]
    public async Task<IActionResult> GetSavedRoadmaps()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _roadmapService.GetSavedRoadmapsAsync(userId);
        return Ok(result);
    }


    [HttpGet("View-my-published-Roadmaps")]
    [Authorize(Roles = "Mentor")] 
    public async Task<IActionResult> GetMentorPublishedRoadmaps()
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);

      
        var result = await _roadmapService.GetAllPublishedRoadmapsAsync(mentorId);

        return Ok(result);
    }

    [HttpGet("Get-Content")]
    [Authorize(Roles = "Mentor")]
     public async Task<IActionResult> GetContent(int roadmapId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.GetContentAsync(roadmapId, mentorId);
        return Ok(result);
    }

    [HttpGet("{roadmapId}")]
    [Authorize]
    public async Task<IActionResult> GetFullRoadmap(int roadmapId)
    {
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role == "Mentor")
        {
            var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _roadmapService.GetFullRoadmapAsync(roadmapId, mentorId, isMentor: true);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
        else
        {
            var menteeId = Guid.Parse(User.FindFirst("MenteeProfileId")!.Value);
            var result = await _roadmapService.GetFullRoadmapAsync(roadmapId, menteeId, isMentor: false);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
    [HttpPut("{roadmapId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdateRoadmap(int roadmapId, [FromBody] UpdateRoadmapDto dto)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.UpdateRoadmapAsync(roadmapId, dto, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{roadmapId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> DeleteRoadmap(int roadmapId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.DeleteRoadmapAsync(roadmapId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPatch("{roadmapId}/publish")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> PublishRoadmap(int roadmapId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.PublishRoadmapAsync(roadmapId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPatch("{roadmapId}/unpublish")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UnpublishRoadmap(int roadmapId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.UnpublishRoadmapAsync(roadmapId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ── PHASES ──

    [HttpPost("{roadmapId}/phases")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreatePhase(int roadmapId, [FromBody] CreateRoadmapPhaseDto dto)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        dto.RoadmapId = roadmapId;
        var result = await _roadmapService.CreatePhaseAsync(dto, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("phases/{phaseId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdatePhase(int phaseId, [FromBody] PhaseDto dto)
    {
        dto.PhaseId = phaseId;
        var result = await _roadmapService.UpdatePhaseAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("phases/{phaseId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> DeletePhase(int phaseId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.DeletePhaseAsync(phaseId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ── TOPICS ──

    [HttpPost("phases/{phaseId}/topics")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateTopic(int phaseId, [FromBody] CreateTopicDto dto)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
     
        var result = await _roadmapService.CreateTopicAsync(dto, phaseId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("topics/{topicId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdateTopic(int topicId, [FromBody] TopicDto dto)
    {
        dto.Id = topicId;
        var result = await _roadmapService.UpdateTopicAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("topics/{topicId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> DeleteTopic(int topicId)
    {
        var mentorId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
        var result = await _roadmapService.DeleteTopicAsync(topicId, mentorId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ── MATERIALS ──

    [HttpPost("topics/{topicId}/materials")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateMaterial(int topicId, [FromBody] ListOfMaterialsDto dto)
    {
        if (dto == null || dto.Materials == null || !dto.Materials.Any())
        
            return BadRequest("No materials provided.");
        
        var result = await _roadmapService.CreateMaterialsAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("materials/{materialId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdateMaterial(int materialId, [FromBody] MaterialDto dto)
    {
        dto.Id = materialId;
        var result = await _roadmapService.UpdateMaterialAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("materials/{materialId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> DeleteMaterial(int materialId)
    {
        var result = await _roadmapService.DeleteMaterialAsync(materialId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    // ── TASKS ──

    [HttpPost("topics/{topicId}/tasks")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> CreateTask(int topicId, [FromBody] TaskDto dto)
    {
        dto.Id = topicId;
        var result = await _roadmapService.CreateTaskAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("tasks/{taskId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto dto)
    {
        dto.Id = taskId;
        var result = await _roadmapService.UpdateTaskAsync(dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("tasks/{taskId}")]
    [Authorize(Roles = "Mentor")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var result = await _roadmapService.DeleteTaskAsync(taskId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}