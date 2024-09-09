using Bogus;
using FluentAssertions;
using NSubstitute;

public class ListSharedFilesEndpointTests
{
    private readonly IFileService _fileService;
    private readonly ListSharedFilesEndpoint _endpoint;

    public ListSharedFilesEndpointTests()
    {
        _fileService = Substitute.For<IFileService>();
        _endpoint = new ListSharedFilesEndpoint
        {
            _fileService = _fileService
        };
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnListOfSharedFiles_WhenFilesAreShared()
    {
        // Arrange
        var files = new Faker<FileDto>()
            .RuleFor(f => f.FileId, f => Guid.NewGuid())
            .RuleFor(f => f.FileName, f => f.Lorem.Word())
            .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
            .RuleFor(f => f.UploadDate, f => f.Date.Past())
            .Generate(5);

        _fileService.GetSharedFilesForUserAsync(Arg.Any<Guid>()).Returns(Task.FromResult(files));

        // Act
        var response = await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        response.Should().BeOfType<ListSharedFilesResponse>();
        response.Files.Should().HaveCount(5);
    }
}
