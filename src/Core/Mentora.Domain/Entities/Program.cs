using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class Program
    {
        public int ProgramId { get; set; }
        public string Title { get; set; } = null!; 
        public string? Description { get; set; } 
        public string? Availability { get; set; } 
        public string? Duration { get; set; }
        public EducationStatus? EducationLevel { get; set; } 
        public CurrentLevel? TargetLevel { get; set; } 
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProgramPostStatus? ProgramPostStatus { get; set; }

        public string? ProgramImageUrl { get; set; }
        public DateTime Deadline { get; set; } 

        public Guid MentorProfileId { get; set; }
        public MentorProfile MentorProfile { get; set; } = null!;

        public int DomainId { get; set; }
        public SkillDomain? Domain { get; set; } 
        public int SubDomainId { get; set; }
        public SubDomain? SubDomain { get; set; }
        public int? RoadmapId { get; set; } 
        public Roadmap? Roadmap { get; set; }

        public ICollection<Technology>? Technologies { get; set; } = new List<Technology>();

        public ICollection<ProgramQuestion>? Questions { get; set; } = new List<ProgramQuestion>();
        public ICollection<MentorshipApplication> Applications { get; set; } = new List<MentorshipApplication>();
        public ICollection<Mentorship> Mentorships { get; set; } = new List<Mentorship>();
        public  ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public  ICollection<SavedPost> SavedByUsers { get; set; } = new List<SavedPost>();
        public  ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
        public  ICollection<SharedPost> Shares { get; set; } = new List<SharedPost>();
        public ICollection<MentorshipRequirement> MentorshipRequirements { get; set; } = new List<MentorshipRequirement>();
    }

}
