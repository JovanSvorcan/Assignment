
using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;

namespace Assignment.Application.Countries.Queries.GetLocations;

[Authorize]
public record GetLocationsQuery : IRequest<IList<CountryDto>>;
public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IList<CountryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<CountryDto>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries
                .AsNoTracking()
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
    }
}
