using Assignment.Application.Cache;
using Assignment.Application.Common.Interfaces;
using Assignment.Application.TodoLists.Queries.GetTodos;

namespace Assignment.Application.TodoItems.Commands.DoneTodoItem;

public record DoneTodoItemCommand(int Id) : IRequest;

public class DoneTodoItemCommandHandler : IRequestHandler<DoneTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DoneTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DoneTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Done = true;

        InMemoryCache<int, IList<TodoItemDto>>.Reset();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
