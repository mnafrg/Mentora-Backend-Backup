using Mentora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mentora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
       
          

            [HttpGet]
            [Authorize]
            public async Task<IActionResult> GetComments(int programId)
            {
                var result = await _commentService.GetCommentsAsync(programId);

                if (!result.Success)
                    return NotFound(result);

                return Ok(result);
            }

            [HttpPost]
            [Authorize]
            public async Task<IActionResult> AddComment(int programId, [FromBody] string commentText)
            {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User  not found .");

            var userId = Guid.Parse(userIdClaim.Value);
            var result = await _commentService.AddCommentAsync(programId, userId, commentText);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
        }
    }

