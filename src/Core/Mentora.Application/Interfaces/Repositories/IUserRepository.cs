using Mentora.Domain.Entities;
using System.Linq.Expressions;

namespace Mentora.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByEmailAsync(string email);
  
    Task<User> CreateAsync(User user);
    Task<int> CountAsync(Expression<Func<User, bool>> filter = null);
    Task UpdateAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}