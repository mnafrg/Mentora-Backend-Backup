using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class ProgramResponseDto
    {
        public int ProgramId { get; set; } 
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ProgramImageUrl { get; set; }

        public string? Availability { get; set; } = null!;
        public string? Duration { get; set; } = null!;
        public string? EducationLevel { get; set; } 

        public string? TargetLevel { get; set; }
        public int Capacity { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public string?  DomainName {get; set; }
        public string? SubDomainName{ get; set; }
        public int? RoadmapId { get; set; }

        public List<TechnologyRequirementDto> Technologies { get; set; } = new();
        public List<ProgramQuestionDto>? Questions { get; set; } = new();
    }
}
