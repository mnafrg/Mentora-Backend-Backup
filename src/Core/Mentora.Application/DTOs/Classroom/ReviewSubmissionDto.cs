using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Classroom;

public class ReviewSubmissionDto
{
    /// Required for a final review. Null when requesting a revision.
    [Range(0, 100)]
    public int? Grade { get; set; }

    public string? Feedback { get; set; }

    /// When true: sends feedback only, resets status to Todo, no grade required.
    public bool RequestRevision { get; set; } = false;
}