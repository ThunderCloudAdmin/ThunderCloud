using Bogus;
using FluentAssertions;
using NSubstitute;

public class ListFilesEndpointTests
{
    //private readonly IFileService _fileService;
    //private readonly ListFilesEndpoint _endpoint;

    //public ListFilesEndpointTests()
    //{
    //    _fileService = Substitute.For<IFileService>();
    //    _endpoint = new ListFilesEndpoint
    //    {
    //        _fileService = _fileService
    //    };
    //}

    //[Fact]
    //public async Task HandleAsync_ShouldReturnListOfFiles_WhenFilesAreFound()
    //{
    //    // Arrange
    //    var files = new Faker<FileDto>()
    //        .RuleFor(f => f.FileId, f => Guid.NewGuid())
    //        .RuleFor(f => f.FileName, f => f.Lorem.Word())
    //        .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
    //        .RuleFor(f => f.UploadDate, f => f.Date.Past())
    //        .Generate(5);

    //    _fileService.GetUserFilesAsync(Arg.Any<Guid>()).Returns(Task.FromResult(files));

    //    // Act
    //    var response = await _endpoint.HandleAsync(CancellationToken.None);

    //    // Assert
    //    response.Should().BeOfType<ListFilesResponse>();
    //    response.Files.Should().HaveCount(5);
    //}
}
