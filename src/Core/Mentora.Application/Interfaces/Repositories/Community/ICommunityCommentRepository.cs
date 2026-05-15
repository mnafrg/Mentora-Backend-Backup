using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityCommentRepository
{
    Task<CommunityComment?> GetCommentByIdAsync(Guid commentId);
    Task<IEnumerable<CommunityComment>> GetAllCommentsByPostIdAsync(Guid postId);
    Task<CommunityComment> CreateAsync(CommunityComment comment);
    Task UpdateAsync(CommunityComment comment);
    Task DeleteAsync(CommunityComment comment);
}
