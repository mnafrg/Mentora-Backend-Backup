using Mentora.Application.DTOs.Auth;
using Mentora.Application.DTOs.Common;
using Mentora.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories;

public interface IMentorProfileRepository
{
    Task<MentorProfile?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<MentorProfile>> GetAllWithUserAsync();
    Task<MentorProfile> CreateAsync(MentorProfile profile);
    Task<int> CountAsync(Expression<Func<MentorProfile, bool>> filter = null);
    Task UpdateAsync(MentorProfile profile);
}
