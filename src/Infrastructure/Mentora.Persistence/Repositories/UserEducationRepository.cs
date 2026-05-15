using Microsoft.EntityFrameworkCore;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities.Profiles;
namespace Mentora.Persistence.Repositories;

public class UserEducationRepository : IUserEducationRepository
{
    private readonly ApplicationDbContext _context;

    public UserEducationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserEducation>> GetByUserIdAsync(Guid userId) =>
        await _context.UserEducations
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.GraduationYear)
            .ToListAsync();

    public async Task<UserEducation?> GetByIdAsync(Guid educationId) =>
        await _context.UserEducations.FindAsync(educationId);

    public async Task CreateAsync(UserEducation education)
    {
        await _context.UserEducations.AddAsync(education);
    }

    public async Task UpdateAsync(UserEducation education)
    {
        _context.UserEducations.Update(education);
    }

    public async Task DeleteAsync(Guid educationId)
    {
        var education = await _context.UserEducations.FindAsync(educationId);
        if (education != null) _context.UserEducations.Remove(education);
    }
}