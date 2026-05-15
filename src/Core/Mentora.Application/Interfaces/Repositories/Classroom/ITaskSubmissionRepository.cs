using Mentora.Domain.Entities.Classroom;
using Mentora.Domain.Enums.Classroom;

namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface ITaskSubmissionRepository
{
    Task<TaskSubmission?> GetByIdAsync(int submissionId);

    Task<TaskSubmission?> GetByTaskAndMenteeAsync(int taskId, Guid menteeProfileId);

    /// All submissions for a program (mentor view).
    Task<IEnumerable<TaskSubmission>> GetByProgramAsync(
        int programId,
        SubmissionStatus? statusFilter = null);

    /// All submissions for every task in a roadmap (mentor view).
    Task<IEnumerable<TaskSubmission>> GetByRoadmapAsync(
        int roadmapId,
        SubmissionStatus? statusFilter = null);

    /// Count of accepted mentees in a program (for registry denominator).
    Task<int> GetEnrolledMenteeCountAsync(int programId);

    Task CreateAsync(TaskSubmission submission);
    Task UpdateAsync(TaskSubmission submission);
    Task DeleteAsync(TaskSubmission submission);
}