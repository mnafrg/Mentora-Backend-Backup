using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Common;
using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces
{
    public interface IApplicantsService
    {
        Task<ApiResponse<ProgramApplicantsResponseDto>> GetAllApplicantsAsync(GetAllApplicantsRequestDto request, Guid mentorProfileId);
        Task<ApiResponse<ProgramApplicantsResponseDto>> GetApplicantsByProgramAsync(GetApplicantsRequestDto request, Guid mentorProfileId);
        Task<ApiResponse<string>> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus, Guid mentorProfileId);
        Task<ApiResponse<string>> NotifyAllApplicantsAsync(int programId, Guid mentorProfileId);
        Task<byte[]> ExportApplicantsToExcelAsync(GetAllApplicantsRequestDto request, Guid mentorProfileId);
        Task<ApiResponse<List<MenteeApplicationDto>>> GetMenteeApplicationsAsync(Guid menteeProfileId);

        Task<ApiResponse<ApplicantProfileDetailsDto>> GetApplicantProfileAsync(int applicationId, Guid mentorId);
    }
}
    

