namespace Mentora.Domain.Enums.Interactions.Report;
public enum ReportedItemStatus
{
    Open        = 0,   // Below threshold or awaiting admin
    FlaggedForReview = 1, // Threshold reached – appears in admin queue
    ActionTaken = 2,   // Admin has resolved
    Cleared     = 3    // Admin dismissed all reports
}
 