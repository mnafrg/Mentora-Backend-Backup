using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class MentorCardDto
    {
        public Guid MentorProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string Bio { get; set; } = string.Empty;
    }
}