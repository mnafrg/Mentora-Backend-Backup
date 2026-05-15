using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class ListOfMaterialsDto
    {
        public int TopicId { get; set; }

        public List<MaterialDto> Materials { get; set; } = new();
    }
}
