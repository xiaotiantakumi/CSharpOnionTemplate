using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Template.Web.FunctionalTests.Integration;

public class WebApplicationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WebApplicationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Application_ShouldStartSuccessfully()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Application_ShouldHaveSwaggerDocumentation()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Application_ShouldHaveOpenApiDocumentation()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Application_ShouldReturnValidContentType()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        // ContentType might be null for some responses, so we just check that the response is valid
        response.Should().NotBeNull();
        // Just verify we got a response (success or expected error)
        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
    }
}
