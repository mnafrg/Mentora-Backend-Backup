using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories
{
    public interface ISavedRoadmapRepository
    {
        Task<SavedRoadmap?> GetAsync(Expression<Func<SavedRoadmap, bool>> filter);

        Task<IEnumerable<SavedRoadmap>> GetAllAsync(
            Expression<Func<SavedRoadmap, bool>> filter,
            params Expression<Func<SavedRoadmap, object>>[] includes);

        Task<SavedRoadmap> CreateAsync(SavedRoadmap save);

        Task DeleteAsync(SavedRoadmap save);
    }
}