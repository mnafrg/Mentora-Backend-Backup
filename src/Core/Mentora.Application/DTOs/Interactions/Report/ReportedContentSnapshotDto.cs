namespace Mentora.Application.DTOs.Interactions.Report;

/// Snapshot of whatever is being reported so the admin sees it in-context.
public class ReportedContentSnapshotDto
{
    /// For User: full name. For content: author name.
    public string Title       { get; set; } = null!;
 
    /// For content types: a preview of the body text (first 500 chars).
    public string? Body       { get; set; }
 
    /// URL to the content if applicable (post URL, event URL).
    public string? ContentUrl { get; set; }
 
    public DateTime CreatedAt { get; set; }
}
 