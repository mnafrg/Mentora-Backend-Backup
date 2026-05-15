using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class TopicDto
    {
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
      

        public List<MaterialDto> Materials { get; set; } = new();

        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}

