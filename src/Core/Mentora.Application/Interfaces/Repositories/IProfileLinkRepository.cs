
using Mentora.Domain.Entities.Profiles;

namespace Mentora.Application.Interfaces.Repositories;
public interface IProfileLinkRepository
{
    Task<List<ProfileLink>> GetByUserIdAsync(Guid userId);
    Task<ProfileLink?> GetByIdAsync(Guid linkId);
    Task CreateAsync(ProfileLink link);
    Task UpdateAsync(ProfileLink link);
    Task DeleteAsync(Guid linkId);
}