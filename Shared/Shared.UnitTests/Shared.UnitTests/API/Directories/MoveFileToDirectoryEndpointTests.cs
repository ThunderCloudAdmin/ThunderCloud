using NSubstitute;
using Shared.API.Directories;
using Shared.Services.Interfaces;

namespace Shared.UnitTests.API.Directories;

public class MoveFileToDirectoryEndpointTests
{
    private readonly IDirectoryService _directoryService;
    private readonly MoveFileToDirectoryEndpoint _endpoint;

    public MoveFileToDirectoryEndpointTests()
    {
        _directoryService = Substitute.For<IDirectoryService>();
        _endpoint = new MoveFileToDirectoryEndpoint(_directoryService);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallMoveFileToDirectoryAsync_WhenFileIsMoved()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var directoryId = Guid.NewGuid();

        // Act
        await _endpoint.HandleAsync(new MoveFileToDirectoryRequest { FileId = fileId, DirectoryId = directoryId }, CancellationToken.None);

        // Assert
        await _directoryService.Received(1).MoveFileToDirectoryAsync(fileId, directoryId);
    }
}