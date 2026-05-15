using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Dashboards.MenteeDashboard
{
    public class SuggestedProgramDto
    {
        public int ProgramId { get; set; }
        public string Title { get; set; } =null!;
        public string MentorName { get; set; } = null!;
        public string MentorImageUrl { get; set; }  = null!;
        public string DomainName { get; set; } = null!;

        public int MatchPercentage { get; set; }

        public int Capacity { get; set; }
        public int CurrentEnrolledCount { get; set; }
        public bool IsFull => CurrentEnrolledCount >= Capacity;
    }
}
