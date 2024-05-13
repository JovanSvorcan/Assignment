using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;
using Assignment.Application.TodoLists.Queries.GetTodos;

namespace Assignment.Application.TodoItems.Queries.GetItems;

[Authorize]
public record GetItemsQuery : IRequest<IList<TodoItemDto>>
{
    public int ListId { get; internal set; }

    public GetItemsQuery(int listId)
    {
        this.ListId = listId;
    }
}

public class GetTodosQueryHandler : IRequestHandler<GetItemsQuery, IList<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<TodoItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems
                .Where(item => item.ListId == request.ListId)
                .AsNoTracking()
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken);
    }

}
