namespace Mentora.Application.DTOs.Community;

public class UpdatePostDto
{
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
}
