using System.ComponentModel.DataAnnotations;
 
namespace Mentora.Application.DTOs.Classroom;

public class CreateSessionRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    
    [MaxLength(500)]
    public string? MeetingLink { get; set; }
 
    [Required(ErrorMessage = "ScheduledAt is required")]
    public DateTime ScheduledAt { get; set; }
 
}