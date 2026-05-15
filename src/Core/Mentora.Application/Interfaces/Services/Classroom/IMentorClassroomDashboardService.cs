using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface IMentorClassroomDashboardService
{
    /// Returns all classroom analytics for the given program.
    /// Only the mentor who owns the program may call this.
    Task<ApiResponse<MentorClassroomDashboardDto>> GetDashboardAsync(
        int programId, Guid mentorProfileId);

    /// Removes a mentee from the program (sets their application to Rejected)
    /// and deactivates their Mentorship record if one exists.
    /// Only the mentor who owns the program may call this.
    Task<ApiResponse<RemoveMenteeResponseDto>> RemoveMenteeFromProgramAsync(
        int programId, Guid menteeId, Guid mentorProfileId);
}