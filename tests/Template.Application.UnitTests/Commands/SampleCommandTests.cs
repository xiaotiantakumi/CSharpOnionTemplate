using FluentAssertions;
using Template.Application.Commands;
using Template.Application.Handlers;
using Xunit;

namespace Template.Application.UnitTests.Commands;

public class SampleCommandTests
{
    [Fact]
    public void SampleCommand_ShouldImplementICommand()
    {
        // Arrange & Act
        var command = new SampleCommand("Test Data");

        // Assert
        command.Should().BeAssignableTo<ICommand>();
    }

    [Fact]
    public void SampleCommand_ShouldHaveDataProperty()
    {
        // Arrange
        var testData = "Test Data";

        // Act
        var command = new SampleCommand(testData);

        // Assert
        command.Data.Should().Be(testData);
    }
}

// Sample command for testing
public class SampleCommand : ICommand
{
    public string Data { get; }

    public SampleCommand(string data)
    {
        Data = data;
    }
}
