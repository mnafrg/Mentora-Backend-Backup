using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Auth;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Dashboards.MentorDashboard;
using Mentora.Application.DTOs.Programs;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services.DashboardServices
{
    public class MentorDashboardServices : IMentorDashboardServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public MentorDashboardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ApiResponse<MentorOverallStatsDto>> GetOverallStatsAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))

                return ApiResponse<MentorOverallStatsDto>.ErrorResponse("User not found");

            var mentorId = Guid.Parse(userIdClaim);
            var totalPrograms = await _unitOfWork.Programs
                .CountAsync(p => p.MentorProfileId == mentorId);


            var totalMentees = await _unitOfWork.Mentorships.GetTotalUniqueMenteesByMentorIdAsync(mentorId);

            var averageRating = await _unitOfWork.Feedbacks
           .GetMentorAverageRatingAsync(mentorId);

            var mentorOverallStatsDto = new MentorOverallStatsDto
            {
                TotalPrograms = totalPrograms,
                TotalMentees = totalMentees,
                AverageRating = Math.Round(averageRating, 1)
            };
            return ApiResponse<MentorOverallStatsDto>.SuccessResponse(mentorOverallStatsDto);
        }
        public async Task<ApiResponse<IEnumerable<ApplicantListItemDto>>> GetRecentApplicantsAsync(Guid mentorProfileId)
        {
            var applications = await _unitOfWork.MentorshipApplications.GetAllAsync(
                a => a.Program.MentorProfileId == mentorProfileId,
                a => a.MenteeProfile.User,
                a => a.Program
            );

            if (applications == null)
                return ApiResponse<IEnumerable<ApplicantListItemDto>>.ErrorResponse("No applications found");

            var recent = applications
                .OrderByDescending(a => a.AppliedAt)
                .Take(3)
                .Select(a => new ApplicantListItemDto
                {
                    ApplicationId = a.ApplicationId,
                    MenteeName = a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName,
                    MenteeProfilePicture = a.MenteeProfile?.ProfilePictureUrl,
                    AppliedAt = a.AppliedAt,
                    Level = a.MenteeProfile.CurrentLevel.ToString(),
                    ProgramName = a.Program?.Title,
                    Status = a.Status.ToString()
                }).ToList();
            return ApiResponse<IEnumerable<ApplicantListItemDto>>.SuccessResponse(recent, "Recent applicants retrieved successfully");
        }
    }
}
