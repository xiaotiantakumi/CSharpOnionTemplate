using FluentAssertions;
using Template.Application.Queries;
using Xunit;

namespace Template.Application.UnitTests.Queries;

public class SampleQueryTests
{
    [Fact]
    public void SampleQuery_ShouldImplementIQuery()
    {
        // Arrange & Act
        var query = new SampleQuery("Test Filter");

        // Assert
        query.Should().BeAssignableTo<IQuery<string>>();
    }

    [Fact]
    public void SampleQuery_ShouldHaveFilterProperty()
    {
        // Arrange
        var testFilter = "Test Filter";

        // Act
        var query = new SampleQuery(testFilter);

        // Assert
        query.Filter.Should().Be(testFilter);
    }
}

// Sample query for testing
public class SampleQuery : IQuery<string>
{
    public string Filter { get; }

    public SampleQuery(string filter)
    {
        Filter = filter;
    }
}
