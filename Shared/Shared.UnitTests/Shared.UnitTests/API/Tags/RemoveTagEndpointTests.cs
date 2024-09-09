using NSubstitute;

public class RemoveTagEndpointTests
{
    private readonly ITagService _tagService;
    private readonly RemoveTagEndpoint _endpoint;

    //public RemoveTagEndpointTests()
    //{
    //    _tagService = Substitute.For<ITagService>();
    //    _endpoint = new RemoveTagEndpoint
    //    {
    //        _tagService = _tagService
    //    };
    //}

    //[Fact]
    //public async Task HandleAsync_ShouldCallRemoveTagFromFileAsync_WhenTagIsRemoved()
    //{
    //    // Arrange
    //    var fileId = Guid.NewGuid();
    //    var tagName = "Old";

    //    // Act
    //    await _endpoint.HandleAsync(new RemoveTagRequest { FileId = fileId, TagName = tagName }, CancellationToken.None);

    //    // Assert
    //    await _tagService.Received(1).RemoveTagFromFileAsync(fileId, tagName);
    //}
}
