using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityPostShareRepository
{
    Task<int> GetSharesCountByPostIdAsync(Guid postId);
    Task<CommunityPostShare> CreateAsync(CommunityPostShare share);
}
