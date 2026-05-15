using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Community;

public class CreateCommunityDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }

    [Range(1, int.MaxValue)]
    public int DomainId { get; set; }
}