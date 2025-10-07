using FluentAssertions;
using Template.Domain.Exceptions;
using Xunit;

namespace Template.Domain.UnitTests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_ShouldCreateWithMessage()
    {
        // Arrange
        var message = "Test domain exception message";

        // Act
        var exception = new TestDomainException(message);

        // Assert
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void DomainException_ShouldCreateWithMessageAndInnerException()
    {
        // Arrange
        var message = "Test domain exception message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new TestDomainException(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void DomainException_ShouldInheritFromException()
    {
        // Arrange & Act
        var exception = new TestDomainException("Test message");

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }

    // Test concrete implementation of abstract DomainException
    private class TestDomainException : DomainException
    {
        public TestDomainException(string message) : base(message)
        {
        }

        public TestDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
