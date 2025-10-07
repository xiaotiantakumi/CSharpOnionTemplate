using FluentAssertions;
using Template.Application.Handlers;
using Xunit;

namespace Template.Application.UnitTests.Handlers;

public class QueryHandlerTests
{
    [Fact]
    public void IQueryHandler_ShouldBeInterface()
    {
        // Arrange & Act
        var handlerType = typeof(IQueryHandler<,>);

        // Assert
        handlerType.IsInterface.Should().BeTrue();
    }

    [Fact]
    public void IQueryHandler_ShouldHaveHandleMethod()
    {
        // Arrange
        var handlerType = typeof(IQueryHandler<,>);
        var baseInterface = handlerType.GetInterfaces().FirstOrDefault(i => i.Name.Contains("IRequestHandler"));

        // Act
        var handleMethod = baseInterface?.GetMethod("Handle");

        // Assert
        handleMethod.Should().NotBeNull();
        handleMethod!.ReturnType.Name.Should().Contain("Task");
    }

    [Fact]
    public void IQueryHandler_ShouldInheritFromIRequestHandler()
    {
        // Arrange
        var handlerType = typeof(IQueryHandler<,>);

        // Act
        var baseInterfaces = handlerType.GetInterfaces();

        // Assert
        baseInterfaces.Should().Contain(i => i.Name.Contains("IRequestHandler"));
    }
}
