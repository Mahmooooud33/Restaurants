using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.IsRestaurantNameExists;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly HttpClient _client;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => 
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _ => _restaurantsRepositoryMock.Object));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Create_RestaurantNameExists_Returns400BadRequest()
    {
        // Arrange
        var url = "/api/restaurants";

        var restaurantName = "Existing Restaurant";
        var request = new IsRestaurantNameExistsQuery(restaurantName);

        var restaurant = new Restaurant()
        {
            Id = 99,
            Name = restaurantName,
            Description = "Test Description",
            Category = "American",
            HasDelivery = true
        };

        _restaurantsRepositoryMock.Setup(m => m.IsRestaurantNameExistsAsync(restaurantName)).ReturnsAsync(true);

        // Act
        var result = await _client.PostAsJsonAsync(url, request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange

        // Act
        var result = await _client.GetAsync("/api/restaurants?pageSize=10&pageNumber=1");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange


        // Act
        var result = await _client.GetAsync("/api/restaurants");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetById_ForNonExistingId_Returns404NotFound()
    {
        // Arrange
        var id = 11245;

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        // Act
        var response = await _client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 99;

        var restaurant = new Restaurant()
        {
            Id = id,
            Name = "New Restaurant",
            Description = "Test Description",
            Category = "American",
            HasDelivery = true
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);

        // Act
        var response = await _client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto!.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
    }  
}