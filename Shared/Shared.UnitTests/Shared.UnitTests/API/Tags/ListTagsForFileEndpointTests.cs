using Bogus;
using FluentAssertions;
using NSubstitute;

public class ListTagsForFileEndpointTests
{
    private readonly ITagService _tagService;
    private readonly ListTagsForFileEndpoint _endpoint;

    public ListTagsForFileEndpointTests()
    {
        _tagService = Substitute.For<ITagService>();
        _endpoint = new ListTagsForFileEndpoint
        {
            _tagService = _tagService
        };
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnListOfTags_WhenTagsAreFound()
    {
        // Arrange
        var tags = new Faker<string>().Generate(5);

        _tagService.GetTagsForFileAsync(Arg.Any<Guid>()).Returns(Task.FromResult(tags));

        // Act
        var response = await _endpoint.HandleAsync(new ListTagsForFileRequest { FileId = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        response.Should().BeOfType<ListTagsForFileResponse>();
        response.Tags.Should().HaveCount(5);
    }
}
