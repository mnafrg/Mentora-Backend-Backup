namespace Mentora.Application.DTOs.Community;

public class CreateCommunityDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int DomainId { get; set; }
}
