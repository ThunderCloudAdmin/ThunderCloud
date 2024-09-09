using NSubstitute;

public class RevokeFileAccessEndpointTests
{
    private readonly IFileService _fileService;
    private readonly RevokeFileAccessEndpoint _endpoint;

    public RevokeFileAccessEndpointTests()
    {
        _fileService = Substitute.For<IFileService>();
        _endpoint = new RevokeFileAccessEndpoint
        {
            _fileService = _fileService
        };
    }

    [Fact]
    public async Task HandleAsync_ShouldCallRevokeFileAccessAsync_WhenAccessIsRevoked()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();

        // Act
        await _endpoint.HandleAsync(new RevokeFileAccessRequest { FileId = fileId, RecipientId = recipientId }, CancellationToken.None);

        // Assert
        await _fileService.Received(1).RevokeFileAccessAsync(fileId, recipientId);
    }
}
