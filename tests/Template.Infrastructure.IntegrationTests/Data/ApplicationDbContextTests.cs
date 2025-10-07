using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Template.Infrastructure.Data.Contexts;
using Xunit;

namespace Template.Infrastructure.IntegrationTests.Data;

public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldBeAbleToConnect()
    {
        // Act
        var canConnect = await _context.Database.CanConnectAsync();

        // Assert
        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldBeAbleToCreateDatabase()
    {
        // Act
        var created = await _context.Database.EnsureCreatedAsync();

        // Assert
        created.Should().BeTrue();
    }

    [Fact]
    public async Task ApplicationDbContext_ShouldBeAbleToDeleteDatabase()
    {
        // Arrange
        await _context.Database.EnsureCreatedAsync();

        // Act
        var deleted = await _context.Database.EnsureDeletedAsync();

        // Assert
        deleted.Should().BeTrue();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
