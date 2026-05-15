namespace Mentora.Application.DTOs.Profile;

public class EducationDto
{
    public Guid EducationId { get; set; }
    public string Institution { get; set; } = null!;
    public string? Faculty { get; set; }
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? GraduationYear { get; set; }
    public int DisplayOrder { get; set; }
}

public class AddEducationRequest
{
    public string Institution { get; set; } = null!;
    public string? Faculty { get; set; }
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? GraduationYear { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateEducationRequest
{
    public string Institution { get; set; } = null!;
    public string? Faculty { get; set; }
    public string? Degree { get; set; }
    public int? StartYear { get; set; }
    public int? GraduationYear { get; set; }
    public int DisplayOrder { get; set; }
}