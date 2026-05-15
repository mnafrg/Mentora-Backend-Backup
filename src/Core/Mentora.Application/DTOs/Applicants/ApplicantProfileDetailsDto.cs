using Mentora.Application.DTOs.Profile;
using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Application
{
    public class ApplicantProfileDetailsDto
    {
        public int ApplicationId { get; set; }

          public Guid MenteeProfileId { get; set; }
        public string MenteeName { get; set; } = string.Empty;
        public string? MenteeProfilePicture { get; set; }
        
        public string Level { get; set; } = string.Empty;

      
        public string Status { get; set; }  = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }

        public List<ApplicationQuestionDto> QuestionsAndAnswers { get; set; } = new();
    }
}

