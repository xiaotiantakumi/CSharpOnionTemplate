using FluentAssertions;
using Template.Domain.Entities;
using Xunit;

namespace Template.Domain.UnitTests.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_ShouldHaveIdProperty()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void BaseEntity_ShouldHaveCreatedAtProperty()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void BaseEntity_ShouldHaveUpdatedAtProperty()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.UpdatedAt.Should().BeNull(); // Initially null until UpdateTimestamp is called
    }

    [Fact]
    public void BaseEntity_ShouldUpdateUpdatedAtWhenModified()
    {
        // Arrange
        var entity = new TestEntity();
        var originalUpdatedAt = entity.UpdatedAt;

        // Act
        Thread.Sleep(10); // Small delay to ensure different timestamp
        entity.UpdateTimestamp();

        // Assert
        entity.UpdatedAt.Should().NotBeNull();
        entity.UpdatedAt!.Value.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    // Test helper class
    private class TestEntity : BaseEntity
    {
        public new void UpdateTimestamp()
        {
            base.UpdateTimestamp();
        }
    }
}
