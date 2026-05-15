namespace Mentora.Domain.Entities.Interactions;

public class Follow
{
    public Guid Id { get; set; }
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }
    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Follower { get; set; } =  null!;
    public User Following { get; set; } =  null!;
}