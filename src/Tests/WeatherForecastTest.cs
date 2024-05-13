using Assignment.Application.Countries.Queries.GetLocations;
using Assignment.UI;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Assignment.Tests;

[TestFixture]
internal class WeatherForecastTest
{
    private WeatherForecastViewModel _viewModel;
    private Mock<ISender> _mockSender;

    [SetUp]
    public void SetUp()
    {
        _mockSender = new Mock<ISender>();
        _viewModel = new WeatherForecastViewModel(_mockSender.Object);
        _viewModel.Countries = [
            new CountryDto {
                Id = 1,
                Name = "Country1",
                Cities = [
                    new CityDto {Id = 1, Name = "City1", CountryId = 1},
                    new CityDto {Id = 2, Name = "City2", CountryId = 1},
                ]
            },
            new CountryDto {
                Id = 2,
                Name = "Country2",
                Cities = [
                    new CityDto {Id = 3, Name = "City3", CountryId = 2},
                    new CityDto {Id = 4, Name = "City4", CountryId = 2},
                ]
            }
        ];
    }


    [Test]
    public void TestListOfCitiesChangeOnCountryChange()
    {
        _viewModel.SelectedCountry = _viewModel.Countries[0];
        var city1 = _viewModel.Cities[0];

        _viewModel.SelectedCountry = _viewModel.Countries[1];
        var city2 = _viewModel.Cities[0];

        Assert.Multiple(() =>
        {
            Assert.That(city1, Is.Not.Null);

            Assert.That(city2, Is.Not.Null);
        });

        Assert.That(city1.Id, Is.Not.EqualTo(city2.Id));
    }

    [Test]
    public async Task TestTemperatureChangeOnCityChange()
    {
        _viewModel.SelectedCountry = _viewModel.Countries[0];
        Assert.That(_viewModel.Temperature, Is.Null);

        _viewModel.SelectedCity = _viewModel.Cities[0];
        await Task.Delay(1000);
        string temperature1 = _viewModel.Temperature;
        Assert.That(temperature1, Is.Not.Null);

        _viewModel.SelectedCity = _viewModel.Cities[1];
        await Task.Delay(1000);
        string temperature2 = _viewModel.Temperature;

        Assert.Multiple(() =>
        {
            Assert.That(temperature2, Is.Not.Null);

            Assert.That(temperature1, Is.Not.EqualTo(temperature2));
        });
    }
}
