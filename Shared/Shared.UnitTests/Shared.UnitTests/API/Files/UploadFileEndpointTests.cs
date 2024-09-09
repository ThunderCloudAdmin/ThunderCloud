using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;

public class UploadFileEndpointTests
{
    //private readonly IFileService _fileService;
    //private readonly UploadFileEndpoint _endpoint;

    //public UploadFileEndpointTests()
    //{
    //    _fileService = Substitute.For<IFileService>();
    //    _endpoint = new UploadFileEndpoint
    //    {
    //        _fileService = _fileService
    //    };
    //}

    //[Fact]
    //public async Task HandleAsync_ShouldReturnFileId_WhenFileIsUploaded()
    //{
    //    // Arrange
    //    var fileId = Guid.NewGuid();
    //    var fileName = "testfile.txt";
    //    var fakeFile = new FormFile(new MemoryStream(), 0, 0, "file", fileName);
    //    var request = new UploadFileRequest { File = fakeFile, FileName = fileName };

    //    _fileService.UploadFileAsync(Arg.Any<UploadFileRequest>())
    //        .Returns(Task.FromResult(fileId));

    //    // Act
    //    var response = await _endpoint.HandleAsync(request, CancellationToken.None);

    //    // Assert
    //    response.Should().BeOfType<UploadFileResponse>();
    //    response.FileId.Should().Be(fileId);
    //}
}
