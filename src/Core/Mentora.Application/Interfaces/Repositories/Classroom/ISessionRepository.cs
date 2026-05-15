using Mentora.Domain.Entities.Classroom;
namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface ISessionRepository
{
    Task<ClassroomSession?> GetSessionByIdAsync(int sessionId);
 
    /// All sessions for a classroom, ordered by ScheduledAt ascending.
    Task<IEnumerable<ClassroomSession>> GetSessionsByClassroomIdAsync(int classroomId);
 
    /// Upcoming (not cancelled, ScheduledAt >= now) sessions, ordered by ScheduledAt.
    Task<IEnumerable<ClassroomSession>> GetUpcomingSessionsAsync(int classroomId);
 
    Task CreateSessionAsync(ClassroomSession session);
    Task UpdateSessionAsync(ClassroomSession session);
}