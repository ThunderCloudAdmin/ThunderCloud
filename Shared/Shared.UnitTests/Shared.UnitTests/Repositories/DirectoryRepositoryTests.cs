using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class DirectoryRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly DirectoryRepository _directoryRepository;

    public DirectoryRepositoryTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _directoryRepository = new DirectoryRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddDirectory()
    {
        // Arrange
        var directory = new Directory { DirectoryId = Guid.NewGuid() };

        _context.Directories.Add(Arg.Any<Directory>()).Returns(x => x.Arg<Directory>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _directoryRepository.AddAsync(directory);

        // Assert
        await _context.Received(1).Directories.Add(directory);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDirectory_WhenExists()
    {
        // Arrange
        var directoryId = Guid.NewGuid();
        var directory = new Directory { DirectoryId = directoryId };

        _context.Directories.FindAsync(directoryId).Returns(directory);

        // Act
        var result = await _directoryRepository.GetByIdAsync(directoryId);

        // Assert
        result.Should().Be(directory);
    }

    [Fact]
    public async Task GetDirectoriesByUserAsync_ShouldReturnDirectoriesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var directories = new Faker<DirectoryDto>()
            .RuleFor(d => d.DirectoryId, d => Guid.NewGuid())
            .RuleFor(d => d.DirectoryName, d => d.Lorem.Word())
            .RuleFor(d => d.ParentDirectoryId, d => d.Random.Guid())
            .Generate(5);

        _context.Directories
            .Where(d => d.UserId == userId)
            .Select(d => new DirectoryDto
            {
                DirectoryId = d.DirectoryId,
                DirectoryName = d.DirectoryName,
                ParentDirectoryId = d.ParentDirectoryId
            })
            .Returns(directories);

        // Act
        var result = await _directoryRepository.GetDirectoriesByUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(directories);
    }
}
