using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Community;

public class UpdatePostDto
{
    [MinLength(1)]
    [MaxLength(2000)]
    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }
}