using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<T?> GetById(object id);
        public Task<IEnumerable<T>> GetAll();
        public Task Create(T entity);
        public Task Update(T entity);
        Task UpdateRange(List<T> entities);

        public Task Delete(T entity);
        public Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate);
    }
}
