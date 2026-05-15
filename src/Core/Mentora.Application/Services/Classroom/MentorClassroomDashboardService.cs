using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Enums;
using Mentora.Domain.Enums.Classroom;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services.Classroom;

public class MentorClassroomDashboardService : IMentorClassroomDashboardService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<MentorClassroomDashboardService> _logger;

    // A student is "at risk" if they have missed deadlines on this many tasks with no submission
    private const int AtRiskMissedTaskThreshold = 2;

    public MentorClassroomDashboardService(
        IUnitOfWork uow,
        ILogger<MentorClassroomDashboardService> logger)
    {
        _uow    = uow;
        _logger = logger;
    }

    // --- Dashboard ----------------------------

    public async Task<ApiResponse<MentorClassroomDashboardDto>> GetDashboardAsync(
        int programId, Guid mentorProfileId)
    {
        // 1. Verify ownership
        var program = await _uow.Programs.GetByIdAsync(programId);
        if (program is null || program.MentorProfileId != mentorProfileId)
            return ApiResponse<MentorClassroomDashboardDto>.ErrorResponse(
                "Program not found or access denied");

        if (program.RoadmapId is null)
            return ApiResponse<MentorClassroomDashboardDto>.ErrorResponse(
                "This program has no roadmap attached");

        // 2. Load roadmap with full hierarchy (phases → topics → materials + tasks)
        var roadmap = await _uow.Roadmaps.GetByIdWithFullHierarchyAsync(program.RoadmapId.Value);
        if (roadmap is null)
            return ApiResponse<MentorClassroomDashboardDto>.ErrorResponse("Roadmap not found");

        // 3. Load all accepted mentees for the program
        var acceptedApplications = await _uow.MentorshipApplications.GetAllAsync(
            a => a.ProgramId == programId && a.Status == ApplicationStatus.Accepted,
            a => a.MenteeProfile.User);

        if (!acceptedApplications.Any())
        {
            return ApiResponse<MentorClassroomDashboardDto>.SuccessResponse(
                new MentorClassroomDashboardDto
                {
                    StudentsWaitingForReview  = 0,
                    StudentsAtRisk            = 0,
                    AverageRoadmapCompletion  = 0,
                    Students                  = new()
                });
        }

        // 4. Flatten all materials and tasks from the roadmap once
        var allMaterials = roadmap.Phases
            .SelectMany(p => p.Topics)
            .SelectMany(t => t.Materials)
            .ToList();

      var allTasks = roadmap.Phases
      .SelectMany(p => p.Topics)
      .SelectMany(t => t.Tasks)
      .ToList();
        int totalMaterials = allMaterials.Count;
        int totalTasks     = allTasks.Count;
        var allTaskIds     = allTasks.Select(t => t.TaskId).ToHashSet();

        // 5. Load all submissions for this roadmap in one query
        var allSubmissions = (await _uow.TaskSubmissions.GetByRoadmapAsync(roadmap.RoadmapId))
            .ToList();

        // 6. Load all material completions for this program in one query
        var allMaterialCompletions = await _uow.MaterialCompletions.GetByProgramAsync(programId);

        var now = DateTime.UtcNow;

        // 7. Build per-student rows
        var studentRows = new List<StudentProgressDto>();

        foreach (var application in acceptedApplications)
        {
            var mentee     = application.MenteeProfile;
            var menteeUser = mentee.User;
            var menteeId   = mentee.UserId;

            // --- Material completion --------------------
            var menteeCompletions = allMaterialCompletions
                .Where(mc => mc.MenteeId == menteeId && mc.IsCompleted)
                .ToList();

            int completedMaterials = menteeCompletions.Count;

            // --- Task submission metrics -------------------
            var menteeSubmissions = allSubmissions
                .Where(s => s.MenteeProfileId == menteeId)
                .ToList();

            int completedTasks        = menteeSubmissions.Count(s => s.Status == SubmissionStatus.Reviewed);
            int waitingForReviewCount = menteeSubmissions.Count(s => s.Status == SubmissionStatus.Submitted);

            // Submitted task IDs (Draft OR Submitted OR Reviewed)
            var submittedTaskIds = menteeSubmissions.Select(s => s.TaskId).ToHashSet();

            // --- At-risk detection ------------------------
            // A task is "missed" when its deadline has passed and the mentee has no submission
            int missedDeadlines = allTasks.Count(task =>
                task.DeadLine.HasValue &&
                task.DeadLine.Value < now &&
                !submittedTaskIds.Contains(task.TaskId));

            bool isAtRisk = missedDeadlines >= AtRiskMissedTaskThreshold;

            // --- Last completed item -------------------------
            DateTime? lastMaterialDate   = menteeCompletions.Any()
                ? menteeCompletions.Max(mc => mc.CompletedAt)
                : null;

            var lastReviewedSubmission = menteeSubmissions
                .Where(s => s.Status == SubmissionStatus.Reviewed && s.UpdatedAt.HasValue)
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefault();

            DateTime? lastTaskDate = lastReviewedSubmission?.UpdatedAt;

            string? lastCompletedTitle = null;
            DateTime? lastCompletedAt  = null;

            if (lastMaterialDate.HasValue || lastTaskDate.HasValue)
            {
                if (lastMaterialDate > lastTaskDate)
                {
                    // Last activity was a material completion
                    var lastMaterial = menteeCompletions
                        .OrderByDescending(mc => mc.CompletedAt)
                        .First();
                    lastCompletedTitle = allMaterials
                        .FirstOrDefault(m => m.MaterialId == lastMaterial.MaterialId)?.Title;
                    lastCompletedAt = lastMaterialDate;
                }
                else
                {
                    // Last activity was a task review
                    lastCompletedTitle = allTasks
                        .FirstOrDefault(t => t.TaskId == lastReviewedSubmission!.TaskId)?.Title;
                    lastCompletedAt = lastTaskDate;
                }
            }

            // --- Progress percentages ------------------
            double materialsPercent = totalMaterials > 0
                ? Math.Round((double)completedMaterials / totalMaterials * 100, 1)
                : 0;

            double tasksPercent = totalTasks > 0
                ? Math.Round((double)completedTasks / totalTasks * 100, 1)
                : 0;

            int totalItems     = totalMaterials + totalTasks;
            int completedItems = completedMaterials + completedTasks;

            double overallPercent = totalItems > 0
                ? Math.Round((double)completedItems / totalItems * 100, 1)
                : 0;

            studentRows.Add(new StudentProgressDto
            {
                StudentId                  = menteeId,
                FullName                   = $"{menteeUser.FirstName} {menteeUser.LastName}",
                ProfilePictureUrl          = mentee.ProfilePictureUrl,
                LastCompletedItemTitle     = lastCompletedTitle,
                LastCompletedAt            = lastCompletedAt,
                CompletedMaterials         = completedMaterials,
                TotalMaterials             = totalMaterials,
                MaterialsCompletionPercent = materialsPercent,
                CompletedTasks             = completedTasks,
                TotalTasks                 = totalTasks,
                TasksCompletionPercent     = tasksPercent,
                OverallCompletionPercent   = overallPercent,
                IsAtRisk                   = isAtRisk,
                TasksWaitingForReview      = waitingForReviewCount
            });
        }

        // 8. Aggregate metrics
        int studentsWaitingForReview = studentRows.Count(s => s.TasksWaitingForReview > 0);
        int studentsAtRisk           = studentRows.Count(s => s.IsAtRisk);
        double averageCompletion     = studentRows.Any()
            ? Math.Round(studentRows.Average(s => s.OverallCompletionPercent), 1)
            : 0;

        var dashboard = new MentorClassroomDashboardDto
        {
            StudentsWaitingForReview = studentsWaitingForReview,
            StudentsAtRisk           = studentsAtRisk,
            AverageRoadmapCompletion = averageCompletion,
            Students                 = studentRows.OrderBy(s => s.FullName).ToList()
        };

        return ApiResponse<MentorClassroomDashboardDto>.SuccessResponse(dashboard);
    }

    // --- Remove Mentee -------------------------------------

    public async Task<ApiResponse<RemoveMenteeResponseDto>> RemoveMenteeFromProgramAsync(
        int programId, Guid menteeId, Guid mentorProfileId)
    {
        // 1. Verify program ownership
        var program = await _uow.Programs.GetByIdAsync(programId);
        if (program is null || program.MentorProfileId != mentorProfileId)
            return ApiResponse<RemoveMenteeResponseDto>.ErrorResponse(
                "Program not found or access denied");

        // 2. Find the accepted application
        var applications = await _uow.MentorshipApplications.GetAllAsync(
            a => a.ProgramId       == programId &&
                 a.MenteeProfileId == menteeId  &&
                 a.Status          == ApplicationStatus.Accepted,
            a => a.MenteeProfile.User);

        var application = applications.FirstOrDefault();
        if (application is null)
            return ApiResponse<RemoveMenteeResponseDto>.ErrorResponse(
                "No accepted application found for this mentee in this program");

        await _uow.BeginTransactionAsync();
        try
        {
            // 3. Reject the application (soft removal — keeps audit trail)
            application.Status     = ApplicationStatus.Rejected;
            application.DecisionAt = DateTime.UtcNow;
            await _uow.MentorshipApplications.UpdateAsync(application);

            // 4. Cancel any active Mentorship record for this mentee in this program
            var mentorships = await _uow.Mentorships.GetAllAsync(
                m => m.ProgramId        == programId       &&
                     m.MenteeProfileId  == menteeId        &&
                     m.Status           == MentorshipStatus.Active);

            foreach (var mentorship in mentorships)
            {
                mentorship.Status  = MentorshipStatus.Cancelled;
                mentorship.EndDate = DateTime.UtcNow;
                await _uow.Mentorships.UpdateAsync(mentorship);
            }

            await _uow.SaveChangesAsync();
            await _uow.CommitTransactionAsync();

            var menteeUser = application.MenteeProfile.User;
            var fullName   = $"{menteeUser.FirstName} {menteeUser.LastName}";

            _logger.LogInformation(
                "Mentor {MentorId} removed mentee {MenteeId} ({Name}) from program {ProgramId}",
                mentorProfileId, menteeId, fullName, programId);

            return ApiResponse<RemoveMenteeResponseDto>.SuccessResponse(
                new RemoveMenteeResponseDto
                {
                    MenteeId = menteeId,
                    FullName = fullName,
                    Message  = $"{fullName} has been removed from the program successfully."
                },
                "Mentee removed successfully");
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();
            _logger.LogError(ex,
                "Error removing mentee {MenteeId} from program {ProgramId}", menteeId, programId);
            return ApiResponse<RemoveMenteeResponseDto>.ErrorResponse(
                "Failed to remove mentee. Please try again.");
        }
    }
}