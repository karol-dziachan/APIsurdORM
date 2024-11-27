using System.Linq.Expressions;
using __ProjectName__.Domain.Entities; 

namespace __ProjectName__.Persistence.Abstractions
{
    public interface I__Entity__Repository
    {
        Task<IEnumerable<__Entity__>> GetAllAsync();
        Task<IEnumerable<__Entity__>> GetAllAsync(int pageIndex, int pageSize);
        Task<__Entity__> GetByIdAsync(Guid id);
        Task<IEnumerable<__Entity__>> FindAsync(Expression<Func<__Entity__, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<__Entity__, bool>> predicate);
        Task<int> CountAsync(Expression<Func<__Entity__, bool>> predicate);
        Task<IEnumerable<__Entity__>> GetByParametersAsync(Dictionary<string, string> parameters);
        Task<Guid> AddAsync(__Entity__ entity);
        Task<int> UpdateAsync(__Entity__ entity, Guid id);
        Task<Guid> RemoveAsync(Guid id);
        Task<int> RemoveByParametersAsync(Dictionary<string, string> parameters);

    }
}
