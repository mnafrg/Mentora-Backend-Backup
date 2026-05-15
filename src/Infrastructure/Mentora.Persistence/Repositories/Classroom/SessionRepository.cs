using Mentora.Domain.Entities.Classroom;
using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Enums.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;
 
    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
 
    public async Task<ClassroomSession?> GetSessionByIdAsync(int sessionId) =>
        await _context.ClassroomSessions
            .Include(s => s.ClassRoom)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
 
    public async Task<IEnumerable<ClassroomSession>> GetSessionsByClassroomIdAsync(int classroomId) =>
        await _context.ClassroomSessions
            .Where(s => s.ClassroomId == classroomId)
            .OrderBy(s => s.ScheduledAt)
            .ToListAsync();
 
    public async Task<IEnumerable<ClassroomSession>> GetUpcomingSessionsAsync(int classroomId) =>
        await _context.ClassroomSessions
            .Where(s =>
                s.ClassroomId == classroomId &&
                s.Status != ClassroomSessionStatus.Cancelled &&
                s.Status != ClassroomSessionStatus.Completed &&
                s.ScheduledAt >= DateTime.UtcNow)
            .OrderBy(s => s.ScheduledAt)
            .ToListAsync();
 
    public async Task CreateSessionAsync(ClassroomSession session)
    {
        await _context.ClassroomSessions.AddAsync(session);
    }
 
    public Task UpdateSessionAsync(ClassroomSession session)
    {
        _context.ClassroomSessions.Update(session);
        return Task.CompletedTask;
    }
}