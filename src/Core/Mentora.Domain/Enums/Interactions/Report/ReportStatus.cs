namespace Mentora.Domain.Enums.Interactions.Report;

// Lifecycle state of a single report submission
public enum ReportStatus
{
    Pending     = 0,   // Just submitted, not yet visible to admin
    UnderReview = 1,   // Threshold reached, admin is reviewing
    Resolved    = 2,   // Admin took action
    Dismissed   = 3    // Admin found no violation
}
 