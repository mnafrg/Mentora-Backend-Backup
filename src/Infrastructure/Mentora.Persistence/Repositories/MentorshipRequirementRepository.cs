using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
    public class MentorshipRequirementRepository : IMentorshipRequirementRepository
    {
        private readonly ApplicationDbContext _context;

        public MentorshipRequirementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MentorshipRequirement>> GetRequirementsByProgramIdAsync(int programId)
        {
            return await _context.MentorshipRequirements
                .Include(r => r.Technology) 
                .Where(r => r.ProgramId == programId)
                .ToListAsync();
        }
    }
}
