using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Pr0t0k07.APIsurdORM.Templates.Interfaces
{
    public interface IGenericCrudRepository<T> where T : class
    {
        Task<int> Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetByParameters(Dictionary<string, string> parameters);
        Task<T> GetById(int id);
        Task<int> Delete(int id);
        Task<int> Update(T entity, int id);
        Task<PagedResult<T>> GetPaged(int pageNumber, int pageSize);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<int> Count(Expression<Func<T, bool>> predicate);
    }
}
