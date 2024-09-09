using NSubstitute;

public class ShareFileEndpointTests
{
    private readonly IFileService _fileService;
    private readonly ShareFileEndpoint _endpoint;

    public ShareFileEndpointTests()
    {
        _fileService = Substitute.For<IFileService>();
        _endpoint = new ShareFileEndpoint
        {
            _fileService = _fileService
        };
    }

    [Fact]
    public async Task HandleAsync_ShouldCallShareFileAsync_WhenFileIsShared()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var permission = FilePermission.Read;

        // Act
        await _endpoint.HandleAsync(new ShareFileRequest { FileId = fileId, RecipientId = recipientId, Permission = permission }, CancellationToken.None);

        // Assert
        await _fileService.Received(1).ShareFileAsync(fileId, recipientId, permission);
    }
}
