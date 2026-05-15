using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Mentora.API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IApplicantsService _applicantsService;

        public ApplicantsController(IApplicantsService applicantsService)
        {
            _applicantsService = applicantsService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetAllApplicants([FromQuery] GetAllApplicantsRequestDto request)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);

            if (request.IsExport)
            {
                var fileBytes = await _applicantsService.ExportApplicantsToExcelAsync(request, mentorProfileId);
                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Applicants_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }

            var result = await _applicantsService.GetAllApplicantsAsync(request, mentorProfileId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("by-program/")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetApplicantsByProgram(int programId, [FromQuery] GetApplicantsRequestDto request)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
           request.ProgramId = programId;

            if (request.IsExport)
            {
                var fileBytes = await _applicantsService.ExportApplicantsToExcelAsync(request, mentorProfileId);

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Applicants_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
            var result = await _applicantsService.GetApplicantsByProgramAsync(request, mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPatch("{applicationId}/accept")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> AcceptApplicant(int applicationId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _applicantsService.UpdateApplicationStatusAsync(applicationId, ApplicationStatus.Accepted, mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("{applicationId}/reject")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> RejectApplicant(int applicationId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _applicantsService.UpdateApplicationStatusAsync(applicationId, ApplicationStatus.Rejected, mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPatch("{applicationId}/pending")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> MoveToPending(int applicationId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _applicantsService.UpdateApplicationStatusAsync(
                applicationId, ApplicationStatus.Pending, mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
               
        [HttpPost("program/{programId}/notify-all")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> NotifyApplicants(int programId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);

            var result = await _applicantsService.NotifyAllApplicantsAsync(programId, mentorProfileId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("my-applications")]
        [Authorize(Roles = "Mentee")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User not found.");
            var menteeProfileId = Guid.Parse(userIdClaim.Value);
            var result = await _applicantsService.GetMenteeApplicationsAsync(menteeProfileId);
            return Ok(result);
        }

        [HttpGet("Applicant-profile")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetApplicantProfile(int applicationId)
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _applicantsService.GetApplicantProfileAsync(applicationId, mentorProfileId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
