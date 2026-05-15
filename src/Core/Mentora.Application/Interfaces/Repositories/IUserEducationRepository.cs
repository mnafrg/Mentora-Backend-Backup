using Mentora.Domain.Entities.Profiles;

namespace Mentora.Application.Interfaces.Repositories;
public interface IUserEducationRepository
{
    Task<List<UserEducation>> GetByUserIdAsync(Guid userId);
    Task<UserEducation?> GetByIdAsync(Guid educationId);
    Task CreateAsync(UserEducation education);
    Task UpdateAsync(UserEducation education);
    Task DeleteAsync(Guid educationId);
}