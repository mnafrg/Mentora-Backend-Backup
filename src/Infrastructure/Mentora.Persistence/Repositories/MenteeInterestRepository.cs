using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
    public class MenteeInterestRepository : IMenteeInterestRepository
    {
        private readonly ApplicationDbContext _context;

        public MenteeInterestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenteeInterest>> GetByMenteeProfileIdAsync(Guid menteeProfileId)
        {
            return await _context.MenteeInterests
                .Where(i => i.UserId == menteeProfileId)
                .ToListAsync();
        }
    }
}
