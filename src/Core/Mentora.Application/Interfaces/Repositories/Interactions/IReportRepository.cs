using Mentora.Domain.Entities.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

namespace Mentora.Application.Interfaces.Repositories.Interactions;
public interface IReportRepository
{
    /// Check if this reporter already filed a report for this target (spam guard).
    Task<bool> ExistsByReporterAndTargetAsync(Guid reporterId, ReportTargetType targetType, Guid targetId);

    Task CreateAsync(Report report);
}
