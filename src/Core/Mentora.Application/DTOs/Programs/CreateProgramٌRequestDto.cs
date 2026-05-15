using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Programs
{
    public class CreateProgramٌRequestDto
    {


        
        
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int DomainId { get; set; } 

        [Required]
        public int SubDomainId { get; set; } 

        [Required]
        public CurrentLevel TargetLevel { get; set; } 
         
        [Required]
        public EducationStatus EducationLevel { get; set; } 

        [Range(1, 1000)]
        public int Capacity { get; set; }

        public string Duration { get; set; } = null!;

        public string Availability { get; set; } = null!;

        public string? ProgramImageUrl { get; set; }

        public DateTime Deadline { get; set; }
        public List<TechnologyRequirementDto> Technologies { get; set; } = new();


        [Required]
        public ProgramPostStatus Status { get; set; }
        public int? RoadmapId { get; set; }



        public List<ProgramQuestionDto?> Questions { get; set; } = new();

    
    }
}
