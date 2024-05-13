using Assignment.Application.Countries.Queries.GetLocations;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;

public class WeatherForecastViewModel: Screen
{
    private readonly ISender _sender;

    private IList<CountryDto> _countries;
    public IList<CountryDto> Countries
    {
        get
        {
            return _countries;
        }
        set
        {
            _countries = value;
            NotifyOfPropertyChange(() => Countries);
        }
    }

    private CountryDto _selectedCountry;
    public CountryDto SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            NotifyOfPropertyChange(() => SelectedCountry);

            Cities = SelectedCountry.Cities;
            NotifyOfPropertyChange(() => Cities);

            Temperature = null;
            NotifyOfPropertyChange(() => Temperature);
        }
    }

    private IList<CityDto> _cities;
    public IList<CityDto> Cities
    {
        get
        {
            return _cities;
        }
        set
        {
            _cities = value;
            NotifyOfPropertyChange(() => Cities);
        }
    }

    private CityDto _selectedCity;
    public CityDto SelectedCity
    {
        get => _selectedCity;
        set
        {
            _selectedCity = value;
            NotifyOfPropertyChange(() => SelectedCity);

            if (SelectedCity != null)
            {
                Task.Run(GetNewTemperature);
            }
        }
    }

    public string Temperature { get; set; }

    public WeatherForecastViewModel(ISender sender)
    {
        _sender = sender;
        Initialize();
    }

    private async void Initialize()
    {
        Countries = await _sender.Send(new GetLocationsQuery());
    }

    private async Task GetNewTemperature()
    {
        var random = new Random();

        int delay = random.Next(1, 2);
        await Task.Delay(delay * 500);

        int newTemp = random.Next(-40, 40);
        Temperature = newTemp.ToString();
        NotifyOfPropertyChange(() => Temperature);
    }
}

