using Mentora.Domain.Entities.Interactions          ;

namespace Mentora.Application.Interfaces.Repositories;

public interface IFollowRepository
{
    Task<bool> ExistsAsync(Guid followerId, Guid followingId);
    Task CreateAsync(Follow follow);
    Task DeleteAsync(Guid followerId, Guid followingId);
    Task<List<Follow>> GetFollowingByUserAsync(Guid followerId);
    Task<int> GetFollowerCountAsync(Guid mentorId);
    Task<List<Follow>> GetFollowersByMentorAsync(Guid mentorId);
}