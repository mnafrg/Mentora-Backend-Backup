using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{


    public interface IRoadmapRepository
    {

        Task<Roadmap?> GetByIdAsync(int id);
        Task<IEnumerable<Roadmap>> GetAllBasicInfoAsync();
        Task<Roadmap?> GetByIdWithFullHierarchyAsync(int id);
        Task<IEnumerable<Roadmap>> GetAllRoadmapsFullHierarchyAsync(Guid mentorId);
        Task<IEnumerable<Roadmap>> GetByMentorIdAsync(Guid mentorId);
        Task AddAsync(Roadmap roadmap);
        void Update(Roadmap roadmap);
        void Delete(Roadmap roadmap);
    }
}
