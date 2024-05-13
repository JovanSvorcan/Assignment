using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Models;
using Assignment.Application.Common.Security;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<IList<LookupDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IList<LookupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<LookupDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
                .Select(list => new LookupDto { Id = list.Id, Title = list.Title })
                .ToListAsync(cancellationToken);
    }
}
