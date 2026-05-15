using Mentora.Application.DTOs.Classroom;
using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces.Services.Classroom;

public interface IMaterialCompletionService
{
    /// Toggle the completion state of a material for the calling mentee.
    /// Returns true when the material is now marked complete, false when unmarked.

    Task<ApiResponse<MaterialCompletionResponseDto>> ToggleMaterialCompletionAsync(
        int materialId, Guid menteeProfileId);

    // Returns the completion status for every material in a roadmap for the given mentee.
    Task<ApiResponse<IEnumerable<MaterialCompletionResponseDto>>> GetMaterialCompletionsForRoadmapAsync(
        int roadmapId, Guid menteeProfileId);
}