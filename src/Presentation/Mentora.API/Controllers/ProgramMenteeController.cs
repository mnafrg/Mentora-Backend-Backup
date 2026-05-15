using Mentora.Application.DTOs.Application;
using Mentora.Application.Interfaces;
using Mentora.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramMenteeController : ControllerBase
    {
        private readonly IProgramMenteeService _programMenteeService;

        public ProgramMenteeController(IProgramMenteeService programMenteeService)
        {
            _programMenteeService = programMenteeService;
        }








        [HttpGet("{programId}/view")]
        [Authorize]
        public async Task<IActionResult> GetProgramView(int programId)
        {
            var result = await _programMenteeService.GetProgramViewAsync(programId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("savedPosts")]
        [Authorize]
        public async Task<IActionResult> GetSavedPrograms()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _programMenteeService.GetSavedProgramsAsync(userId);
            return Ok(result);
        }


        [HttpGet("{programId}/mentor-card")]
        [Authorize]
        public async Task<IActionResult> GetMentorCard(int programId)
        {
         
            var result = await _programMenteeService.GetMentorCardByProgramIdAsync(programId);


            if (!result.Success)
                return NotFound(result);

            return Ok(result);

        }

        [HttpGet("{programId}/questions")]
        [Authorize(Roles = "Mentee")]
        public async Task<IActionResult> GetProgramQuestions(int programId)
        {
            
            var result = await _programMenteeService.GetProgramQuestionsAsync(programId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost("{programId}/like")]
        [Authorize]
        public async Task<IActionResult> ToggleLike(int programId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User  not found .");

            var userId = Guid.Parse(userIdClaim.Value);
            var result = await _programMenteeService.ToggleLikeProgramAsync(programId, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{programId}/save")]
        [Authorize]
        public async Task<IActionResult> ToggleSave(int programId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User  not found .");

            var userId = Guid.Parse(userIdClaim.Value);
            var result = await _programMenteeService.ToggleSaveProgramAsync(programId, userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{programId}/share-link")]
        [Authorize]
        public async Task<IActionResult> GenerateShareLink(int programId) 
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User  not found .");

            var senderId = Guid.Parse(userIdClaim.Value);

            var result = await _programMenteeService.GenerateShareLinkAsync(programId, senderId);

            if (!result.Success)
                return BadRequest(result); 

            return Ok(result);
        }

        [HttpGet("share/{encryptedLink}")]
        [Authorize]
        public async Task<IActionResult> GetProgramByShareLink(string encryptedLink)
        {
            var result = await _programMenteeService.GetProgramByShareLinkAsync(encryptedLink);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("{programId}/apply")]
        [Authorize(Roles = "Mentee")]
        public async Task<IActionResult> ApplyToProgram(int programId, [FromBody] CreateApplicationDto dto)
        {
            var menteeProfileId = Guid.Parse(User.FindFirst("MenteeProfileId")!.Value);
            var result = await _programMenteeService.ApplyToProgramAsync(programId, menteeProfileId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

       
    }
}