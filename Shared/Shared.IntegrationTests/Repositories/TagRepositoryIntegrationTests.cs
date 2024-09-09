public class TagRepositoryIntegrationTests : IClassFixture<TestContainersSetup>
{
    private readonly TestContainersSetup _containerSetup;
    private readonly ApplicationDbContext _context;
    private readonly TagRepository _tagRepository;

    public TagRepositoryIntegrationTests(TestContainersSetup containerSetup)
    {
        _containerSetup = containerSetup;
        _context = ApplicationDbContextFactory.Create(_containerSetup.ConnectionString);
        _tagRepository = new TagRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTag()
    {
        // Arrange
        var tag = new Tag { FileId = Guid.NewGuid(), TagName = "Important" };

        // Act
        await _tagRepository.AddAsync(tag);

        // Assert
        var result = await _context.Tags.FirstOrDefaultAsync(t => t.FileId == tag.FileId && t.TagName == tag.TagName);
        result.Should().NotBeNull();
        result.TagName.Should().Be(tag.TagName);
    }

    [Fact]
    public async Task GetByFileIdAndTagNameAsync_ShouldReturnTag_WhenExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tagName = "Important";
        var tag = new Tag { FileId = fileId, TagName = tagName };
        await _tagRepository.AddAsync(tag);

        // Act
        var result = await _tagRepository.GetByFileIdAndTagNameAsync(fileId, tagName);

        // Assert
        result.Should().NotBeNull();
        result.TagName.Should().Be(tagName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTag()
    {
        // Arrange
        var tag = new Tag { FileId = Guid.NewGuid(), TagName = "Important" };
        await _tagRepository.AddAsync(tag);

        // Act
        await _tagRepository.DeleteAsync(tag);

        // Assert
        var result = await _context.Tags.FirstOrDefaultAsync(t => t.FileId == tag.FileId && t.TagName == tag.TagName);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTagsByFileIdAsync_ShouldReturnTagsForFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tags = new Faker<Tag>()
            .RuleFor(t => t.FileId, fileId)
            .RuleFor(t => t.TagName, t => t.Lorem.Word())
            .Generate(5);

        foreach (var tag in tags)
        {
            await _tagRepository.AddAsync(tag);
        }

        // Act
        var result = await _tagRepository.GetTagsByFileIdAsync(fileId);

        // Assert
        result.Should().BeEquivalentTo(tags.Select(t => t.TagName));
    }
}
