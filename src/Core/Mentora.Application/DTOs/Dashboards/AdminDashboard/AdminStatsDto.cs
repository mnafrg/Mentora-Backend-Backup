using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Dashboards.AdminDashboard
{
    public class AdminStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalMentors { get; set; }
        public int TotalMentees { get; set; }
        public int TotalPrograms { get; set; }
        public int TotalCommunities { get; set; }

        public int NewUsersThisMonth { get; set; }
        public double GrowthRate { get; set; }
        public int PendingMentorApprovals { get; set; }
        public int FlaggedContentCount { get; set; }
    }
}
