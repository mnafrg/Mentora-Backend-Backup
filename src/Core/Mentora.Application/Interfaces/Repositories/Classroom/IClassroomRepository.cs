using Mentora.Domain.Entities.Classroom;
namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface IClassroomRepository
{
    Task<ClassRoom?> GetByProgramIdAsync(int programId);
    Task<ClassRoom?> GetByIdAsync(int classroomId);
    Task CreateAsync(ClassRoom classroom);
 
}