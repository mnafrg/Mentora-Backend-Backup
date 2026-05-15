using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Dashboards.MenteeDashboard;
using Mentora.Application.DTOs.MenteeDashboard.Mentora.Application.DTOs.MenteeDashboard;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services.DashboardServices
{




        public class MenteeDashboardServices : IMenteeDashboardServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenteeDashboardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


      
     //public async Task<ApiResponse<IEnumerable<CalendarEventDto>>> GetUpcomingSessionsAsync(ClaimsPrincipal user , int count = 2)
     //   {
     //       var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
     //       if (string.IsNullOrEmpty(userIdClaim))
     //           return ApiResponse<IEnumerable<CalendarEventDto>>.ErrorResponse("User Not Found");

     //       var menteeId = Guid.Parse(userIdClaim);

     //       var sessions = await _unitOfWork.Sessions.GetUpcomingSessionsByMenteeIdAsync(menteeId, count);
     //       var result = sessions.Select(s => new CalendarEventDto
     //       {
     //           SessionId = s.SessionId, 
     //           SessionTitle = $"Session with {s.Mentorship.Program.MentorProfile.User.FirstName}",
     //           ProgramTitle = s.Mentorship.Program.Title,
     //           StartTime = s.ScheduledAt, 
     //           DurationInMinutes = s.Duration 
     //       }).OrderBy(s => s.StartTime).Take(2).ToList();

     //       return ApiResponse<IEnumerable<CalendarEventDto>>.SuccessResponse(result);
     //   }


    }
}
