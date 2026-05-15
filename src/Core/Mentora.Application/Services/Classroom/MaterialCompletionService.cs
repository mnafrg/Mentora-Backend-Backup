using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Entities.Classroom;
using Mentora.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Mentora.Application.Services.Classroom;

public class MaterialCompletionService : IMaterialCompletionService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<MaterialCompletionService> _logger;

    public MaterialCompletionService(IUnitOfWork uow, ILogger<MaterialCompletionService> logger)
    {
        _uow    = uow;
        _logger = logger;
    }

    public async Task<ApiResponse<MaterialCompletionResponseDto>> ToggleMaterialCompletionAsync(
        int materialId, Guid menteeProfileId)
    {
        // 1. Verify the material exists
        var material = await _uow.Materials.GetByIdAsync(materialId);
        if (material is null)
            return ApiResponse<MaterialCompletionResponseDto>.ErrorResponse("Material not found");

        // 2. Verify the mentee is enrolled in the program that contains this material
        // Walk: material → topic → phase → roadmap → program(s)
        var topic = await _uow.Topics.GetByIdAsync(material.TopicId);
        if (topic is null)
            return ApiResponse<MaterialCompletionResponseDto>.ErrorResponse("Associated topic not found");

        var phase = await _uow.RoadmapPhases.GetByIdAsync(topic.RoadmapPhaseId);
        if (phase is null)
            return ApiResponse<MaterialCompletionResponseDto>.ErrorResponse("Associated phase not found");

        // Find a program that uses this roadmap and has the mentee accepted
        var enrolledPrograms = await _uow.MentorshipApplications.GetAllAsync(
            a => a.MenteeProfileId == menteeProfileId &&
                 a.Status == ApplicationStatus.Accepted);

        // Among the enrolled programs, find one whose roadmap matches
        bool isEnrolled = false;
        foreach (var application in enrolledPrograms)
        {
            var program = await _uow.Programs.GetByIdAsync(application.ProgramId);
            if (program?.RoadmapId == phase.RoadmapId)
            {
                isEnrolled = true;
                break;
            }
        }

        if (!isEnrolled)
            return ApiResponse<MaterialCompletionResponseDto>.ErrorResponse(
                "You are not enrolled in the program containing this material");

        // 3. Toggle
        var existing = await _uow.MaterialCompletions.GetAsync(menteeProfileId, materialId);

        if (existing is null)
        {
            // First time touching this material — create as completed
            existing = new MaterialCompletion
            {
                MenteeId    = menteeProfileId,
                MaterialId  = materialId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };
            await _uow.MaterialCompletions.CreateAsync(existing);
        }
        else
        {
            // Flip the flag
            if (existing.IsCompleted)
            {
                existing.IsCompleted = false;
                existing.CompletedAt = null;
            }
            else
            {
                existing.IsCompleted = true;
                existing.CompletedAt = DateTime.UtcNow;
            }
            await _uow.MaterialCompletions.UpdateAsync(existing);
        }

        await _uow.SaveChangesAsync();

        _logger.LogInformation(
            "Mentee {MenteeId} toggled material {MaterialId} → IsCompleted={IsCompleted}",
            menteeProfileId, materialId, existing.IsCompleted);

        return ApiResponse<MaterialCompletionResponseDto>.SuccessResponse(
            Map(existing, material),
            existing.IsCompleted ? "Material marked as completed" : "Material unmarked");
    }

    // --- List for roadmap -------------------------------------------

    public async Task<ApiResponse<IEnumerable<MaterialCompletionResponseDto>>> GetMaterialCompletionsForRoadmapAsync(
        int roadmapId, Guid menteeProfileId)
    {
        var completions = await _uow.MaterialCompletions
            .GetByMenteeAndRoadmapAsync(menteeProfileId, roadmapId);

        // Build a dictionary for O(1) lookup
        var completionMap = completions.ToDictionary(c => c.MaterialId);

        // Load the full roadmap to enumerate all materials
        var roadmap = await _uow.Roadmaps.GetByIdWithFullHierarchyAsync(roadmapId);
        if (roadmap is null)
            return ApiResponse<IEnumerable<MaterialCompletionResponseDto>>.ErrorResponse(
                "Roadmap not found");

        var result = new List<MaterialCompletionResponseDto>();
        foreach (var phase in roadmap.Phases)
        {
            foreach (var topic in phase.Topics)
            {
                foreach (var mat in topic.Materials)
                {
                    completionMap.TryGetValue(mat.MaterialId, out var completion);
                    result.Add(new MaterialCompletionResponseDto
                    {
                        MaterialId    = mat.MaterialId,
                        MaterialTitle = mat.Title,
                        MaterialUrl   = mat.Url,
                        MaterialType  = mat.MaterialType.ToString(),
                        IsCompleted   = completion?.IsCompleted ?? false,
                        CompletedAt   = completion?.CompletedAt
                    });
                }
            }
        }

        return ApiResponse<IEnumerable<MaterialCompletionResponseDto>>.SuccessResponse(result);
    }

    // -- Mapping --------------------------

    private static MaterialCompletionResponseDto Map(
        MaterialCompletion completion,
        Domain.Entities.TopicMaterial material) => new()
    {
        MaterialId    = material.MaterialId,
        MaterialTitle = material.Title,
        MaterialUrl   = material.Url,
        MaterialType  = material.MaterialType.ToString(),
        IsCompleted   = completion.IsCompleted,
        CompletedAt   = completion.CompletedAt
    };
}