using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface IRoadmapTaskRepository
    {
        Task<TopicTask?> GetByIdAsync(int id);
        Task<IEnumerable<TopicTask>> GetByTopicIdAsync(int topicId);

        void Add(TopicTask task);
        void Update(TopicTask task);
        void Delete(TopicTask task);
    }
}
