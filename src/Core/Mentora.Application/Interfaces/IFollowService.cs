using Mentora.Application.DTOs;
using Mentora.Application.DTOs.Interactions.Follow;
using Mentora.Application.DTOs.Common;
namespace Mentora.Application.Interfaces;

public interface IFollowService
{    
    Task<ApiResponse<bool>> FollowAsync(Guid followerId, Guid mentorId);
    Task<ApiResponse<bool>> UnfollowAsync(Guid followerId, Guid mentorId);
    Task<ApiResponse<List<FollowingMentorDto>>> GetFollowingAsync(Guid userId);
    Task<ApiResponse<int>> GetFollowerCountAsync(Guid mentorId);
}