using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class PostLike
    {
        public int LikeId { get; set; } 
        public int ProgramId { get; set; }
        public Program Program { get; set; } =null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } 
    }
}

