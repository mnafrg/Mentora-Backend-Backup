using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityPostRepository
{
    Task<CommunityPost?> GetPostByIdAsync(Guid postId);
    Task<CommunityPost?> GetPostWithDetailsAsync(Guid postId);
    Task<IEnumerable<CommunityPost>> GetAllPostsByCommunityIdAsync(Guid communityId);
    Task<IEnumerable<CommunityPost>> GetFeedPostsAsync();
    Task<CommunityPost> CreateAsync(CommunityPost post);
    Task UpdateAsync(CommunityPost post);
    Task DeleteAsync(CommunityPost post);
}
