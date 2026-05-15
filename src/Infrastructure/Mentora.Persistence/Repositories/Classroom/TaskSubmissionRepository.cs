using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Entities.Classroom;
using Mentora.Domain.Enums.Classroom;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class TaskSubmissionRepository : ITaskSubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public TaskSubmissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<TaskSubmission> BaseQuery() =>
        _context.TaskSubmissions
            .Include(s => s.Links)
            .Include(s => s.Review)
            .Include(s => s.MenteeProfile).ThenInclude(m => m.User)
            .Include(s => s.Task);

    public async Task<TaskSubmission?> GetByIdAsync(int submissionId) =>
        await BaseQuery().FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

    public async Task<TaskSubmission?> GetByTaskAndMenteeAsync(int taskId, Guid menteeProfileId) =>
        await BaseQuery().FirstOrDefaultAsync(
            s => s.TaskId == taskId && s.MenteeProfileId == menteeProfileId);

    public async Task<IEnumerable<TaskSubmission>> GetByProgramAsync(
        int programId, SubmissionStatus? statusFilter = null)
    {
        // First, resolve the roadmapId in a separate query — avoids the nullable subquery issue
        var roadmapId = await _context.Programs
            .Where(p => p.ProgramId == programId)
            .Select(p => p.RoadmapId)
            .FirstOrDefaultAsync();

        if (roadmapId == null)
            return Enumerable.Empty<TaskSubmission>();

        var query = BaseQuery()
            .Where(s => s.Task.Topic.RoadmapPhase.RoadmapId == roadmapId.Value);

        if (statusFilter.HasValue)
            query = query.Where(s => s.Status == statusFilter.Value);

        return await query.OrderByDescending(s => s.SubmittedAt ?? s.CreatedAt).ToListAsync();
    }
    
    public async Task<IEnumerable<TaskSubmission>> GetByRoadmapAsync(
        int roadmapId, SubmissionStatus? statusFilter = null)
    {
        var query = BaseQuery()
            .Where(s => s.Task.Topic.RoadmapPhase.RoadmapId == roadmapId);

        if (statusFilter.HasValue)
            query = query.Where(s => s.Status == statusFilter.Value);

        return await query.OrderByDescending(s => s.SubmittedAt ?? s.CreatedAt).ToListAsync();
    }

    public async Task<int> GetEnrolledMenteeCountAsync(int programId) =>
        await _context.MentorshipApplications
            .CountAsync(a => a.ProgramId == programId && a.Status == ApplicationStatus.Accepted);

    public async Task CreateAsync(TaskSubmission submission)
    {
        await _context.TaskSubmissions.AddAsync(submission);
    }

    public Task UpdateAsync(TaskSubmission submission)
    {
        _context.TaskSubmissions.Update(submission);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskSubmission submission)
    {
        _context.TaskSubmissions.Remove(submission);
        return Task.CompletedTask;
    }
}