using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Applicants
{
    public class GetAllApplicantsRequestDto
    {
        public string? Search { get; set; }
        public ApplicationStatus? Status { get; set; }
        public CurrentLevel? Level { get; set; }
        public bool IsExport { get; set; } = false;
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
