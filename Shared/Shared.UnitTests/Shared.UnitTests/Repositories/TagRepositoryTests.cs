using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class TagRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly TagRepository _tagRepository;

    public TagRepositoryTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _tagRepository = new TagRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTag()
    {
        // Arrange
        var tag = new Tag { FileId = Guid.NewGuid(), TagName = "Important" };

        _context.Tags.Add(Arg.Any<Tag>()).Returns(x => x.Arg<Tag>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _tagRepository.AddAsync(tag);

        // Assert
        await _context.Received(1).Tags.Add(tag);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetByFileIdAndTagNameAsync_ShouldReturnTag_WhenExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tagName = "Important";
        var tag = new Tag { FileId = fileId, TagName = tagName };

        _context.Tags
            .FirstOrDefaultAsync(t => t.FileId == fileId && t.TagName == tagName)
            .Returns(tag);

        // Act
        var result = await _tagRepository.GetByFileIdAndTagNameAsync(fileId, tagName);

        // Assert
        result.Should().Be(tag);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTag()
    {
        // Arrange
        var tag = new Tag { FileId = Guid.NewGuid(), TagName = "Important" };

        _context.Tags.Remove(Arg.Any<Tag>()).Returns(x => x.Arg<Tag>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _tagRepository.DeleteAsync(tag);

        // Assert
        await _context.Received(1).Tags.Remove(tag);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetTagsByFileIdAsync_ShouldReturnTagsForFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var tags = new Faker<string>().Generate(5);

        _context.Tags
            .Where(t => t.FileId == fileId)
            .Select(t => t.TagName)
            .Returns(tags);

        // Act
        var result = await _tagRepository.GetTagsByFileIdAsync(fileId);

        // Assert
        result.Should().BeEquivalentTo(tags);
    }
}
