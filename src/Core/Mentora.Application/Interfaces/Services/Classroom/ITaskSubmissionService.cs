using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;
using Mentora.Domain.Enums.Classroom;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface ITaskSubmissionService
{
    // ── Mentee ────────────────────────────────────────────────────────────────
    Task<ApiResponse<SubmissionResponseDto>> CreateSubmissionAsync(
        int taskId, Guid menteeProfileId, CreateSubmissionDto dto);

    Task<ApiResponse<SubmissionResponseDto>> UpdateSubmissionAsync(
        int submissionId, Guid menteeProfileId, UpdateSubmissionDto dto);

    Task<ApiResponse<bool>> DeleteSubmissionAsync(
        int submissionId, Guid menteeProfileId);

    Task<ApiResponse<SubmissionResponseDto>> GetMySubmissionAsync(
        int taskId, Guid menteeProfileId);

    /// Returns all tasks in a phase with the mentee's personal status.
    Task<ApiResponse<IEnumerable<MenteeTaskStatusDto>>> GetMenteeTasksForPhaseAsync(
        int phaseId, Guid menteeProfileId, string? statusFilter = null);

    // ── Mentor ────────────────────────────────────────────────────────────────
    Task<ApiResponse<IEnumerable<SubmissionResponseDto>>> GetSubmissionsForProgramAsync(
        int programId, Guid mentorProfileId, SubmissionStatus? statusFilter = null);

    Task<ApiResponse<IEnumerable<SubmissionResponseDto>>> GetSubmissionsForRoadmapAsync(
        int roadmapId, Guid mentorProfileId, SubmissionStatus? statusFilter = null);

    Task<ApiResponse<SubmissionResponseDto>> ReviewSubmissionAsync(
        int submissionId, Guid mentorProfileId, ReviewSubmissionDto dto);

    // ── Analytics ────────────────────────────────────────────────────────────
    Task<ApiResponse<IEnumerable<TaskRegistryDto>>> GetTaskRegistryAsync(
        int programId, Guid mentorProfileId);
}