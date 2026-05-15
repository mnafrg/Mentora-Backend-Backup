using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Persistence.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public ProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Program?> GetByIdAsync(int programId)
        {
            return await _context.Programs.FindAsync(programId);
        }


        public async Task<Program?> GetDraftProgramWithDetailsAsync(int programId, params string[] includes)
        {
            IQueryable<Program> query = _context.Programs;

           
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(p =>
                p.ProgramId == programId &&
                p.ProgramPostStatus == ProgramPostStatus.Draft);
        }
        public async Task<IEnumerable<Program>> GetAllAsync(
      Expression<Func<Program, bool>>? filter = null,
      params Expression<Func<Program, object>>[] includes)
        {
            IQueryable<Program> query = _context.Programs;

         
            if (filter != null)
               { query = query.Where(filter); }

            
            foreach (var include in includes)
                
              { query = query.Include(include); }

         
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Program>> GetProgramAsync(
    Expression<Func<Program, bool>>? filter = null,
    params string[] includes) 
        {
            IQueryable<Program> query = _context.Programs;
            if (filter != null) query = query.Where(filter);

            foreach (var includePath in includes)
                query = query.Include(includePath);

            return await query.ToListAsync();
        }
        public async Task<Program?> GetAsync(
            Expression<Func<Program, bool>> filter,
            params Expression<Func<Program, object>>[] includes)
        {
            IQueryable<Program> query = _context.Programs;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<int> CountAsync(Expression<Func<Program, bool>> filter = null)
        {
            IQueryable<Program> query = _context.Programs;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }


        public async Task<Program> CreateAsync(Program program)
        {
            await _context.Programs.AddAsync(program);
            return program;
        }

        public  Task UpdateAsync(Program program)
        {
            _context.Programs.Update(program);
            return Task.CompletedTask;
        }

        public  Task DeleteAsync(Program program)
        {
            _context.Programs.Remove(program);
            return Task.CompletedTask;
        }

      

      
    }
}
