namespace Mentora.Domain.Entities.Interactions.Report;
public class UserBan
{
    public Guid BanId { get; set; }
    public Guid UserId { get; set; }

    // Null = permanent ban.
    // DateTime = temporary ban that expires at this UTC time.
    public DateTime? ExpiresAt { get; set; }
    public bool IsPermanent { get; set; }
    public bool IsRevoked   { get; set; }   // admin can manually lift ban early
    public string? Reason { get; set; }

    //Links to ReportedItem that triggered this ban
    public Guid? ReportedItemId { get; set; }
    public Guid IssuedBy   { get; set; }    // admin userId
    public DateTime IssuedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? RevokedBy { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}