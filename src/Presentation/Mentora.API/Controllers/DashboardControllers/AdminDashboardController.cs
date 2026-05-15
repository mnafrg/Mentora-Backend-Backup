using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces;


namespace Mentora.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminService;

        public AdminDashboardController(IAdminDashboardService adminService)
        {
            _adminService = adminService;
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("stats")]
        //public async Task<IActionResult> GetGlobalStats()
        //{
        //    var result = await _adminService.GetGlobalStatsAsync();

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

    
    }
}
