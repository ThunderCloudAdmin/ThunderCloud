using NSubstitute;

public class AddTagEndpointTests
{
    private readonly ITagService _tagService;
    private readonly AddTagEndpoint _endpoint;

    //public AddTagEndpointTests()
    //{
    //    _tagService = Substitute.For<ITagService>();
    //    _endpoint = new AddTagEndpoint
    //    {
    //        _tagService = _tagService
    //    };
    //}

    //[Fact]
    //public async Task HandleAsync_ShouldCallAddTagToFileAsync_WhenTagIsAdded()
    //{
    //    // Arrange
    //    var fileId = Guid.NewGuid();
    //    var tagName = "Important";

    //    // Act
    //    await _endpoint.HandleAsync(new AddTagRequest { FileId = fileId, TagName = tagName }, CancellationToken.None);

    //    // Assert
    //    await _tagService.Received(1).AddTagToFileAsync(fileId, tagName);
    //}
}
