using FluentAssertions;
using Template.Application.Handlers;
using Xunit;

namespace Template.Application.UnitTests.Handlers;

public class CommandHandlerTests
{
    [Fact]
    public void ICommandHandler_ShouldBeInterface()
    {
        // Arrange & Act
        var handlerType = typeof(ICommandHandler<,>);

        // Assert
        handlerType.IsInterface.Should().BeTrue();
    }

    [Fact]
    public void ICommandHandler_ShouldHaveHandleMethod()
    {
        // Arrange
        var handlerType = typeof(ICommandHandler<,>);
        var baseInterface = handlerType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IRequestHandler"));

        // Act
        var handleMethod = baseInterface?.GetMethod("Handle");

        // Assert
        handleMethod.Should().NotBeNull();
        handleMethod!.ReturnType.Name.Should().Contain("Task");
    }

    [Fact]
    public void ICommandHandler_ShouldInheritFromIRequestHandler()
    {
        // Arrange
        var handlerType = typeof(ICommandHandler<,>);

        // Act
        var baseInterfaces = handlerType.GetInterfaces();

        // Assert
        baseInterfaces.Should().Contain(i => i.Name.Contains("IRequestHandler"));
    }
}
