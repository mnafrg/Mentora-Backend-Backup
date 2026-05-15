using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Dashboards.MentorDashboard;
using Mentora.Application.DTOs.Programs;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces
{
    public interface IMentorDashboardServices
    {
        //Task<ApiResponse<MentorOverallStatsDto>> GetOverallStatsAsync(ClaimsPrincipal user);
        Task<ApiResponse<IEnumerable<ApplicantListItemDto>>> GetRecentApplicantsAsync(Guid mentorProfileId);


    }

}
