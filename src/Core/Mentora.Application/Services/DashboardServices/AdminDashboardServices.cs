using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Dashboards.AdminDashboard;
using Mentora.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services.DashboardServices
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<ApiResponse<AdminStatsDto>> GetGlobalStatsAsync()
        //{
        //    var totalUsers = await _unitOfWork.Users.CountAsync();
        //    var totalMentors = await _unitOfWork.MentorProfiles.CountAsync();
        //    var totalMentees = await _unitOfWork.MenteeProfiles.CountAsync();
        //    var totalPrograms = await _unitOfWork.Programs.CountAsync();
        //   // var totalCommunities = await _unitOfWork.Communities.CountAsync();

       
        //  //  var pendingApprovals = await _unitOfWork.MentorProfiles.CountAsync(m => !m.IsVerified);

        //    var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    var newUsersThisMonth = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= startOfMonth);

        //    double growth = totalUsers > 0 ? (double)newUsersThisMonth / totalUsers * 100 : 0;

        //    var stats = new AdminStatsDto
        //    {
        //        TotalUsers = totalUsers,
        //        TotalMentors = totalMentors,
        //        TotalMentees = totalMentees,
        //        TotalPrograms = totalPrograms,
        //      //  TotalCommunities = totalCommunities,
        //      //  PendingMentorApprovals = pendingApprovals,
        //        NewUsersThisMonth = newUsersThisMonth,
        //        GrowthRate = Math.Round(growth, 1) 
        //    };

        //    return ApiResponse<AdminStatsDto>.SuccessResponse(stats);
        //}
    }
}
