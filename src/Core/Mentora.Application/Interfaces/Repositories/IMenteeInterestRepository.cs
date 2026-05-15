using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IMenteeInterestRepository
    {
        Task<IEnumerable<MenteeInterest>> GetByMenteeProfileIdAsync(Guid menteeProfileId);
    }
}
