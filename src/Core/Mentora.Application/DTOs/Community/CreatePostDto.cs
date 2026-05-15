using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Community;

public class CreatePostDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }
}