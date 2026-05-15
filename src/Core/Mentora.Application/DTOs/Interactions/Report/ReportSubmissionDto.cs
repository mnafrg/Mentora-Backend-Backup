namespace Mentora.Application.DTOs.Interactions.Report;

/// One individual submission inside the full context view.
public class ReportSubmissionDto
{
    public Guid     ReportId    { get; set; }
    public Guid     ReporterId  { get; set; }
    public string   ReporterName { get; set; } = null!;
    public string?  ReporterPictureUrl { get; set; }
    public string   Reason      { get; set; } = null!;
    public string?  Description { get; set; }
    public DateTime CreatedAt   { get; set; }
}
 