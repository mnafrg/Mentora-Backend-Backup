using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface ISessionService
{
    Task<ApiResponse<SessionDto>> CreateSessionAsync(int programId, Guid mentorProfileId, CreateSessionRequest dto);
    Task<ApiResponse<SessionDto>> UpdateSessionAsync(int sessionId, Guid mentorProfileId, UpdateSessionRequest dto);
    Task<ApiResponse<bool>> CancelSessionAsync(int sessionId, Guid mentorProfileId);
 
    //  Sessions (shared – mentor + enrolled mentees)
 
    /// <summary>Full list of future sessions (Upcoming / Live) sorted by date asc.</summary>
    Task<ApiResponse<IEnumerable<SessionDto>>> GetFutureSessionsAsync(int programId, Guid requesterId);
 
    /// The single next upcoming/live session – the "join" card shown at the top
    /// of the classroom dashboard.
    Task<ApiResponse<UpcomingSessionDto>> GetNextUpcomingSessionAsync(int programId, Guid requesterId);
}