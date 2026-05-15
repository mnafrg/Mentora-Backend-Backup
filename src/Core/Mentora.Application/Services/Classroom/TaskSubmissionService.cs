using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Entities.Classroom;
using Mentora.Domain.Enums.Classroom;
using Microsoft.Extensions.Logging;
using Mentora.Application.Interfaces;

namespace Mentora.Application.Services.Classroom;

public class TaskSubmissionService : ITaskSubmissionService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<TaskSubmissionService> _logger;

    public TaskSubmissionService(IUnitOfWork uow, ILogger<TaskSubmissionService> logger)
    {
        _uow    = uow;
        _logger = logger;
    }

    // ── Mentee: Create ────────────────────────────────────────────────────────

    public async Task<ApiResponse<SubmissionResponseDto>> CreateSubmissionAsync(
        int taskId, Guid menteeProfileId, CreateSubmissionDto dto)
    {
        // Verify the task exists
        var task = await _uow.Tasks.GetByIdAsync(taskId);
        if (task is null)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse("Task not found");

        // One submission per mentee per task
        var existing = await _uow.TaskSubmissions.GetByTaskAndMenteeAsync(taskId, menteeProfileId);
        if (existing is not null)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse(
                "You already have a submission for this task");

        var submission = new TaskSubmission
        {
            TaskId          = taskId,
            MenteeProfileId = menteeProfileId,
            Title           = dto.Title.Trim(),
            NotesForMentor  = dto.NotesForMentor?.Trim(),
            Status          = dto.Publish ? SubmissionStatus.Submitted : SubmissionStatus.Draft,
            CreatedAt       = DateTime.UtcNow,
            SubmittedAt     = dto.Publish ? DateTime.UtcNow : null,
            Links           = dto.Links.Select(l => new SubmissionLink
            {
                Url   = l.Url.Trim(),
                Label = l.Label?.Trim()
            }).ToList()
        };

        await _uow.TaskSubmissions.CreateAsync(submission);
        await _uow.SaveChangesAsync();

        var full = await _uow.TaskSubmissions.GetByIdAsync(submission.SubmissionId);
        return ApiResponse<SubmissionResponseDto>.SuccessResponse(
            Map(full!), dto.Publish ? "Submission published" : "Draft saved");
    }

    // ── Mentee: Update ────────────────────────────────────────────────────────

    public async Task<ApiResponse<SubmissionResponseDto>> UpdateSubmissionAsync(
        int submissionId, Guid menteeProfileId, UpdateSubmissionDto dto)
    {
        var submission = await _uow.TaskSubmissions.GetByIdAsync(submissionId);
        if (submission is null || submission.MenteeProfileId != menteeProfileId)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse("Submission not found");

        if (submission.Status == SubmissionStatus.Reviewed)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse(
                "Reviewed submissions cannot be edited");

        if (dto.Title is not null)          submission.Title          = dto.Title.Trim();
        if (dto.NotesForMentor is not null) submission.NotesForMentor = dto.NotesForMentor.Trim();
        if (dto.Links is not null)
        {
            submission.Links.Clear();
            foreach (var l in dto.Links)
                submission.Links.Add(new SubmissionLink { Url = l.Url.Trim(), Label = l.Label?.Trim() });
        }

        // Publishing is allowed from both Draft and Submitted states
        if (dto.Publish == true && submission.Status == SubmissionStatus.Draft)
        {
            submission.Status      = SubmissionStatus.Submitted;
            submission.SubmittedAt = DateTime.UtcNow;
        }

        submission.UpdatedAt = DateTime.UtcNow;
        await _uow.TaskSubmissions.UpdateAsync(submission);
        await _uow.SaveChangesAsync();

        var full = await _uow.TaskSubmissions.GetByIdAsync(submissionId);
        return ApiResponse<SubmissionResponseDto>.SuccessResponse(Map(full!), "Submission updated");
    }

    // ── Mentee: Delete ────────────────────────────────────────────────────────

    public async Task<ApiResponse<bool>> DeleteSubmissionAsync(
        int submissionId, Guid menteeProfileId)
    {
        var submission = await _uow.TaskSubmissions.GetByIdAsync(submissionId);
        if (submission is null || submission.MenteeProfileId != menteeProfileId)
            return ApiResponse<bool>.ErrorResponse("Submission not found");

        if (submission.Status == SubmissionStatus.Reviewed)
            return ApiResponse<bool>.ErrorResponse("Reviewed submissions cannot be deleted");

        // EF cascade will remove links and review
        await _uow.TaskSubmissions.DeleteAsync(submission);
        await _uow.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResponse(true, "Submission deleted");
    }

    // ── Mentee: Get my submission ─────────────────────────────────────────────

    public async Task<ApiResponse<SubmissionResponseDto>> GetMySubmissionAsync(
        int taskId, Guid menteeProfileId)
    {
        var submission = await _uow.TaskSubmissions.GetByTaskAndMenteeAsync(taskId, menteeProfileId);
        if (submission is null)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse("No submission found for this task");

        return ApiResponse<SubmissionResponseDto>.SuccessResponse(Map(submission));
    }

    // ── Mentee: Task list for a phase with personal status ────────────────────

    public async Task<ApiResponse<IEnumerable<MenteeTaskStatusDto>>> GetMenteeTasksForPhaseAsync(
        int phaseId, Guid menteeProfileId, string? statusFilter = null)
    {
        var topics = await _uow.Topics.GetByPhaseIdAsync(phaseId);

        var result = new List<MenteeTaskStatusDto>();
        foreach (var topic in topics)
        {
            if (!topic.Tasks.Any()) continue; // ← بدل is null

            foreach (var task in topic.Tasks) // ← لف على كل Task
            {
                var submission = await _uow.TaskSubmissions
                    .GetByTaskAndMenteeAsync(task.TaskId, menteeProfileId);

                string personalStatus = submission?.Status switch
                {
                    SubmissionStatus.Draft => "Draft",
                    SubmissionStatus.Submitted => "Submitted",
                    SubmissionStatus.Reviewed => "Reviewed",
                    _ => "Todo"
                };

                if (statusFilter is not null &&
                    !personalStatus.Equals(statusFilter, StringComparison.OrdinalIgnoreCase))
                    continue;

                result.Add(new MenteeTaskStatusDto
                {
                    TaskId = task.TaskId,
                    TaskTitle = task.Title,
                    TaskDescription = task.Description,
                    Deadline = task.DeadLine,
                    PersonalStatus = personalStatus,
                    Submission = submission is not null ? Map(submission) : null
                });
            }
        }

        return ApiResponse<IEnumerable<MenteeTaskStatusDto>>.SuccessResponse(result);
    }

    // ── Mentor: View submissions ──────────────────────────────────────────────

    public async Task<ApiResponse<IEnumerable<SubmissionResponseDto>>> GetSubmissionsForProgramAsync(
        int programId, Guid mentorProfileId, SubmissionStatus? statusFilter = null)
    {
        var program = await _uow.Programs.GetByIdAsync(programId);
        if (program is null || program.MentorProfileId != mentorProfileId)
            return ApiResponse<IEnumerable<SubmissionResponseDto>>.ErrorResponse(
                "Program not found or access denied");

        var submissions = await _uow.TaskSubmissions.GetByProgramAsync(programId, statusFilter);
        return ApiResponse<IEnumerable<SubmissionResponseDto>>.SuccessResponse(
            submissions.Select(Map));
    }

    public async Task<ApiResponse<IEnumerable<SubmissionResponseDto>>> GetSubmissionsForRoadmapAsync(
        int roadmapId, Guid mentorProfileId, SubmissionStatus? statusFilter = null)
    {
        var roadmap = await _uow.Roadmaps.GetByIdAsync(roadmapId);
        if (roadmap is null || roadmap.MentorProfileId != mentorProfileId)
            return ApiResponse<IEnumerable<SubmissionResponseDto>>.ErrorResponse(
                "Roadmap not found or access denied");

        var submissions = await _uow.TaskSubmissions.GetByRoadmapAsync(roadmapId, statusFilter);
        return ApiResponse<IEnumerable<SubmissionResponseDto>>.SuccessResponse(
            submissions.Select(Map));
    }

    // ── Mentor: Review ────────────────────────────────────────────────────────

    public async Task<ApiResponse<SubmissionResponseDto>> ReviewSubmissionAsync(
        int submissionId, Guid mentorProfileId, ReviewSubmissionDto dto)
    {
        var submission = await _uow.TaskSubmissions.GetByIdAsync(submissionId);
        if (submission is null)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse("Submission not found");

        if (submission.Status != SubmissionStatus.Submitted)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse(
                "Only submitted submissions can be reviewed");

        // Validate: a final review requires a grade
        if (!dto.RequestRevision && dto.Grade is null)
            return ApiResponse<SubmissionResponseDto>.ErrorResponse(
                "Grade is required for a final review");

        if (dto.RequestRevision)
        {
            // Reset to Todo so mentee can resubmit
            submission.Status = SubmissionStatus.Draft;
            submission.SubmittedAt = null;
        }
        else
        {
            submission.Status = SubmissionStatus.Reviewed;
        }

        submission.UpdatedAt = DateTime.UtcNow;

        // Upsert the review record
        var existingReview = await _uow.SubmissionReviews.GetBySubmissionIdAsync(submissionId);
        if (existingReview is null)
        {
            await _uow.SubmissionReviews.CreateAsync(new SubmissionReview
            {
                SubmissionId      = submissionId,
                MentorProfileId   = mentorProfileId,
                Grade             = dto.Grade,
                Feedback          = dto.Feedback?.Trim(),
                IsRevisionRequest = dto.RequestRevision,
                ReviewedAt        = DateTime.UtcNow
            });
        }
        else
        {
            existingReview.Grade             = dto.Grade;
            existingReview.Feedback          = dto.Feedback?.Trim();
            existingReview.IsRevisionRequest = dto.RequestRevision;
            existingReview.UpdatedAt         = DateTime.UtcNow;
            await _uow.SubmissionReviews.UpdateAsync(existingReview);
        }

        await _uow.TaskSubmissions.UpdateAsync(submission);
        await _uow.SaveChangesAsync();

        var full = await _uow.TaskSubmissions.GetByIdAsync(submissionId);
        var msg  = dto.RequestRevision ? "Revision requested" : "Submission reviewed successfully";
        return ApiResponse<SubmissionResponseDto>.SuccessResponse(Map(full!), msg);
    }

    // ── Analytics: Task registry ──────────────────────────────────────────────

    public async Task<ApiResponse<IEnumerable<TaskRegistryDto>>> GetTaskRegistryAsync(
        int programId, Guid mentorProfileId)
    {
        // Use GetAllAsync with a filter instead of FindAsync to avoid stale tracked entities
        var programs = await _uow.Programs.GetAllAsync(
            p => p.ProgramId == programId);
        var program = programs.FirstOrDefault();

        if (program is null || program.MentorProfileId != mentorProfileId)
            return ApiResponse<IEnumerable<TaskRegistryDto>>.ErrorResponse(
                "Program not found or access denied");

        if (program.RoadmapId is null)
            return ApiResponse<IEnumerable<TaskRegistryDto>>.ErrorResponse(
                "This program has no roadmap attached");

        var totalStudents = await _uow.TaskSubmissions.GetEnrolledMenteeCountAsync(programId);

        var roadmap = await _uow.Roadmaps.GetByIdWithFullHierarchyAsync(program.RoadmapId.Value);
        if (roadmap is null)
            return ApiResponse<IEnumerable<TaskRegistryDto>>.ErrorResponse("Roadmap not found");

        var allSubmissions = await _uow.TaskSubmissions
            .GetByRoadmapAsync(roadmap.RoadmapId);
        var submissionsByTask = allSubmissions
            .GroupBy(s => s.TaskId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<TaskRegistryDto>();
        foreach (var phase in roadmap.Phases)
        {
            foreach (var topic in phase.Topics)
            {
                if (!topic.Tasks.Any()) continue;

                foreach (var task in topic.Tasks) 
                {
                    var taskId = task.TaskId;
                    var subs = submissionsByTask.GetValueOrDefault(taskId, new());
                    var total = subs.Count;
                    var reviewed = subs.Count(s => s.Status == SubmissionStatus.Reviewed);
                    var avg = subs
                        .Where(s => s.Review?.Grade is not null)
                        .Select(s => (double)s.Review!.Grade!)
                        .DefaultIfEmpty()
                        .Average();
                    var isDone = totalStudents > 0 && reviewed >= totalStudents;

                    result.Add(new TaskRegistryDto
                    {
                        TaskId = taskId,
                        TaskName = task.Title,      
                        PhaseName = phase.Title,
                        TotalStudents = totalStudents,
                        TotalSubmissions = total,
                        ReviewedSubmissions = reviewed,
                        AverageScore = reviewed > 0 ? Math.Round(avg, 1) : null,
                        SubmissionRate = totalStudents > 0 ? Math.Round((double)total / totalStudents, 2) : 0,
                        ReviewRate = total > 0 ? Math.Round((double)reviewed / total, 2) : 0,
                        CompletionRate = totalStudents > 0 ? Math.Round((double)reviewed / totalStudents, 2) : 0,
                        Status = isDone ? "Done" : "StillRunning"
                    });
                }
            }
        }

        return ApiResponse<IEnumerable<TaskRegistryDto>>.SuccessResponse(result);
    }

    // ── Mapping helper ────────────────────────────────────────────────────────

    private static SubmissionResponseDto Map(TaskSubmission s) => new()
    {
        SubmissionId        = s.SubmissionId,
        TaskId              = s.TaskId,
        TaskTitle           = s.Task?.Title ?? string.Empty,
        MenteeProfileId     = s.MenteeProfileId,
        MenteeName          = s.MenteeProfile?.User is null
                                ? string.Empty
                                : $"{s.MenteeProfile.User.FirstName} {s.MenteeProfile.User.LastName}",
        MenteeProfilePicture= s.MenteeProfile?.ProfilePictureUrl,
        Title               = s.Title,
        NotesForMentor      = s.NotesForMentor,
        Status              = s.Status.ToString(),
        Links               = s.Links.Select(l => new SubmissionLinkDto
                              { Url = l.Url, Label = l.Label }).ToList(),
        CreatedAt           = s.CreatedAt,
        SubmittedAt         = s.SubmittedAt,
        Review              = s.Review is null ? null : new ReviewDto
        {
            ReviewId          = s.Review.ReviewId,
            Grade             = s.Review.Grade,
            Feedback          = s.Review.Feedback,
            IsRevisionRequest = s.Review.IsRevisionRequest,
            ReviewedAt        = s.Review.ReviewedAt
        }
    };
}