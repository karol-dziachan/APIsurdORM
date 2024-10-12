using Pr0t0k07.ApiSurdOrm.Domain.Entities;

namespace Pr0t0k07.ApiSurdOrm.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
