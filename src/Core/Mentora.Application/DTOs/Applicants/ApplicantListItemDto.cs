using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Applicants
{
    public class ApplicantListItemDto
    {
        public int ApplicationId { get; set; }
        public string MenteeName { get; set; } = string.Empty;
        public string? MenteeProfilePicture { get; set; }
        public DateTime AppliedAt { get; set; }
        public string Level { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
