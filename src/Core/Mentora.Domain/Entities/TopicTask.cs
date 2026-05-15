using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
         public   class TopicTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string TaskAttachmentUrl { get; set; } = null!;
        public DateTime? DeadLine { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;

    }
}
