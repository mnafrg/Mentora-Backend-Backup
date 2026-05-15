using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.MenteeProfile)
                .Include(u => u.MentorProfile)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.MenteeProfile)
                .Include(u => u.MentorProfile)
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }
    public async Task<int> CountAsync(Expression<Func<User, bool>> filter = null)
    {
        if (filter != null)
        {
            return await _context.Users.CountAsync(filter);
        }
        return await _context.Users.CountAsync();
    }
    public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email.ToLower());
        }
    }