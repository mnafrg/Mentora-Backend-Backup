using Mentora.Application.DTOs.Applicants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Application
{
    public class ProgramApplicantsResponseDto
    {
        public List<ApplicantListItemDto> Items { get; set; } = new();
        
        // Pagination
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        // Tabs Counters
        public int AllApplicantsCount { get; set; }
        public int PendingCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}
