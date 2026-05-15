using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Dashboards.MentorDashboard
{
    public class ProgramStatsDto
    {
        public int ProgramId { get; set; }
        public string ProgramTitle { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SavesCount { get; set; }
        public int SharesCount { get; set; }
    }
}
