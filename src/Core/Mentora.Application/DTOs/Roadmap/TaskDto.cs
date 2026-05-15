using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class TaskDto
    {
        public int? Id { get; set; }
        public string Title { get; set; } =string.Empty;
        public string AttachmentUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DeadLine { get; set; }
    }
}
