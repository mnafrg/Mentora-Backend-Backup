using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Applicants
{
    public class MenteeApplicationDto
    {
       
            public int ApplicationId { get; set; }
            public string ProgramTitle { get; set; } = string.Empty;
            public string? ProgramDescription { get; set; } 
            public string? ProgramImageUrl { get; set; }    
            public DateTime AppliedAt { get; set; }
            public string Status { get; set; } = string.Empty;
            public string MentorName { get; set; } = string.Empty;
            public string MentorDomain { get; set; } = string.Empty;
            public string? MentorProfilePicture { get; set; }
        }
    }

