using Mentora.Application.DTOs.Applicants;
using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs.Application
{
    public class GetApplicantsRequestDto : GetAllApplicantsRequestDto
    {
        public int ProgramId { get; set; }
    }
}
