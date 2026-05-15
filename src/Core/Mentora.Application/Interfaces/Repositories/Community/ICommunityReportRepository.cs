using Mentora.Domain.Entities;

namespace Mentora.Application.Interfaces.Repositories.Community;

public interface ICommunityReportRepository
{
    Task<CommunityReport?> GetReportByIdAsync(Guid reportId);
    Task<IEnumerable<CommunityReport>> GetAllReportsByPostIdAsync(Guid postId);
    Task<CommunityReport> CreateAsync(CommunityReport report);
    Task UpdateAsync(CommunityReport report);
    Task DeleteAsync(CommunityReport report);
}
