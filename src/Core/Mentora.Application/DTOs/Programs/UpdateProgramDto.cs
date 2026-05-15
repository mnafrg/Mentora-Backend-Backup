using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class UpdateProgramDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Availability { get; set; }
        public string? Duration { get; set; }
        public EducationStatus? EducationLevel { get; set; }
        public CurrentLevel? TargetLevel { get; set; }
        public string? ProgramImageUrl { get; set; }
        public ProgramPostStatus? Status { get; set; } 
        public DateTime? Deadline { get; set; } 
        public int? Capacity { get; set; }
        public int? DomainId { get; set; }
        public int? SubDomainId { get; set; }
        public int? RoadmapId { get; set; }
        public List<TechnologyRequirementDto> Technologies { get; set; } = new();
        public List<ProgramQuestionDto>? Questions { get; set; }
    }
}
