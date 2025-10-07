using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Contexts;
using Template.Infrastructure.Repositories;
using Xunit;

namespace Template.Infrastructure.IntegrationTests.Repositories;

public class RepositoryTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly Repository<TestEntity> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new Repository<TestEntity>(_context);
    }

    [Fact]
    public async Task Repository_ShouldImplementIRepository()
    {
        // Assert
        _repository.Should().BeAssignableTo<IRepository<TestEntity>>();
    }

    [Fact]
    public async Task Repository_ShouldBeAbleToAddEntity()
    {
        // Arrange
        var entity = new TestEntity { Name = "Test Entity" };

        // Act
        await _repository.AddAsync(entity);
        await _context.SaveChangesAsync();

        // Assert
        var savedEntity = await _context.Set<TestEntity>().FirstOrDefaultAsync(e => e.Id == entity.Id);
        savedEntity.Should().NotBeNull();
        savedEntity!.Name.Should().Be("Test Entity");
    }

    [Fact]
    public async Task Repository_ShouldBeAbleToGetEntityById()
    {
        // Arrange
        var entity = new TestEntity { Name = "Test Entity" };
        await _context.Set<TestEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        var retrievedEntity = await _repository.GetByIdAsync(entity.Id);

        // Assert
        retrievedEntity.Should().NotBeNull();
        retrievedEntity!.Name.Should().Be("Test Entity");
    }

    [Fact]
    public async Task Repository_ShouldBeAbleToGetAllEntities()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new() { Name = "Entity 1" },
            new() { Name = "Entity 2" }
        };

        await _context.Set<TestEntity>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        // Act
        var allEntities = await _repository.GetAllAsync();

        // Assert
        allEntities.Should().HaveCount(2);
        allEntities.Should().Contain(e => e.Name == "Entity 1");
        allEntities.Should().Contain(e => e.Name == "Entity 2");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

// Test entity for repository testing
public class TestEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}

// Test DbContext for testing
public class TestDbContext : ApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(ConvertOptions(options))
    {
    }

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    private static DbContextOptions<ApplicationDbContext> ConvertOptions(DbContextOptions<TestDbContext> options)
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(options.FindExtension<Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal.InMemoryOptionsExtension>()?.StoreName ?? Guid.NewGuid().ToString())
            .Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure TestEntity
        modelBuilder.Entity<TestEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });
    }
}
