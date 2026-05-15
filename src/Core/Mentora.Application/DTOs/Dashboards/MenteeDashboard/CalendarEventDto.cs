using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.MenteeDashboard
{
    namespace Mentora.Application.DTOs.MenteeDashboard
    {
        public class CalendarEventDto
        {
            public Guid SessionId { get; set; } 
            public string SessionTitle { get; set; } =null!;
            public string ProgramTitle { get; set; }  = null!;
            public DateTime StartTime { get; set; }
            public double DurationInMinutes { get; set; } 
            public string DateDisplay => StartTime.Date == DateTime.Today.AddDays(1)
                                         ? "Tomorrow"
                                         : StartTime.ToString("MMM dd");

            public string TimeDisplay => StartTime.ToString("h tt").ToLower(); 
        }
    }
}
