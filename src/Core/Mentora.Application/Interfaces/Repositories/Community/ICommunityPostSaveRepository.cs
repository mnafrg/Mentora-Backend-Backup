using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityPostSaveRepository
{
    Task<CommunityPostSave?> GetSaveAsync(Guid postId, Guid userId);
    Task<IEnumerable<CommunityPostSave>> GetAllSavedPostsByUserIdAsync(Guid userId);
    Task<CommunityPostSave> CreateAsync(CommunityPostSave save);
    Task DeleteAsync(CommunityPostSave save);
}
