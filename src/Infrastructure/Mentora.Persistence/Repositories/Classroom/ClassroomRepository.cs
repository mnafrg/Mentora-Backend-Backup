using Mentora.Domain.Entities.Classroom;
using Mentora.Application.Interfaces.Repositories.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class ClassroomRepository : IClassroomRepository
{
    private readonly ApplicationDbContext _context;
 
    public ClassroomRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ClassRoom?> GetByProgramIdAsync(int programId) =>
    await _context.Classrooms
        .Include(c => c.Sessions)
        .FirstOrDefaultAsync(c => c.ProgramId == programId);

    public async Task<ClassRoom?> GetByIdAsync(int classroomId) =>
        await _context.Classrooms
            .Include(c => c.Sessions)
            .FirstOrDefaultAsync(c => c.ClassroomId == classroomId);
 
    public async Task CreateAsync(ClassRoom classroom)
    {
        await _context.Classrooms.AddAsync(classroom);
    }
}