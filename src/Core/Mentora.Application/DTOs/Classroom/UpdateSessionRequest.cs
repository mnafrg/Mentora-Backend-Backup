using System.ComponentModel.DataAnnotations;
 
namespace Mentora.Application.DTOs.Classroom;

public class UpdateSessionRequest
{
    [MaxLength(200)]
    public string? Title { get; set; }
 
    [MaxLength(500)]
    public string? MeetingLink { get; set; }
 
    public DateTime? ScheduledAt { get; set; }

}