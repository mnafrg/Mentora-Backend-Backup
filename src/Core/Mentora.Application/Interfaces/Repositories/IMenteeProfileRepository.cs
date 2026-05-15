using Mentora.Application.DTOs.Auth;
using Mentora.Application.DTOs.Common;
using Mentora.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces.Repositories;

public interface IMenteeProfileRepository
{
    Task<MenteeProfile?> GetByUserIdAsync(Guid userId);
    Task<MenteeProfile> CreateAsync(MenteeProfile profile);
    Task<int> CountAsync(Expression<Func<MenteeProfile, bool>> filter = null);
    Task UpdateAsync(MenteeProfile profile);
}