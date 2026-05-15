namespace Mentora.Domain.Entities.Profiles;

public class UserEducation
{
    public Guid EducationId { get; set; }
    public Guid UserId { get; set; }
    public string Institution { get; set; } = null!; // e.g Helwan Univesity
    public string? Faculty { get; set; } // e.g Computer Science
    public string? Degree { get; set; } // e.g Bachelor
    public int? StartYear { get; set; }
    public int? GraduationYear { get; set; }
    public int? DisplayOrder { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}