using Bogus;
using FluentAssertions;
using NSubstitute;

public class DirectoryServiceTests
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IFileRepository _fileRepository;
    private readonly DirectoryService _directoryService;

    public DirectoryServiceTests()
    {
        _directoryRepository = Substitute.For<IDirectoryRepository>();
        _fileRepository = Substitute.For<IFileRepository>();
        _directoryService = new DirectoryService(_directoryRepository, _fileRepository);
    }

    [Fact]
    public async Task CreateDirectoryAsync_ShouldReturnDirectoryId_WhenDirectoryIsCreated()
    {
        // Arrange
        var directoryId = Guid.NewGuid();
        var directoryName = "TestDir";

        _directoryRepository.AddAsync(Arg.Any<Directory>()).Returns(Task.CompletedTask);

        // Act
        var result = await _directoryService.CreateDirectoryAsync(directoryName, null);

        // Assert
        result.Should().Be(directoryId);
    }

    [Fact]
    public async Task GetUserDirectoriesAsync_ShouldReturnDirectoriesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var directories = new Faker<DirectoryDto>()
            .RuleFor(d => d.DirectoryId, d => Guid.NewGuid())
            .RuleFor(d => d.DirectoryName, d => d.Lorem.Word())
            .Generate(5);

        _directoryRepository.GetDirectoriesByUserAsync(userId).Returns(Task.FromResult(directories));

        // Act
        var result = await _directoryService.GetUserDirectoriesAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(directories);
    }

    [Fact]
    public async Task MoveFileToDirectoryAsync_ShouldUpdateFileDirectory_WhenFileAndDirectoryExist()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var directoryId = Guid.NewGuid();
        var file = new File { FileId = fileId };
        var directory = new Directory { DirectoryId = directoryId };

        _fileRepository.GetByIdAsync(fileId).Returns(file);
        _directoryRepository.GetByIdAsync(directoryId).Returns(directory);

        // Act
        await _directoryService.MoveFileToDirectoryAsync(fileId, directoryId);

        // Assert
        file.DirectoryId.Should().Be(directoryId);
        await _fileRepository.Received(1).UpdateAsync(file);
    }
}
