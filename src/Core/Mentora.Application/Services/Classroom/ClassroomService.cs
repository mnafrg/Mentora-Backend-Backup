using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Classroom;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services.Classroom;

public class ClassroomService : IClassroomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClassroomService> _logger;
 
    public ClassroomService(IUnitOfWork unitOfWork, ILogger<ClassroomService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ClassRoom?> CreateClassroomForProgramAsync(int programId, string programTitle, string? programDescription = null)
    {
        // Guard: do not create a duplicate classroom for the same program.
        var existing = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (existing != null)
        {
            _logger.LogWarning(
                "Classroom already exists for ProgramId={ProgramId}. Skipping creation.", programId);
            return existing;
        }
 
        var classroom = new ClassRoom
        {
            ProgramId   = programId,
            Title       = programTitle,
            Description = programDescription,
            CreatedAt   = DateTime.UtcNow,
            IsActive    = true
        };
 
        await _unitOfWork.Classrooms.CreateAsync(classroom);
        await _unitOfWork.SaveChangesAsync();
 
        _logger.LogInformation(
            "Classroom created for ProgramId={ProgramId}, ClassroomId={ClassroomId}",
            programId, classroom.ClassroomId);
 
        return classroom;
    }
 
    public async Task<ApiResponse<ClassroomDto>> GetClassroomByProgramIdAsync(
        int programId, Guid requesterId)
    {
        if (!await RequesterHasAccessAsync(programId, requesterId))
            return ApiResponse<ClassroomDto>.ErrorResponse("Access denied");
 
        var classroom = await _unitOfWork.Classrooms.GetByProgramIdAsync(programId);
        if (classroom == null)
            return ApiResponse<ClassroomDto>.ErrorResponse("Classroom not found");
 
        return ApiResponse<ClassroomDto>.SuccessResponse(MapToDto(classroom));
    }
    
    // Mapping helpers
    private static ClassroomDto MapToDto(ClassRoom c) => new()
    {
        ClassroomId = c.ClassroomId,
        ProgramId   = c.ProgramId,
        Title       = c.Title,
        Description = c.Description,
        IsActive    = c.IsActive,
        CreatedAt   = c.CreatedAt
    };
 
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
 
}