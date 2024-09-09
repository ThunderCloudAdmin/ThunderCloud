using Bogus;
using FluentAssertions;
using NSubstitute;
using Shared.API.Directories;
using Shared.Services.Interfaces;

namespace Shared.UnitTests.API.Directories;

public class ListDirectoriesEndpointTests
{
    private readonly IDirectoryService _directoryService;
    private readonly ListDirectoriesEndpoint _endpoint;

    public ListDirectoriesEndpointTests()
    {
        _directoryService = Substitute.For<IDirectoryService>();
        _endpoint = new ListDirectoriesEndpoint(_directoryService);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnListOfDirectories_WhenDirectoriesAreFound()
    {
        // Arrange
        var directories = new Faker<DirectoryDto>()
            .RuleFor(d => d.DirectoryId, d => Guid.NewGuid())
            .RuleFor(d => d.DirectoryName, d => d.Lorem.Word())
            .Generate(5);

        _directoryService.GetUserDirectoriesAsync(Arg.Any<Guid>()).Returns(Task.FromResult(directories));

        // Act
        var response = await _endpoint.HandleAsync(CancellationToken.None);

        // Assert
        response.Should().BeOfType<ListDirectoriesResponse>();
        response.Directories.Should().HaveCount(5);
    }
}