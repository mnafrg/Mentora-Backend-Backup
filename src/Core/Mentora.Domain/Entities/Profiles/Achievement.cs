namespace Mentora.Domain.Entities.Profiles;

public class Achievement
{
    public int AchievementId { get; set; }
    public string Name { get; set; } = null!; // e.g. "Top Mentor of the Year"
    public string? Description { get; set; }
     
    // Navigation property
    public ICollection<UserAchievement> UserAchievements { get; set; } =  new List<UserAchievement>();
}

public class UserAchievement 
{
    public Guid UserId {get; set;}
    public int AchievementId {get; set;}
    public DateTime DateAchieved {get; set;}

    // Navigation properties
    public User User { get; set; } = null!;
    public Achievement Achievement { get; set; } = null!;
}