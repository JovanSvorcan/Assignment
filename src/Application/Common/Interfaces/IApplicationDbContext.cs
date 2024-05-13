using Assignment.Domain.Entities;

namespace Assignment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<City> Cities { get; }

    DbSet<Country> Countries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
