using FluentAssertions;
using System.Net;
using Xunit;
using Moq;
using Template.Functions.Functions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Template.Functions.FunctionalTests.Controllers;

public class HealthControllerTests
{
    private readonly Mock<HealthCheckService> _healthCheckServiceMock;
    private readonly Mock<ILogger<HealthFunctions>> _loggerMock;
    private readonly HealthFunctions _healthFunctions;

    public HealthControllerTests()
    {
        _healthCheckServiceMock = new Mock<HealthCheckService>();
        _loggerMock = new Mock<ILogger<HealthFunctions>>();
        _healthFunctions = new HealthFunctions(_healthCheckServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Health_Get_ShouldReturnOk()
    {
        // Arrange
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                { "database", new HealthReportEntry(HealthStatus.Healthy, "OK", TimeSpan.FromMilliseconds(100), null, null) }
            },
            HealthStatus.Healthy,
            TimeSpan.FromMilliseconds(100));

        _healthCheckServiceMock
            .Setup(x => x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        var httpRequest = CreateMockHttpRequest();

        // Act
        var result = await _healthFunctions.GetHealth(httpRequest);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Microsoft.AspNetCore.Mvc.OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public void Health_Live_ShouldReturnOk()
    {
        // Arrange
        var httpRequest = CreateMockHttpRequest();

        // Act
        var result = _healthFunctions.GetLive(httpRequest);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Microsoft.AspNetCore.Mvc.OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var content = JsonSerializer.Serialize(okResult.Value);
        content.Should().Contain("Alive");
    }

    [Fact]
    public async Task Health_Ready_ShouldReturnOk()
    {
        // Arrange
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                { "database", new HealthReportEntry(HealthStatus.Healthy, "OK", TimeSpan.FromMilliseconds(100), null, null) }
            },
            HealthStatus.Healthy,
            TimeSpan.FromMilliseconds(100));

        _healthCheckServiceMock
            .Setup(x => x.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(healthReport);

        var httpRequest = CreateMockHttpRequest();

        // Act
        var result = await _healthFunctions.GetReady(httpRequest);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Microsoft.AspNetCore.Mvc.OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    private static HttpRequest CreateMockHttpRequest()
    {
        var httpContext = new DefaultHttpContext();
        return httpContext.Request;
    }
}
