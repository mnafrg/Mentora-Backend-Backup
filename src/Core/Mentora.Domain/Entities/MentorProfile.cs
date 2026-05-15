namespace Mentora.Domain.Entities;

public class MentorProfile
{
    public Guid UserId { get; set; }
    public int DomainId { get; set; }
    public int YearsOfExperience { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? PastExperience { get; set; }
    public bool IsVerified { get; set; }
    public decimal? AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CountryCode { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? CvUrl { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public SkillDomain Domain { get; set; } = null!;
    public Country? Country { get; set; }

    // Relevant expertise (SubDomains) - Multi-select
    public ICollection<MentorSubDomain> MentorSubDomains { get; set; } = new List<MentorSubDomain>();
    public ICollection<MentorExpertise> MentorExpertises { get; set; } = new List<MentorExpertise>();
    public ICollection<Program> Programs { get; set; } = new List<Program>();
    public ICollection<Mentorship> Mentorships { get; set; } = new List<Mentorship>();
    public ICollection<Feedback> FeedbacksReceived { get; set; } = new List<Feedback>();
}