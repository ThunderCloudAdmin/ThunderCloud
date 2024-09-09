using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class FileRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly FileRepository _fileRepository;

    public FileRepositoryTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _fileRepository = new FileRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddFile()
    {
        // Arrange
        var file = new File { FileId = Guid.NewGuid() };

        _context.Files.Add(Arg.Any<File>()).Returns(x => x.Arg<File>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _fileRepository.AddAsync(file);

        // Assert
        await _context.Received(1).Files.Add(file);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFile_WhenFileExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var file = new File { FileId = fileId };

        _context.Files.FindAsync(fileId).Returns(file);

        // Act
        var result = await _fileRepository.GetByIdAsync(fileId);

        // Assert
        result.Should().Be(file);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveFile()
    {
        // Arrange
        var file = new File { FileId = Guid.NewGuid() };

        _context.Files.Remove(Arg.Any<File>()).Returns(x => x.Arg<File>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _fileRepository.DeleteAsync(file);

        // Assert
        await _context.Received(1).Files.Remove(file);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFile()
    {
        // Arrange
        var file = new File { FileId = Guid.NewGuid() };

        _context.Files.Update(Arg.Any<File>()).Returns(x => x.Arg<File>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _fileRepository.UpdateAsync(file);

        // Assert
        await _context.Received(1).Files.Update(file);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetFilesByUserAsync_ShouldReturnFilesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var files = new Faker<FileDto>()
            .RuleFor(f => f.FileId, f => Guid.NewGuid())
            .RuleFor(f => f.FileName, f => f.Lorem.Word())
            .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
            .RuleFor(f => f.UploadDate, f => f.Date.Past())
            .Generate(5);

        _context.Files
            .Where(f => f.UserId == userId)
            .Select(f => new FileDto
            {
                FileId = f.FileId,
                FileName = f.FileName,
                FileSize = f.FileSize,
                UploadDate = f.UploadDate
            })
            .Returns(files);

        // Act
        var result = await _fileRepository.GetFilesByUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(files);
    }

    [Fact]
    public async Task AddFileShareAsync_ShouldAddFileShare()
    {
        // Arrange
        var fileShare = new FileShare();

        _context.FileShares.Add(Arg.Any<FileShare>()).Returns(x => x.Arg<FileShare>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _fileRepository.AddFileShareAsync(fileShare);

        // Assert
        await _context.Received(1).FileShares.Add(fileShare);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetFileShareAsync_ShouldReturnFileShare_WhenExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var fileShare = new FileShare { FileId = fileId, UserId = userId };

        _context.FileShares
            .FirstOrDefaultAsync(fs => fs.FileId == fileId && fs.UserId == userId)
            .Returns(fileShare);

        // Act
        var result = await _fileRepository.GetFileShareAsync(fileId, userId);

        // Assert
        result.Should().Be(fileShare);
    }

    [Fact]
    public async Task DeleteFileShareAsync_ShouldRemoveFileShare()
    {
        // Arrange
        var fileShare = new FileShare();

        _context.FileShares.Remove(Arg.Any<FileShare>()).Returns(x => x.Arg<FileShare>());
        _context.SaveChangesAsync().Returns(Task.CompletedTask);

        // Act
        await _fileRepository.DeleteFileShareAsync(fileShare);

        // Assert
        await _context.Received(1).FileShares.Remove(fileShare);
        await _context.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetSharedFilesByUserAsync_ShouldReturnSharedFiles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var files = new Faker<FileDto>()
            .RuleFor(f => f.FileId, f => Guid.NewGuid())
            .RuleFor(f => f.FileName, f => f.Lorem.Word())
            .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
            .RuleFor(f => f.UploadDate, f => f.Date.Past())
            .Generate(5);

        _context.FileShares
            .Where(fs => fs.UserId == userId)
            .Select(fs => new FileDto
            {
                FileId = fs.File.FileId,
                FileName = fs.File.FileName,
                FileSize = fs.File.FileSize,
                UploadDate = fs.File.UploadDate
            })
            .Returns(files);

        // Act
        var result = await _fileRepository.GetSharedFilesByUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(files);
    }
}
