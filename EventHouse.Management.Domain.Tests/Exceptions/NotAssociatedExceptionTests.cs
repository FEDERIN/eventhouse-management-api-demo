using EventHouse.Management.Domain.Exceptions;

namespace EventHouse.Management.Domain.Tests.Exceptions;

public sealed class NotAssociatedExceptionTests
{
    [Fact]
    public void Exception_ShouldFormatMessageCorrectly()
    {
        // Arrange
        var parent = "Calendar";
        var child = "Artist";
        var pId = Guid.NewGuid();
        var cId = Guid.NewGuid();

        // Act
        var exception = new NotAssociatedException(parent, child, pId, cId);

        // Assert
        var expectedMessage = $"{parent} '{pId}' is not associated with {child} '{cId}'.";
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(parent, exception.Parent);
        Assert.Equal(pId, exception.ParentId);
    }
}
