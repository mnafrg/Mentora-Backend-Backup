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
        public interface IProgramMenteeService
        {
            Task<ApiResponse<ProgramViewDto>> GetProgramViewAsync(int programId);
        Task<ApiResponse<IEnumerable<ProgramViewDto>>> GetSavedProgramsAsync(Guid userId);
        Task<ApiResponse<MentorCardDto>> GetMentorCardByProgramIdAsync(int programId);
        
            Task<ApiResponse<IEnumerable<ProgramQuestionDto>>> GetProgramQuestionsAsync(int programId);
        Task<ApiResponse<bool>> ToggleLikeProgramAsync(int programId, Guid menteeProfileId);
        Task<ApiResponse<bool>> ToggleSaveProgramAsync(int programId, Guid menteeProfileId);
        Task<ApiResponse<string>> GenerateShareLinkAsync(int programId,Guid senderId);
        Task<ApiResponse<ProgramViewDto>> GetProgramByShareLinkAsync(string encryptedLink);
        Task<ApiResponse<bool>> ApplyToProgramAsync(int programId, Guid menteeProfileId, CreateApplicationDto dto);

    }

    }

