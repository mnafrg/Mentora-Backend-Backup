using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class TopicMaterial
    {
        public int MaterialId { get; set; }

        public string Title { get; set; } = null!;
        public MaterialType MaterialType { get; set; }
        public string Url { get; set; } = null!;
        public int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;
    }

}
