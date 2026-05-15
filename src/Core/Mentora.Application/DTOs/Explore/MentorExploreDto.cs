using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Explore
{
    public class MentorExploreDto
    {
        public Guid MentorId { get; set; }
        public string FullName { get; set; }
         public string DomainName { get; set; }
      
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public decimal? Rating { get; set; }
    }

}
