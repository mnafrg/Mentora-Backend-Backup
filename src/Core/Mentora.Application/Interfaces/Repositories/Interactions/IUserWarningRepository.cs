using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Application.Interfaces.Repositories.Interactions;
public interface IUserWarningRepository
{
    Task<List<UserWarning>> GetByUserIdAsync(Guid userId);
    Task CreateAsync(UserWarning warning);
}