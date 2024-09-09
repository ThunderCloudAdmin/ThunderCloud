using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.API.Files;
using Shared.Services.Interfaces;

namespace Shared.UnitTests.API.Files;

public class DownloadFileEndpointTests
{
    private readonly IFileService _fileService;
    private readonly ILogger<DownloadFileEndpoint> _logger;
    private readonly DownloadFileEndpoint _endpoint;

    public DownloadFileEndpointTests()
    {
        _fileService = Substitute.For<IFileService>();
        _logger = Substitute.For<ILogger<DownloadFileEndpoint>>();
        _endpoint = new DownloadFileEndpoint(_fileService, _logger);
    }

    //[Fact]
    //public async Task HandleAsync_ShouldReturnFileStream_WhenFileIsFound()
    //{
    //    // Arrange
    //    var fileId = Guid.NewGuid();
    //    var fileName = "testfile.txt";
    //    var fileStream = new MemoryStream();
    //    var file = new DownloadFileResponse { FileStream = fileStream, FileName = fileName };

    //    _fileService.GetFileAsync(fileId).Returns(Task.FromResult(file));

    //    // Act
    //    var response = await _endpoint.HandleAsync(new DownloadFileRequest { FileId = fileId }, CancellationToken.None);

    //    // Assert
    //    response.Should().BeOfType<DownloadFileResponse>();
    //    response.FileStream.Should().Be(fileStream);
    //    response.FileName.Should().Be(fileName);
    //}
}