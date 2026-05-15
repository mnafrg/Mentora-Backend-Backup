using Mentora.Domain.Enums.Classroom;
using Mentora.Domain.Entities.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Classroom;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services.Classroom;

public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClassroomService> _logger;

    // A session is considered "joinable" when it starts within this window.
    private static readonly TimeSpan JoinableWindow = TimeSpan.FromMinutes(15);
 
    public SessionService(IUnitOfWork unitOfWork, ILogger<ClassroomService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ApiResponse<SessionDto>> CreateSessionAsync(
        int programId, Guid mentorProfileId, CreateSessionRequest dto)
    {
        // Verify ownership
        var program = await _unitOfWork.Programs.GetByIdAsync(programId);
        if (program == null || program.MentorProfileId != mentorProfileId)
            return ApiResponse<SessionDto>.ErrorResponse("Program not found or access denied");
 
        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<SessionDto>.ErrorResponse("Classroom not found for this program");
 
        if (dto.ScheduledAt <= DateTime.UtcNow)
            return ApiResponse<SessionDto>.ErrorResponse("Session must be scheduled in the future");
 
        var session = new ClassroomSession
        {
            ClassroomId     = classroom.ClassroomId,
            Title           = dto.Title.Trim(),
            MeetingLink     = dto.MeetingLink?.Trim(),
            ScheduledAt     = dto.ScheduledAt.ToUniversalTime(),
            Status          = ClassroomSessionStatus.Upcoming,
            CreatedAt       = DateTime.UtcNow
        };
 
        await _unitOfWork.Sessions.CreateSessionAsync(session);
        await _unitOfWork.SaveChangesAsync();
 
        _logger.LogInformation(
            "Session created: SessionId={SessionId}, ClassroomId={ClassroomId}",
            session.SessionId, classroom.ClassroomId);
 
        return ApiResponse<SessionDto>.SuccessResponse(
            MapSessionToDto(session), "Session created successfully");
    }
 
    public async Task<ApiResponse<SessionDto>> UpdateSessionAsync(
        int sessionId, Guid mentorProfileId, UpdateSessionRequest dto)
    {
        var (session, error) = await GetSessionAndVerifyOwnershipAsync(sessionId, mentorProfileId);
        if (session == null)
            return ApiResponse<SessionDto>.ErrorResponse(error!);
 
        if (session.Status == ClassroomSessionStatus.Cancelled)
            return ApiResponse<SessionDto>.ErrorResponse("Cannot update a cancelled session");
 
        if (session.Status == ClassroomSessionStatus.Completed)
            return ApiResponse<SessionDto>.ErrorResponse("Cannot update a completed session");
 
        if (dto.ScheduledAt.HasValue && dto.ScheduledAt.Value <= DateTime.UtcNow)
            return ApiResponse<SessionDto>.ErrorResponse("Session must be scheduled in the future");
 
        // Apply partial updates
        if (dto.Title != null)           session.Title           = dto.Title.Trim();
        if (dto.MeetingLink != null)     session.MeetingLink     = dto.MeetingLink.Trim();
        if (dto.ScheduledAt.HasValue)    session.ScheduledAt     = dto.ScheduledAt.Value.ToUniversalTime();
 
        session.UpdatedAt = DateTime.UtcNow;
 
        await _unitOfWork.Sessions.UpdateSessionAsync(session);
        await _unitOfWork.SaveChangesAsync();
 
        return ApiResponse<SessionDto>.SuccessResponse(
            MapSessionToDto(session), "Session updated successfully");
    }
 
    public async Task<ApiResponse<bool>> CancelSessionAsync(int sessionId, Guid mentorProfileId)
    {
        var (session, error) = await GetSessionAndVerifyOwnershipAsync(sessionId, mentorProfileId);
        if (session == null)
            return ApiResponse<bool>.ErrorResponse(error!);
 
        if (session.Status == ClassroomSessionStatus.Cancelled)
            return ApiResponse<bool>.ErrorResponse("Session is already cancelled");
 
        if (session.Status == ClassroomSessionStatus.Completed)
            return ApiResponse<bool>.ErrorResponse("Cannot cancel a completed session");
 
        session.Status    = ClassroomSessionStatus.Cancelled;
        session.UpdatedAt = DateTime.UtcNow;
 
        await _unitOfWork.Sessions.UpdateSessionAsync(session);
        await _unitOfWork.SaveChangesAsync();
 
        return ApiResponse<bool>.SuccessResponse(true, "Session cancelled successfully");
    }
 
    // Sessions – Shared (mentor + enrolled mentees)
 
    public async Task<ApiResponse<IEnumerable<SessionDto>>> GetFutureSessionsAsync(
        int programId, Guid requesterId)
    {
        if (!await RequesterHasAccessAsync(programId, requesterId))
            return ApiResponse<IEnumerable<SessionDto>>.ErrorResponse("Access denied");
 
        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<IEnumerable<SessionDto>>.ErrorResponse("Classroom not found");
 
        var sessions = await _unitOfWork.Sessions.GetUpcomingSessionsAsync(classroom.ClassroomId);
        var dtos     = sessions.Select(MapSessionToDto);
 
        return ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(
            dtos, "Future sessions retrieved successfully");
    }
 
    public async Task<ApiResponse<UpcomingSessionDto>> GetNextUpcomingSessionAsync(
        int programId, Guid requesterId)
    {
        if (!await RequesterHasAccessAsync(programId, requesterId))
            return ApiResponse<UpcomingSessionDto>.ErrorResponse("Access denied");
 
        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<UpcomingSessionDto>.ErrorResponse("Classroom not found");
 
        var sessions = await _unitOfWork.Sessions.GetUpcomingSessionsAsync(classroom.ClassroomId);
        var next     = sessions.FirstOrDefault();
 
        if (next == null)
            return ApiResponse<UpcomingSessionDto>.ErrorResponse("No upcoming sessions");
 
        var now       = DateTime.UtcNow;
        var isJoinable = next.Status == ClassroomSessionStatus.Live
            || (next.ScheduledAt - now) <= JoinableWindow && next.ScheduledAt >= now;
 
        var dto = new UpcomingSessionDto
        {
            SessionId       = next.SessionId,
            Title           = next.Title,
            MeetingLink     = next.MeetingLink,
            ScheduledAt     = next.ScheduledAt,
            IsJoinable      = isJoinable
        };
 
        return ApiResponse<UpcomingSessionDto>.SuccessResponse(dto);
    }
 
    // ─────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// Returns true if the requester is either the mentor who owns the program
    /// OR a mentee whose application to that program was accepted.
    private async Task<bool> RequesterHasAccessAsync(int programId, Guid requesterId)
    {
        var program = await _unitOfWork.Programs.GetByIdAsync(programId);
        if (program == null) return false;
 
        // Mentor who owns the program always has access
        if (program.MentorProfileId == requesterId) return true;
 
        // Mentee must have an accepted application
        var applications = await _unitOfWork.MentorshipApplications.GetAllAsync(
            a => a.ProgramId       == programId
              && a.MenteeProfileId == requesterId
              && a.Status          == Domain.Enums.ApplicationStatus.Accepted);
 
        return applications.Any();
    }
 
    /// Retrieves a session and verifies that the caller is the mentor who owns the program.
    /// Returns (null, errorMessage) on failure.

    private async Task<(ClassroomSession? session, string? error)> GetSessionAndVerifyOwnershipAsync(
        int sessionId, Guid mentorProfileId)
    {
        var session = await _unitOfWork.Sessions.GetSessionByIdAsync(sessionId);
        if (session == null)
            return (null, "Session not found");
 
        // Navigate: session → classroom → program → mentor
        var program = await _unitOfWork.Programs.GetByIdAsync(session.ClassRoom.ProgramId);
        if (program == null || program.MentorProfileId != mentorProfileId)
            return (null, "Access denied");
 
        return (session, null);
    }
 
    // Mapping helpers
    private static SessionDto MapSessionToDto(ClassroomSession s) => new()
    {
        SessionId       = s.SessionId,
        ClassroomId     = s.ClassroomId,
        Title           = s.Title,
        MeetingLink     = s.MeetingLink,
        ScheduledAt     = s.ScheduledAt,
        Status          = s.Status.ToString(),
        CreatedAt       = s.CreatedAt
    };
}