using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IMaterialRepository
    {
        Task<TopicMaterial?> GetByIdAsync(int id);
        Task<IEnumerable<TopicMaterial>> GetByTopicIdAsync(int topicId);

        void Add(TopicMaterial material);
        void Update(TopicMaterial material);
        void Delete(TopicMaterial material);
    }
}
