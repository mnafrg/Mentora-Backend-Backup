using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities.Profiles;

public class ProfileLink
{
    public Guid LinkId { get; set; }
    public Guid UserId { get; set; }
    public string Label { get; set; } = null!; // e.g. LinkedIn, GitHub
    public string URL { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
}