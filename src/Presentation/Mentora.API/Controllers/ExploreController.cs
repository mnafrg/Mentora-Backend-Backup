
using global::Mentora.Application.Interfaces;
using Mentora.API.Extensions;
using Mentora.Application.DTOs.Explore;
using Mentora.Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Services; 
using Mentora.Application.DTOs;
namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]


public class ExploreController : ControllerBase
{
    private readonly IExploreService _exploreService;

    public ExploreController(IExploreService exploreService)
    {
        _exploreService = exploreService;
    }
    [HttpGet("roadmaps")]
    [Authorize]
    public async Task<IActionResult> GetRoadmaps([FromQuery] ExploreSearchRequest request)
    {
        var result = await _exploreService.ExploreRoadmapsAsync(request);
        return Ok(result);
    }

    [HttpGet("mentors")]
    [Authorize]
    public async Task<IActionResult> GetMentors([FromQuery] ExploreSearchRequest request)
    {
        var result = await _exploreService.ExploreMentorsAsync(request);
        return Ok(result);
    }

    [HttpGet("programs")]
    [Authorize]
    public async Task<IActionResult> GetPrograms([FromQuery] ExploreSearchRequest request)
    {
        var result = await _exploreService.ExploreProgramsAsync(request);
        return Ok(result);
    }
    [HttpGet("communities")]
        [Authorize]
    public async Task<IActionResult> GetCommunities([FromQuery] ExploreSearchRequest request)
    {
        var result = await _exploreService.ExploreCommunitiesAsync(request);
        return Ok(result);
    }


}
