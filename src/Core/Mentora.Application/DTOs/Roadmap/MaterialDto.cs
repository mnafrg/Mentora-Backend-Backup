using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Roadmap
{
    public class MaterialDto
    {
        public int?   Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public MaterialType MaterialType { get; set; } 
    }
}
