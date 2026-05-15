using Mentora.Domain.Entities.Classroom;

namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface IMaterialCompletionRepository
{
    /// Get a single completion record for (menteeId, materialId). Returns null if none exists yet.
    Task<MaterialCompletion?> GetAsync(Guid menteeId, int materialId);

    /// All completion records for a mentee inside a specific roadmap.
    Task<List<MaterialCompletion>> GetByMenteeAndRoadmapAsync(Guid menteeId, int roadmapId);

    /// All completion records for every mentee enrolled in a program (used for mentor analytics).
    Task<List<MaterialCompletion>> GetByProgramAsync(int programId);

    Task CreateAsync(MaterialCompletion completion);
    Task UpdateAsync(MaterialCompletion completion);
}