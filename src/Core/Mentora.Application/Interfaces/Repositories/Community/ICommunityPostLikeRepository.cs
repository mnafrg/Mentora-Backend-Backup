using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityPostLikeRepository
{
    Task<CommunityPostLike?> GetLikeAsync(Guid postId, Guid userId);
    Task<IEnumerable<CommunityPostLike>> GetAllLikesByPostIdAsync(Guid postId);
    Task<CommunityPostLike> CreateAsync(CommunityPostLike like);
    Task DeleteAsync(CommunityPostLike like);
}
