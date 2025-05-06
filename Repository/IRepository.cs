using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> includeFunc = null);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllAsync(int id);

    }
}
