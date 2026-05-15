namespace Mentora.Application.DTOs.Interactions.Report;

/// Full context view of a reported item — everything the admin needs to
/// make a decision on one screen.
public class ReportedItemDetailDto
{
    // ── Target info ───────────────────────────────────────────────────────────
    public Guid    ReportedItemId  { get; set; }
    public string  TargetType      { get; set; } = null!;
    public Guid    TargetId        { get; set; }
 
    /// Snapshot of the content being reported.
    /// For User: basic profile info.
    /// For Post / Event / Comment: content body snippet.
    /// Populated by the service based on TargetType.
    public ReportedContentSnapshotDto Content { get; set; } = null!;
 
    // ── Owner ─────────────────────────────────────────────────────────────────
    public Guid    OwnerUserId     { get; set; }
    public string  OwnerName       { get; set; } = null!;
    public string? OwnerPictureUrl { get; set; }
    public string  OwnerRole       { get; set; } = null!;
    public DateTime OwnerJoinedAt  { get; set; }
 
    // ── Scoring & status ──────────────────────────────────────────────────────
    public int     ReportScore     { get; set; }
    public int     ReportThreshold { get; set; }
    public string  Status          { get; set; } = null!;
    public DateTime CreatedAt      { get; set; }
    public DateTime UpdatedAt      { get; set; }
 
    // ── All individual report submissions ─────────────────────────────────────
    public List<ReportSubmissionDto> Reports { get; set; } = new();
 
    // ── Reason breakdown (for the admin to see patterns) ──────────────────────
    public Dictionary<string, int> ReasonBreakdown { get; set; } = new();
 
    // ── History: previous actions taken on this owner ────────────────────────
    public List<UserActionHistoryDto> OwnerHistory { get; set; } = new();
 
    // ── Current resolution (if already actioned) ─────────────────────────────
    public string? ContentAction  { get; set; }
    public string? UserAction     { get; set; }
    public string? AdminNotes     { get; set; }
    public DateTime? ResolvedAt   { get; set; }
}