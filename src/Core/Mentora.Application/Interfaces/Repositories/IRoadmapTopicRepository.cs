using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mentora.Domain.Entities;


namespace Mentora.Application.Interfaces.Repositories
{
    public interface IRoadmapTopicRepository
    {
        Task<Topic?> GetByIdAsync(int id);
        Task<IEnumerable<Topic>> GetByPhaseIdAsync(int phaseId);

        void Add(Topic topic);
        void Update(Topic topic);
        void Delete(Topic topic);
    }
}