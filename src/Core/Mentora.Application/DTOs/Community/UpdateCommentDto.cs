using System.ComponentModel.DataAnnotations;

namespace Mentora.Application.DTOs.Community;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public string Content { get; set; } = null!;
}