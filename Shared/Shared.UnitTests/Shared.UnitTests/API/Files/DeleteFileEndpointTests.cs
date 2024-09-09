using NSubstitute;
using Shared.API.Files;
using Shared.Services.Interfaces;

public class DeleteFileEndpointTests
{
    private readonly IFileService _fileService;
    private readonly DeleteFileEndpoint _endpoint;

    public DeleteFileEndpointTests()
    {
        _fileService = Substitute.For<IFileService>();
        //_endpoint = new DeleteFileEndpoint(_fileService);
    }

    //[Fact]
    //public async Task HandleAsync_ShouldCallDeleteFileAsync_WhenFileIsDeleted()
    //{
    //    // Arrange
    //    var fileId = Guid.NewGuid();

    //    // Act
    //    await _endpoint.HandleAsync(new DeleteFileRequest { FileId = fileId }, CancellationToken.None);

    //    // Assert
    //    await _fileService.Received(1).DeleteFileAsync(fileId);
    //}
}
