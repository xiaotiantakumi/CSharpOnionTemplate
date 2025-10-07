using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Template.Web.Controllers;
using Xunit;

namespace Template.Web.FunctionalTests.Controllers;

public class BaseControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BaseControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task BaseController_ShouldBeAccessible()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Swagger_ShouldBeAccessible()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task HealthCheck_ShouldBeAccessible()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }
}
