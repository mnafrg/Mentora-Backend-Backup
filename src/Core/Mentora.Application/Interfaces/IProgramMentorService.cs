using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces
{
    public interface IProgramMentorService
    {
        Task<ApiResponse<CreateProgramResponseDto>> CreateProgramAsync(CreateProgramٌRequestDto dto, Guid mentorProfileId);
        Task<ApiResponse<IEnumerable<ProgramResponseDto>>> GetAllDraftsAsync(Guid mentorProfileId);
        Task<ApiResponse<ProgramResponseDto>> GetProgramByIdAsync(int programId, Guid mentorProfileId);
        Task<ApiResponse<IEnumerable<ProgramResponseDto>>> GetAllPublishedAsync(Guid mentorProfileId);
        Task<ApiResponse<ProgramResponseDto>> UpdateProgramAsync(int programId, UpdateProgramDto dto , Guid mentorProfileId);
    }
}
