using Mentora.Domain.Entities.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface IClassroomService
{
    /// Called internally when a Program is created.
    /// Creates a Classroom linked to that Program.
    Task<ClassRoom?> CreateClassroomForProgramAsync(int programId, string programTitle, string? programDescription = null);
 
    /// Get classroom details by program id (accessible to enrolled mentees + the mentor).
    Task<ApiResponse<ClassroomDto>> GetClassroomByProgramIdAsync(int programId, Guid requesterId);
 
}