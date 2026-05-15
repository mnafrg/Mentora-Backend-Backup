using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Programs;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramMentorController : ControllerBase
    {
        private readonly IProgramMentorService _programService;

        public ProgramMentorController(IProgramMentorService programService)
        {
            _programService = programService;
        }

        [HttpPost("Create_Program")]
       [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramٌRequestDto dto)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _programService.CreateProgramAsync(dto, mentorProfileId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("drafts")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetAllDrafts()
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _programService.GetAllDraftsAsync(mentorProfileId);
            return Ok(result);
        }

        [HttpGet("{programId}/GetById")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetProgramById(int programId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _programService.GetProgramByIdAsync(programId, mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("AllpublishedPrograms")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetAllPublished()
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _programService.GetAllPublishedAsync(mentorProfileId);
            return Ok(result);
        }


        [HttpPatch("{programId}/update")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> UpdateProgram(int programId, [FromBody] UpdateProgramDto dto)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _programService.UpdateProgramAsync(programId, dto, mentorProfileId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }




     

    }
}
