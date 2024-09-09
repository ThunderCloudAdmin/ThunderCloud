using Bogus;
using FluentAssertions;
using NSubstitute;

public class TagServiceTests
{
    private readonly ITagRepository _tagRepository;
    private readonly IFileRepository _fileRepository;
    private readonly TagService _tagService;

    public TagServiceTests()
    {
        _tagRepository = Substitute.For<ITagRepository>();
        _fileRepository = Substitute.For<IFileRepository>();
        _tagService = new TagService(_tagRepository, _fileRepository);
    }

    [Fact]
    public async Task AddTagToFileAsync_ShouldAddTag_WhenFileExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tagName = "Important";
        var file = new File { FileId = fileId };

        _fileRepository.GetByIdAsync(fileId).Returns(file);

        // Act
        await _tagService.AddTagToFileAsync(fileId, tagName);

        // Assert
        await _tagRepository.Received(1).AddAsync(Arg.Is<Tag>(t =>
            t.FileId == fileId && t.TagName == tagName));
    }

    [Fact]
    public async Task RemoveTagFromFileAsync_ShouldRemoveTag_WhenTagExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tagName = "OldTag";
        var tag = new Tag { FileId = fileId, TagName = tagName };

        _tagRepository.GetByFileIdAndTagNameAsync(fileId, tagName).Returns(tag);

        // Act
        await _tagService.RemoveTagFromFileAsync(fileId, tagName);

        // Assert
        await _tagRepository.Received(1).DeleteAsync(tag);
    }

    [Fact]
    public async Task GetTagsForFileAsync_ShouldReturnTagsForFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tags = new Faker<string>().Generate(5);

        _tagRepository.GetTagsByFileIdAsync(fileId).Returns(Task.FromResult(tags));

        // Act
        var result = await _tagService.GetTagsForFileAsync(fileId);

        // Assert
        result.Should().BeEquivalentTo(tags);
    }
}
