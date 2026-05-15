using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Mentora.Application.Interfaces.Repositories
{
    public interface IProgramRepository
    {
        
       
        Task<Program?> GetByIdAsync(int programId);

        Task<IEnumerable<Program>> GetAllAsync(
       Expression<Func<Program, bool>>? filter = null,
       params Expression<Func<Program, object>>[] includes);
     Task<Program?> GetDraftProgramWithDetailsAsync(int programId, params string[] includes);
        Task<IEnumerable<Program>> GetProgramAsync(
      Expression<Func<Program, bool>>? filter = null,
      params string[] includes);
        Task<Program?> GetAsync(
            Expression<Func<Program, bool>> filter,
            params Expression<Func<Program, object>>[] includes);
        Task<int> CountAsync(Expression<Func<Program, bool>> filter = null);
        Task<Program> CreateAsync(Program program);

        Task UpdateAsync(Program program);

        Task DeleteAsync(Program program);

       
    }

}
