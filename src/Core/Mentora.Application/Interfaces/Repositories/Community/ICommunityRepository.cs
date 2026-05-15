using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityRepository
{
    Task<Domain.Entities.Community?> GetCommunityByIdAsync(Guid communityId);
    Task<IEnumerable<Domain.Entities.Community>> GetAllCommunitiesAsync();
    Task<IEnumerable<Domain.Entities.Community>> GetAllCommunitiesByMemberIdAsync(Guid userId);
    Task<Domain.Entities.Community> CreateAsync(Domain.Entities.Community community);
    Task UpdateAsync(Domain.Entities.Community community);
    Task DeleteAsync(Domain.Entities.Community community);
}
