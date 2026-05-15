namespace Mentora.Application.DTOs.Profile;

public class ProfileLinkDTO
{
    public Guid LinkId { get; set; }
    public string Label { get; set; } = null!; // e.g. LinkedIn, GitHub
    public string URL { get; set; } = null!;
    public int DisplayOrder { get; set; }
}

public class AddLinkRequest
{
    public string Label { get; set; } = null!; // e.g. LinkedIn, GitHub
    public string URL { get; set; } = null!;
    public int DisplayOrder { get; set; } = 0;
}

public class UpdateLinkRequest
{
    public string Label { get; set; } = null!; // e.g. LinkedIn, GitHub
    public string URL { get; set; } = null!;
    public int DisplayOrder { get; set; }
}