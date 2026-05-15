using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.DTOs.Profile;
using Mentora.Application.Interfaces;
using Mentora.API.Extensions;
using Mentora.Domain.Enums;
namespace Mentora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IUnitOfWork     _unitOfWork;

    public ProfileController(IProfileService profileService, IUnitOfWork unitOfWork)
    {
        _profileService = profileService;
        _unitOfWork     = unitOfWork;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetOwnProfile()
    {
        var userId = User.GetUserId();
        var user   = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return Unauthorized();

        if (user.Role == UserRole.Mentor)
        {
            var result = await _profileService.GetMentorOwnProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        else
        {
            var result = await _profileService.GetMenteeOwnProfileAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }

    [HttpGet("{targetUserId:guid}")]
    public async Task<IActionResult> GetPublicProfile(Guid targetUserId)
    {
        var viewerId = User.GetUserId();
        var result   = await _profileService.GetPublicProfileAsync(targetUserId);
        return result.Success ? Ok(result) : NotFound(result);
    }


    /// Update bio, country code, or profile picture for a mentee.
    [HttpPut("me/mentee")]
    [Authorize(Policy = "MenteeOnly")]
    public async Task<IActionResult> UpdateMenteeProfile([FromBody] UpdateMenteeProfileRequest request)
    {
        var result = await _profileService.UpdateMenteeProfileAsync(User.GetUserId(), request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    
    // PUT /api/profile/me/mentor - Update mentor-specific fields like expertise, experience, etc.
    [HttpPut("me/mentor")]
    [Authorize(Policy = "MentorOnly")]
    public async Task<IActionResult> UpdateMentorProfile([FromBody] UpdateMentorProfileRequest request)
    {
        var result = await _profileService.UpdateMentorProfileAsync(User.GetUserId(), request);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    // POST /api/profile/me/education - Add a new education entry
    [HttpPost("me/education")]
    public async Task<IActionResult> AddEducation([FromBody] AddEducationRequest request)
    {
        var result = await _profileService.AddEducationAsync(User.GetUserId(), request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PUT /api/profile/me/education/{educationId} - Update an existing education entry
    [HttpPut("me/education/{educationId:guid}")]
    public async Task<IActionResult> UpdateEducation(
        Guid educationId,
        [FromBody] UpdateEducationRequest request)
    {
        var result = await _profileService.UpdateEducationAsync(
            User.GetUserId(), educationId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    // DELETE /api/profile/me/education/{educationId} - Delete an education entry
    [HttpDelete("me/education/{educationId:guid}")]
    public async Task<IActionResult> DeleteEducation(Guid educationId)
    {
        var result = await _profileService.DeleteEducationAsync(
            User.GetUserId(), educationId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // POST /api/profile/me/link - Add a new profile link
    [HttpPost("me/links")]
    public async Task<IActionResult> AddLink([FromBody] AddLinkRequest request)
    {
        var result = await _profileService.AddLinkAsync(User.GetUserId(), request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // PUT /api/profile/me/link/{linkId} - Update an existing profile link (only owner can update)
    [HttpPut("me/links/{linkId:guid}")]
    public async Task<IActionResult> UpdateLink(
        Guid linkId,
        [FromBody] UpdateLinkRequest request)
    {
        var result = await _profileService.UpdateLinkAsync(User.GetUserId(), linkId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE /api/profile/me/link/{linkId} - Delete a profile link (only owner can delete)
    [HttpDelete("me/links/{linkId:guid}")]
    public async Task<IActionResult> DeleteLink(Guid linkId)
    {
        var result = await _profileService.DeleteLinkAsync(User.GetUserId(), linkId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}