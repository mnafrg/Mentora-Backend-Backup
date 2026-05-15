using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class MaterialCompletionRepository : IMaterialCompletionRepository
{
    private readonly ApplicationDbContext _context;

    public MaterialCompletionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialCompletion?> GetAsync(Guid menteeId, int materialId) =>
        await _context.MaterialCompletions
            .FirstOrDefaultAsync(mc => mc.MenteeId == menteeId && mc.MaterialId == materialId);

    public async Task<List<MaterialCompletion>> GetByMenteeAndRoadmapAsync(Guid menteeId, int roadmapId) =>
        await _context.MaterialCompletions
            .Include(mc => mc.Material)
                .ThenInclude(m => m.Topic)
                    .ThenInclude(t => t.RoadmapPhase)
            .Where(mc =>
                mc.MenteeId == menteeId &&
                mc.Material.Topic.RoadmapPhase.RoadmapId == roadmapId)
            .ToListAsync();

    /// Returns all MaterialCompletion rows for mentees whose application to the program
    /// was Accepted, joined through the roadmap attached to the program.
    public async Task<List<MaterialCompletion>> GetByProgramAsync(int programId) =>
        await _context.MaterialCompletions
            .Include(mc => mc.Material)
                .ThenInclude(m => m.Topic)
                    .ThenInclude(t => t.RoadmapPhase)
            .Where(mc =>
                _context.Programs
                    .Where(p => p.ProgramId == programId && p.RoadmapId != null)
                    .Select(p => p.RoadmapId)
                    .Contains(mc.Material.Topic.RoadmapPhase.RoadmapId) &&
                _context.MentorshipApplications
                    .Where(a =>
                        a.ProgramId == programId &&
                        a.Status == Domain.Enums.ApplicationStatus.Accepted)
                    .Select(a => a.MenteeProfileId)
                    .Contains(mc.MenteeId))
            .ToListAsync();

    public async Task CreateAsync(MaterialCompletion completion)
    {
        await _context.MaterialCompletions.AddAsync(completion);
    }

    public Task UpdateAsync(MaterialCompletion completion)
    {
        _context.MaterialCompletions.Update(completion);
        return Task.CompletedTask;
    }
}