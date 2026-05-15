using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Persistence.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _ctx;
    public ReportRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<bool> ExistsByReporterAndTargetAsync(
        Guid reporterId, ReportTargetType targetType, Guid targetId) =>
        await _ctx.Reports
            .AnyAsync(r =>
                r.ReporterId == reporterId &&
                r.ReportedItem.TargetType == targetType &&
                r.ReportedItem.TargetId   == targetId);

    public async Task CreateAsync(Report report)
    {
        await _ctx.Reports.AddAsync(report);
    }
}