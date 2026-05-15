using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Dashboards.MentorDashboard
{
    public class MentorOverallStatsDto
    {
        public int TotalPrograms { get; set; }
        public int TotalMentees { get; set; } 
        public double AverageRating { get; set; } 
    }
}
