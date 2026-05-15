using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Community;

public class CommunityReportRepository : ICommunityReportRepository
{
    private readonly ApplicationDbContext _context;

    public CommunityReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityReport?> GetReportByIdAsync(Guid reportId)
    {
        return await _context.CommunityReports
            .FirstOrDefaultAsync(r => r.CommunityReportId == reportId);
    }

    public async Task<IEnumerable<CommunityReport>> GetAllReportsByPostIdAsync(Guid postId)
    {
        return await _context.CommunityReports
            .Where(r => r.TargetPostId == postId && r.Status == CommunityReportStatus.Pending)
            .Include(r => r.Reporter)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<CommunityReport> CreateAsync(CommunityReport report)
    {
        await _context.CommunityReports.AddAsync(report);
        return report;
    }

    public Task UpdateAsync(CommunityReport report)
    {
        _context.CommunityReports.Update(report);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(CommunityReport report)
    {
        _context.CommunityReports.Remove(report);
        return Task.CompletedTask;
    }
}
