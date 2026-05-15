using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Interactions.Follow;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities.Interactions;
using Mentora.Domain.Enums; 
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FollowService> _logger;
        public FollowService(IUnitOfWork unitOfWork, ILogger<FollowService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> FollowAsync(Guid followerId, Guid mentorId)
        {
            if (followerId == mentorId)
                return ApiResponse<bool>.ErrorResponse("You cannot follow yourself");
    
            // Target must be a Mentor
            var target = await _unitOfWork.Users.GetByIdAsync(mentorId);
            if (target == null || target.Role != UserRole.Mentor)
                return ApiResponse<bool>.ErrorResponse("Mentor not found");
    
            if (await _unitOfWork.Follows.ExistsAsync(followerId, mentorId))
                return ApiResponse<bool>.SuccessResponse(true, "Already following");
    
            await _unitOfWork.Follows.CreateAsync(new Follow
            {
                FollowerId  = followerId,
                FollowingId = mentorId,
                FollowedAt  = DateTime.UtcNow
            });
    
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Now following this mentor");
        }
 
        public async Task<ApiResponse<bool>> UnfollowAsync(Guid followerId, Guid mentorId)
        {
            await _unitOfWork.Follows.DeleteAsync(followerId, mentorId);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Unfollowed successfully");
        }
    
        public async Task<ApiResponse<List<FollowingMentorDto>>> GetFollowingAsync(Guid userId)
        {
            var follows = await _unitOfWork.Follows.GetFollowingByUserAsync(userId);
    
            var dtos = new List<FollowingMentorDto>();
            foreach (var f in follows)
            {
                var followerCount = await _unitOfWork.Follows.GetFollowerCountAsync(f.FollowingId);
                dtos.Add(new FollowingMentorDto
                {
                    MentorId          = f.FollowingId,
                    FullName          = $"{f.Following.FirstName} {f.Following.LastName}",
                    ProfilePictureUrl = f.Following.MentorProfile?.ProfilePictureUrl,
                    DomainName        = f.Following.MentorProfile?.Domain?.Name ?? "",
                    AverageRating     = f.Following.MentorProfile?.AverageRating,
                    FollowerCount     = followerCount,
                    FollowedAt        = f.FollowedAt
                });
            }
    
            return ApiResponse<List<FollowingMentorDto>>.SuccessResponse(dtos);
        }
    
        public async Task<ApiResponse<int>> GetFollowerCountAsync(Guid mentorId)
        {
            var count = await _unitOfWork.Follows.GetFollowerCountAsync(mentorId);
            return ApiResponse<int>.SuccessResponse(count);
        }


    }
}