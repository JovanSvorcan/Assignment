using Assignment.Domain.Entities;

namespace Assignment.Application.Countries.Queries.GetLocations;
public class CountryDto
{
    public CountryDto()
    {
        Cities = Array.Empty<CityDto>();
    }
    public int Id { get; init; }
    public string? Name { get; init; }
    public IList<CityDto> Cities { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Country, CountryDto>();
        }
    }
}
