using FluentAssertions;
using NSubstitute;
using Shared.API.Directories;
using Shared.Services.Interfaces;

namespace Shared.UnitTests.API.Directories;

public class CreateDirectoryEndpointTests
{
    private readonly IDirectoryService _directoryService;
    private readonly CreateDirectoryEndpoint _endpoint;

    public CreateDirectoryEndpointTests()
    {
        _directoryService = Substitute.For<IDirectoryService>();
        _endpoint = new CreateDirectoryEndpoint(_directoryService);
    }

    //[Fact]
    //public async Task HandleAsync_ShouldReturnDirectoryId_WhenDirectoryIsCreated()
    //{
    //    // Arrange
    //    var directoryId = Guid.NewGuid();
    //    var request = new CreateDirectoryRequest { DirectoryName = "TestDir" };

    //    _directoryService.CreateDirectoryAsync(Arg.Any<string>(), Arg.Any<Guid?>()).Returns(Task.FromResult(directoryId));

    //    // Act
    //    var response = await _endpoint.HandleAsync(request, CancellationToken.None);

    //    // Assert
    //    response.Should().BeOfType<CreateDirectoryResponse>();
    //    response.DirectoryId.Should().Be(directoryId);
    //}
}