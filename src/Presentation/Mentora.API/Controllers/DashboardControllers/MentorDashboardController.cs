using Mentora.Application.DTOs.Programs;
using Mentora.Application.Interfaces;
using Mentora.Application.Services.DashboardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers.DashboardController
{

    [ApiController]
    [Route("api/[controller]")]
    public class MentorDashboardController : ControllerBase
    {
        private readonly IMentorDashboardServices _mentorService;

        public MentorDashboardController(IMentorDashboardServices mentorService)
        {
            _mentorService = mentorService;
        }


        //        [HttpGet("overallMentor-stats")]
        //        public async Task<IActionResult> GetOverallStats()
        //        {
        //            var stats = await _mentorService.GetOverallStatsAsync(User);
        //            return Ok(stats);
        //        }
        [HttpGet("recent-applicants")]
        [Authorize(Roles = "Mentor")]
        public async Task<IActionResult> GetRecentApplicants()
        {
            var mentorProfileId = Guid.Parse(User.FindFirst("MentorProfileId")!.Value);
            var result = await _mentorService.GetRecentApplicantsAsync(mentorProfileId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }



    }
}
