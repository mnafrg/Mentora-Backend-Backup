using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityMemberRepository
{
    Task<CommunityMember?> GetMemberByIdAsync(Guid memberId);
    Task<CommunityMember?> GetMemberAsync(Guid communityId, Guid userId);
    Task<IEnumerable<CommunityMember>> GetAllMembersByCommunityIdAsync(Guid communityId);

    Task<CommunityMember> CreateAsync(CommunityMember member);
    Task UpdateAsync(CommunityMember member);
    Task DeleteAsync(CommunityMember member);
}
