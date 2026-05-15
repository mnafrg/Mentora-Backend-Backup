namespace Mentora.Application.DTOs.Community;

public class UpdateCommunityDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int? DomainId { get; set; }
}
