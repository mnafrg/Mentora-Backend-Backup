namespace Mentora.Application.DTOs.Interactions.Report;

/// One entry in the owner's moderation history.
public class UserActionHistoryDto
{
    public string   ActionType  { get; set; } = null!;  // Warning | TemporaryBan | PermanentBan
    public string?  Message     { get; set; }
    public DateTime IssuedAt    { get; set; }
    public DateTime? ExpiresAt  { get; set; }
    public bool     IsActive    { get; set; }
}